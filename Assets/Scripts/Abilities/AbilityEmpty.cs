using UnityEngine;
using System.Collections;

public class AbilityEmpty : ISpell {
    public override void Cast() {
        Debug.Log("Empty spell");
    }
}
