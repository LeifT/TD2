using UnityEngine;

[ExecuteInEditMode]
// ReSharper disable once CheckNamespace
public class GridVisuals : MonoBehaviour {
    //private DateTime? _nextRefresh;
    public bool DrawAlways;
    public bool DrawObstacles;
    public int EditorRefreshDelay = 100;

    public Color BoundsColor = Color.gray;
    public Color GridLineColor = new Color(135f/255f, 135f/255f, 135f/255f);
    public Color ObstacleColor = new Color(226f/255f, 41f/255f, 32f/255f, 150f/255f);

    private void DrawVisualization() {
        var grid = GetComponent<GridComponent>();

        if (grid == null) {
            return;
        }

        if (grid.enabled) {
            DrawGrid(grid);
        }
    }

    private void DrawGrid(GridComponent grid) {
        if (grid.SizeX == 0 || grid.SizeZ == 0 || grid.CellSize < 0.1f) {
            return;
        }

        DrawLayout(grid);
        DrawArrows(grid);
        //return true;
    }
    
    private void DrawArrows(GridComponent grid) {
        if (!Application.isPlaying) {
            return;
        }

        Gizmos.color = Color.white;

        foreach (var vector in grid.Grid) {
            if (vector.Parent != null) {
                DrawArrow.Arrow(vector.Position, vector.Parent.Position);
            }
        }
    }

    private void DrawLayout(GridComponent grid) {
        Gizmos.color = GridLineColor;
        var step = grid.CellSize;
        var halfCell = step * .5f ;
        var xMin = transform.position.x - grid.SizeX * halfCell;
        var xMax = transform.position.x + grid.SizeX * halfCell;
        var zMin = transform.position.z - grid.SizeZ * halfCell;
        var zMax = transform.position.z + grid.SizeZ * halfCell;
        var y = transform.position.z + 0.05f;

        Gizmos.color = BoundsColor;
        Gizmos.DrawWireCube(transform.position, new Vector3(grid.SizeX * step, 0, grid.SizeZ * step));
        Gizmos.color = GridLineColor;

        for (var x = xMin + step; x <= xMax -step ; x += step) {
            Gizmos.DrawLine(new Vector3(x, y, zMin), new Vector3(x, y, zMax));
        }

        for (var z = zMin + step; z <= zMax - step; z += step) {
            Gizmos.DrawLine(new Vector3(xMin, y, z), new Vector3(xMax, y, z));
        }

        if (!Application.isPlaying) {
            return;
        }

        if (DrawObstacles) {
            Gizmos.color = ObstacleColor;

            foreach (var cell in grid.Grid) {
                if (cell.Blocked) {
                    Gizmos.DrawCube(cell.Position, new Vector3(step, 0.5f, step));
                }
            }
        }
    }

    // ReSharper disable once UnusedMember.Local
    private void Update() {
        if (Application.isEditor && !Application.isPlaying) {
            //_nextRefresh = DateTime.UtcNow.AddMilliseconds(EditorRefreshDelay);
        }
    }

    public void Refresh() {
        DrawVisualization();
    }

    // ReSharper disable once UnusedMember.Local
    private void OnDrawGizmos() {
        if (DrawAlways && enabled) {
            DrawVisualization();
        }
    }

    // ReSharper disable once UnusedMember.Local
    private void OnDrawGizmosSelected() {
        if (!DrawAlways && enabled) {
            DrawVisualization();
        }
    }
}