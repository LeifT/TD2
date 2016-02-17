using UnityEngine;

public class Cell : IHeapItem<Cell> {
    public int GCost;
    public int HCost;
    public Cell Parent;

    public int PosX { get; private set; }
    public int PosZ { get; private set; }
    public Vector3 Position { get; set; }
    public bool Blocked { get; set; }

    #region Constructor

    public Cell(Vector3 position, int gridX, int gridZ, bool blocked) {
        Position = position;
        Blocked = blocked;
        PosX = gridX;
        PosZ = gridZ;
    }

    #endregion

    public int CompareTo(Cell other) {
//                        if (FCost < other.FCost) {
//                            return 1;
//                        }
//                        if (FCost > other.FCost) {
//                            return -1;
//                        }
//                        return HCost.CompareTo(other.HCost);
        var compare = FCost.CompareTo(other.FCost);
        if (compare == 0) {
            compare = HCost.CompareTo(other.HCost);
        }
        return -compare;
    }

    #region Struct

    public struct CellPosition {
        public int X;
        public int Z;
    }

    #endregion

    #region Properties

    public int FCost {
        get { return GCost + HCost; }
    }

    public int HeapIndex { get; set; }

    #endregion
}