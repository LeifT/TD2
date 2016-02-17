using System.Collections.Generic;
using UnityEngine;

// ReSharper disable once CheckNamespace
public class ControlGroups : MonoBehaviour
{
    public KeyModifiers Select;
    public KeyModifiers Add;
    public KeyModifiers AddAndRemove;
    public KeyModifiers Assign;
    public KeyModifiers AssignAndRemove;
    public List<KeyCode> Groups;

    [System.Serializable]
    public class KeyModifiers
    {
        public bool Enable = true;
        public bool Alt = false;
        public bool Ctrl = false;
        public bool Shift = false;
    }
}