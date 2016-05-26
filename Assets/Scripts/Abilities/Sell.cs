using System;
using UnityEngine;
using UnityEngine.EventSystems;

[Serializable]
public class Sell : ISpell {
    public override void Cast() {

        var selectedUnits = GameManagerComponent.Selection.Selected;

        for (int i = selectedUnits.Count - 1; i >= 0; i--) {
            GameObject.Find("Grid")
                .GetComponent<GridManager>()
                .GridComponent.NodeFromWorldPoint(selectedUnits[i].Transform.position)
                .Blocked = false;
            Destroy(selectedUnits[i].GameObject);
        }

        GameObject.Find("Grid").GetComponent<GridManager>().FindPath();
        //Destroy(Parent);
    }
}