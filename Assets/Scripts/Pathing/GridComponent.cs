using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GridComponent : MonoBehaviour {

    public Cell[,] Grid;

    public float CellSize = 2;
    public bool CutCorner = false;
    public int SizeX = 10;
    public int SizeZ = 10;

    public int MaxSize {
        get { return SizeX*SizeZ; }
    }

    private void Awake() {
        CreateGrid();
    }

    private void CreateGrid() {
        Grid = new Cell[SizeX, SizeZ];
        var worldBottomLeft = transform.position - new Vector3(SizeX/2f*CellSize, 0, SizeZ / 2f * CellSize);

        for (var x = 0; x < SizeX; x++) {
            for (var z = 0; z < SizeZ; z++) {
                var worldPoint = worldBottomLeft + new Vector3(x*CellSize + CellSize/2, 0, z * CellSize + CellSize / 2);
                Grid[x, z] = new Cell(worldPoint, x, z, false);
            }
        }
    }

    //public void UpdateNodePosition() {
    //    var worldBottomLeft = transform.position - new Vector3(SizeX/2f*CellSize, 0, SizeZ / 2f * CellSize);

    //    for (var x = 0; x < SizeX; x++) {
    //        for (var z = 0; z < SizeZ; z++) {
    //            Grid[x, z].Position = worldBottomLeft + new Vector3(x*CellSize + CellSize/2, 0, z * CellSize + CellSize / 2);
    //            Grid[x, z].Blocked = false;
    //            Grid[x, z].Parent = null;
    //        }
    //    }
    //}

    public bool InBounds( Vector3 pos) {
        if (pos.x < transform.position.x - SizeX/2f) return false;
        if (pos.x > transform.position.x + SizeX/2f) return false;
        if (pos.z < transform.position.z - SizeZ/2f) return false;
        if (pos.z > transform.position.z + SizeZ/2f) return false;

        return true;
    }

    private bool InBounds(Cell cell) {
        return cell.PosX >= 0 && cell.PosX < SizeX && cell.PosZ >= 0 && cell.PosZ < SizeZ;
    }

    private bool TryGetWalkableNeighbour(int dx, int dz, Cell cell, List<Cell> neighbours) {
        var x = cell.PosX + dx;
        var z = cell.PosZ + dz;

        if (x < 0 || x >= SizeX || z < 0 || z >= SizeZ) {
            return false;
        }

        if (Grid[x, z].Blocked) {
            return false;
        }

        neighbours.Add(Grid[x, z]);
        return true;
    }

    public List<Cell> GetNeighbours(Cell cell) {
        var neighbours = new List<Cell>();

        var up = TryGetWalkableNeighbour(0, 1, cell, neighbours);
        var down = TryGetWalkableNeighbour(0, -1, cell, neighbours);
        var left = TryGetWalkableNeighbour(-1, 0, cell, neighbours);
        var right = TryGetWalkableNeighbour(1, 0, cell, neighbours);

        bool upRight, downRight, downLeft, upLeft;

        if (CutCorner) {
            downRight = down || right;
            downLeft = down || left;
            upRight = up || right;
            upLeft = up || left;
        }
        else {
            downRight = down && right;
            downLeft = down && left;
            upRight = up && right;
            upLeft = up && left;
        }

        if (upRight) {
            TryGetWalkableNeighbour(1, 1, cell, neighbours);
        }

        if (downRight) {
            TryGetWalkableNeighbour(1, -1, cell, neighbours);
        }

        if (downLeft) {
            TryGetWalkableNeighbour(-1, -1, cell, neighbours);
        }

        if (upLeft) {
            TryGetWalkableNeighbour(-1, 1, cell, neighbours);
        }
        return neighbours;
    }

    public Cell NodeFromWorldPoint(Vector3 position) {
        if (!InBounds(position)) return null;

        var start = transform.position - new Vector3(SizeX / 2f * CellSize, 0, SizeZ / 2f * CellSize);

        var fx = (position.x - start.x) / CellSize;
        var fz = (position.z - start.z) / CellSize;

        var x = Mathf.FloorToInt(fx);
        var z = Mathf.FloorToInt(fz);

        return Grid[x, z];
    }
}