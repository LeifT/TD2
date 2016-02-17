using UnityEngine;
// ReSharper disable UnusedMember.Local
// ReSharper disable once CheckNamespace
public class EnemyComponent : ISelectableUnit {
    private bool _isSelected;
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

    private void OnEnable() {
        _isSelected = true;
        IsSelected = false;
        GameManagerComponent.GetSelections.AddSelectable(gameObject);
    }

    private void OnDisable() {
        //if (GameManagerComponent.Instance != null) {
        //    GameManagerComponent.GetSelections.RemoveSelectable(gameObject);
        //}
        
        EnemyManager.Instance.GetEnemies().Remove(gameObject);
    }
}