using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;


public class SpellComponent : MonoBehaviour {
    public ISpell[] spells = new ISpell[9];

    void Start() {
        for (int i = 0; i < 9; i++) {
            if (spells[i] == null) {
                spells[i] = ScriptableObject.CreateInstance("AbilityEmpty") as ISpell;
            }
        }
    }
}
