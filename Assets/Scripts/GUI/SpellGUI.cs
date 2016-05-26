using System;
using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class SpellGUI : MonoBehaviour, IPointerClickHandler {

    public ISpell Spell;

    public void OnPointerClick(PointerEventData eventData) {
        if (Spell != null) {
            Spell.Cast();
        }
    }
}
