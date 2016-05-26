using System.Collections.Generic;
using Assets.Scripts.Message;
using UnityEngine;

public class SpellGrid : MonoBehaviour, IMessage<UnitsSelectedMessage>, IMessage<UnitRemovedMessage>,
    IMessage<UnitTypeSelectionMessage> {
    private readonly List<GameObject> _slots = new List<GameObject>();
    //private readonly List<SpellGUI> _spells = new List<SpellGUI>();
    public SpellGUI Spell;

    public void Handle(UnitRemovedMessage message) {}

    public void Handle(UnitsSelectedMessage message) {}

    public void Handle(UnitTypeSelectionMessage message) {

        var spellcomponent = message.Unit.GameObject.GetComponent<SpellComponent>();

        if (spellcomponent != null) {
            SetSpells(spellcomponent.spells);
        }
    }

    private void OnEnable() {
        GameManagerComponent.MessageBus.Subscribe<UnitsSelectedMessage>(this);
        GameManagerComponent.MessageBus.Subscribe<UnitRemovedMessage>(this);
        GameManagerComponent.MessageBus.Subscribe<UnitTypeSelectionMessage>(this);
    }

    private void OnDisable() {
        GameManagerComponent.MessageBus.Unsubscribe<UnitsSelectedMessage>(this);
        GameManagerComponent.MessageBus.Unsubscribe<UnitRemovedMessage>(this);
        GameManagerComponent.MessageBus.Unsubscribe<UnitTypeSelectionMessage>(this);
    }

    // Use this for initialization
    private void Start() {
        foreach (Transform child in transform) {
            _slots.Add(child.gameObject);
        }
    }

    private void ClearSlots() {
        foreach (var ability in _slots) {
            foreach (Transform child in ability.transform) {
                Destroy(child.gameObject);
            }
        }
    }

    private void SetSpells(ISpell[] spells) {
        ClearSlots();

        for (var i = 0; i < spells.Length; i++) {
            if (spells[i] != null) {
                var temp = Instantiate(Spell);
                temp.transform.SetParent(_slots[i].transform, false);
            }
        }
    }
}