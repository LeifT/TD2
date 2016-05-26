using UnityEngine;
using System.Collections.Generic;

// ReSharper disable once CheckNamespace
public class UnitGrid : MonoBehaviour, IMessage<UnitsSelectedMessage>, IMessage<UnitOnDisableMessage> {
    public GameObject UnitsPanel;
    public UnitGUI Unit;
    private readonly Dictionary<IUnitFacade, GameObject> _slots = new Dictionary<IUnitFacade, GameObject>();

	void OnEnable () {
        GameManagerComponent.MessageBus.Subscribe<UnitsSelectedMessage>(this);
        GameManagerComponent.MessageBus.Subscribe<UnitOnDisableMessage>(this);
    }
	
	void OnDisable () {
	    GameManagerComponent.MessageBus.Unsubscribe<UnitsSelectedMessage>(this);
	    GameManagerComponent.MessageBus.Unsubscribe<UnitOnDisableMessage>(this);
	}

    public void Handle(UnitsSelectedMessage message) {
        foreach (var slot in _slots) {
            Destroy(slot.Value.gameObject);
        }

        _slots.Clear();

        foreach (var selectedUnit in message.SelectedUnits) {
            var unit = Instantiate(Unit);
            unit.transform.SetParent(UnitsPanel.transform, false);
            unit.UnitFacade = selectedUnit;
            _slots.Add(unit.UnitFacade, unit.gameObject);
        }
    }

    public void Handle(UnitOnDisableMessage message) {
        GameObject unit;

        if (_slots.TryGetValue(message.Unit, out unit)) {
            Destroy(unit);
            _slots.Remove(message.Unit);
        }
    }
}
