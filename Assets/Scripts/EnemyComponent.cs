using UnityEngine;

// ReSharper disable UnusedMember.Local
// ReSharper disable once CheckNamespace
public class EnemyComponent : ISelectableUnit {
    private bool _isSelected;
    private bool? _selectPending;
    public GameObject SelectionVisual;

    public override bool IsSelected {
        get { return _isSelected; }
        set {
            if (_isSelected != value) {
                _isSelected = value;

                if (SelectionVisual != null) {
                    SelectionVisual.SetActive(value);
                }
            }
        }
    }

    public override bool IsSelectable { get; set; }
    //public Vector3 transform { get; set; }

    private void OnEnable() {
        _isSelected = true;
        IsSelected = false;
        IsSelectable = true;
        GameManagerComponent.RegisterUnit(gameObject);
    }

    private void OnDisable() {
        EnemyManager.Instance.GetEnemies().Remove(gameObject);
    }

    public override void MarkSelectPending(bool pending) {
        if (_selectPending != pending) {
            _selectPending = pending;

            if (SelectionVisual != null) {
                SelectionVisual.SetActive(pending);
            }
        }
    }
}