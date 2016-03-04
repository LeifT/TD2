using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class ISelectableUnit : MonoBehaviour {
    public abstract bool IsSelected { get; set; }

    public abstract bool IsSelectable { get; set; }

    public abstract void MarkSelectPending(bool pending);
}
