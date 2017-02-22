using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ReSharper disable once CheckNamespace
public class UnitGrid : MonoBehaviour, IMessage<SelectionChangedMessage> {
    private readonly Dictionary<IUnitFacade, GameObject> _units = new Dictionary<IUnitFacade, GameObject>();

    [SerializeField]
    private UnitGUI _unit;
    
    private void OnEnable() {
        GameManagerComponent.MessageBus.Subscribe(this);
    }

    private void OnDisable() {
        GameManagerComponent.MessageBus.Unsubscribe(this);
    }
    
    public void Handle(SelectionChangedMessage message) {
        foreach (var unitFacade in message.Removed) {
            Destroy(_units[unitFacade]);
            _units.Remove(unitFacade);
            
        }

        message.Added.Sort((unit1, unit2) => unit2.Priority.CompareTo(unit1.Priority));

        for (int i = 0; i < message.Added.Count; i++) {
            var unit = Instantiate(_unit);
            unit.transform.SetParent(transform, false);
            unit.UnitFacade = message.Added[i];

            unit.transform.GetChild(0).GetComponent<Image>().sprite = message.Added[i].Icon;
            _units.Add(unit.UnitFacade, unit.gameObject);
        }
    }
}