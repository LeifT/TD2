using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Enumerable = System.Linq.Enumerable;

// ReSharper disable once CheckNamespace
public class UnitGrid : MonoBehaviour, IMessage<UnitsSelectedMessage>, IMessage<UnitRemovedMessage> {
    private readonly List<GameObject> _slots = new List<GameObject>();
    private readonly List<Image> _images = new List<Image>(); 

    private readonly Dictionary<IUnitFacade, GameObject> _units = new Dictionary<IUnitFacade, GameObject>();
    public UnitGUI Unit;
    //public GameObject UnitsPanel;

    public void Handle(UnitRemovedMessage message) {
        GameObject unit;

        if (_units.TryGetValue(message.Unit, out unit)) {
            Destroy(unit);
            _units.Remove(message.Unit);
        }
    }

    public void Handle(UnitsSelectedMessage message) {
        var units = message.SelectedUnits;

        ClearSlots();
        
        for (int i = 0; i < units.Count; i++) {
            var unit = Instantiate(Unit);
            unit.transform.SetParent(_slots[i].transform, false);
            unit.UnitFacade = units[i];
            _units.Add(unit.UnitFacade, unit.gameObject);
            unit.ImageComponent.sprite = units[i].Icon;
            _images[i].enabled = true;
            
        }
    }

    private int GetEmptyIndex() {
        for (int i = 0; i < _slots.Count; i++) {

            if (_slots[i].transform.childCount < 1) {
                 return i;
            }
        }
        return -1;
    } 

    private void Start() {
        foreach (Transform child in transform) {
            _slots.Add(child.gameObject);
            var imageComponent = child.gameObject.GetComponent<Image>();
            _images.Add(imageComponent);
            imageComponent.enabled = false;
        }
    }

    private void OnEnable() {
        GameManagerComponent.MessageBus.Subscribe<UnitsSelectedMessage>(this);
        GameManagerComponent.MessageBus.Subscribe<UnitRemovedMessage>(this);
    }

    private void OnDisable() {
        GameManagerComponent.MessageBus.Unsubscribe<UnitsSelectedMessage>(this);
        GameManagerComponent.MessageBus.Unsubscribe<UnitRemovedMessage>(this);
    }

    private void ClearSlots() {
        _units.Clear();

        for (int i = 0; i < _slots.Count; i++) {
            foreach (Transform child in _slots[i].transform) {
                Destroy(child.gameObject);
            }

            _images[i].enabled = false;
        }
    }
}