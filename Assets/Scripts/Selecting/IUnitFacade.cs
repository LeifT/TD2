using UnityEngine;

public interface IUnitFacade : IUnitProperties {
    Transform Transform { get; }
    Collider Collider { get; }
    GameObject GameObject { get; }
    void Initialize(GameObject unitObject);
}
