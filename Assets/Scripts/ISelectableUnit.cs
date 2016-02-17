using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class ISelectableUnit : MonoBehaviour {
    private List<string> list = new List<string>();

    public void a() {
       
        list.IndexOf("a");
    }

    public abstract bool IsSelected { get; set; }
}
