using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Enumerable = System.Linq.Enumerable;

// ReSharper disable once CheckNamespace
public class UnitGrid : MonoBehaviour, IMessage<UnitsSelectedMessage>, IMessage<UnitRemovedMessage> {

    private readonly Dictionary<IUnitFacade, GameObject> _units = new Dictionary<IUnitFacade, GameObject>();
    public UnitGUI Unit;

    public void Handle(UnitRemovedMessage message) {
        GameObject unit;

        if (_units.TryGetValue(message.Unit, out unit)) {
            Destroy(unit);
            _units.Remove(message.Unit);
        }
    }

    public void Handle(UnitsSelectedMessage message) {
        var units = message.SelectedUnits;

        units.Sort((unit1, unit2) => unit2.Priority.CompareTo(unit1.Priority));
        ClearSlots();
        
        for (int i = 0; i < units.Count; i++) {
            var unit = Instantiate(Unit);
            unit.transform.SetParent(transform, false);
            unit.UnitFacade = units[i];

            unit.transform.GetChild(0).GetComponent<Image>().sprite = units[i].Icon;
            _units.Add(unit.UnitFacade, unit.gameObject);
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
        
        foreach (Transform child in transform) {
            Destroy(child.gameObject);
        }
    }
}