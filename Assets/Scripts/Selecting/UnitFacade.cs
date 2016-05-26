using System;
using UnityEngine;

// ReSharper disable once CheckNamespace
public class UnitFacade : IUnitFacade {
    public bool IsSelected {
        get {
            return _props.IsSelected;
        }
        set { _props.IsSelected = value; }
    }

    public bool IsSelectable { get { return _props.IsSelectable; } set { _props.IsSelectable = value; } }
    public Transform Transform { get; private set; }
    public Collider Collider { get; private set; }
    public GameObject GameObject { get; private set; }

    private IUnitProperties _props;

    public void MarkSelectPending(bool pending) {
        _props.MarkSelectPending(pending);
    }

    public int Group { get; set; }
    public int Priority { get; set; }
    public Sprite Icon { get; set; }

    public void Initialize(GameObject unitObject) {
        _props = unitObject.As<IUnitProperties>(false, true);

        GameObject = unitObject;
        Transform = unitObject.transform;
        Collider = unitObject.GetComponent<Collider>();

        Priority = _props.Priority;
        Group = _props.Group;
        Icon = _props.Icon;

        IsSelected = false;
    }
}
