using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellGrid : MonoBehaviour, IMessage<UnitsSelectedMessage>, IMessage<UnitOnDisableMessage> {
    private readonly List<SpellGUI> _spells = new List<SpellGUI>();
    public SpellGUI Spell;

    public void Handle(UnitOnDisableMessage message) {}

    public void Handle(UnitsSelectedMessage message) {
        if (message.SelectedUnits.Count > 0) {
            var spellComponent = message.SelectedUnits[0].GameObject.GetComponent<SpellComponent>();

            if (spellComponent == null) {
                return;
            }


            for (var i = 0; i < 9; i++) {
                if (spellComponent.spells[i].Name != null) {
                    _spells[i].GetComponent<Image>().sprite = spellComponent.spells[i].Icon;
                    _spells[i].Spell = spellComponent.spells[i];
                }
            }
        }
        else {
            Debug.Log("Empty");
        }
    }

    private void OnEnable() {
        GameManagerComponent.MessageBus.Subscribe<UnitsSelectedMessage>(this);
        GameManagerComponent.MessageBus.Subscribe<UnitOnDisableMessage>(this);
    }

    private void OnDisable() {
        GameManagerComponent.MessageBus.Unsubscribe<UnitsSelectedMessage>(this);
        GameManagerComponent.MessageBus.Unsubscribe<UnitOnDisableMessage>(this);
    }

    // Use this for initialization
    private void Start() {
        for (var i = 0; i < 9; i++) {
            _spells.Add(Instantiate(Spell));
            _spells[i].transform.SetParent(transform, false);
        }
    }

    // Update is called once per frame
    private void Update() {}
}