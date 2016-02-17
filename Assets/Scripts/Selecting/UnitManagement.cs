using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitManagement : MonoBehaviour {
    public HotKey NextSubgroup;
    public HotKey PreviousSubgroup;
    public HotKey Select;
    public HotKey CancelDragSelect;
    public HotKey SelectAll;


    [System.Serializable]
    public class HotKey {
        public KeyCode Key;
        public bool Alt = false;
        public bool Ctrl = false;
        public bool Shift = false;
    }
}
