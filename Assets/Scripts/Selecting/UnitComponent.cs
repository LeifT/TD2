using UnityEngine;

// ReSharper disable UnusedMember.Local
// ReSharper disable once CheckNamespace
public class UnitComponent : MonoBehaviour, IUnitProperties{
    private bool _isSelected;
    private bool? _selectPending;
    public GameObject SelectionVisual;
    
    public bool IsSelected {
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

    [SerializeField]
    private bool _isSelectable;

    [SerializeField]
    private int _priority;

    [SerializeField]
    private int _group;

    [SerializeField]
    private Sprite _sprite;


    public bool IsSelectable { get {return _isSelectable;} set { _isSelectable = value; } }
    //public Vector3 transform { get; set; }

    private void OnEnable() {
        _isSelected = true;
        IsSelected = false;
        GameManagerComponent.RegisterUnit(gameObject);
        //EnemyManager.Instance.
    }

    private void OnDisable() {
        //EnemyManager.Instance.GetEnemies().Remove(gameObject);
        GameManagerComponent.UnregisterUnit(gameObject);
    }

    //private void OnDestroy()
    //{
    //    GameManagerComponent.UnregisterUnit(gameObject);
    //}

    public void MarkSelectPending(bool pending) {
        if (_selectPending != pending) {
            _selectPending = pending;

            if (SelectionVisual != null) {
                SelectionVisual.SetActive(pending);
            }
        }
    }

    public int Group { get { return _group; } set { _group = value; } }
    public int Priority { get {return _priority;} set { _priority = value; } }
    public Sprite Icon { get {return _sprite;} set { _sprite = value; } }
}