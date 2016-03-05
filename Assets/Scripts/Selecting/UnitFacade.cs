using UnityEngine;

// ReSharper disable once CheckNamespace
public class UnitFacade : IUnitFacade {
    public bool IsSelected {
        get {
            return _props.IsSelected;
        }
        set { _props.IsSelected = value; }
    }

    public bool IsSelectable { get { return _props.IsSelectable; } }
    public Transform Transform { get; private set; }
    public Collider Collider { get; private set; }
    public GameObject GameObject { get; private set; }

    private IUnitProperties _props;

    public void MarkSelectPending(bool pending) {
        _props.MarkSelectPending(pending);
    }
    
    public void Initialize(GameObject unitObject) {
        _props = unitObject.As<IUnitProperties>(false, true);

        GameObject = unitObject;
        Transform = unitObject.transform;
        Collider = unitObject.GetComponent<Collider>();
        IsSelected = false;
    }
}
