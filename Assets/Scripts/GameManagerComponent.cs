using UnityEngine;

// ReSharper disable once CheckNamespace
public static class GameManagerComponent {
    static GameManagerComponent() {
        GetSelections = new Selections();
    }

    public static Selections GetSelections { get; set; }

    public static void RegisterUnit(GameObject unitGameObject) {
        var unit = unitGameObject.GetComponent<ISelectableUnit>();

        if (unit != null && unit.IsSelectable) {
            GetSelections.AddSelectable(unit);
        }
    }

    /// <summary>
    ///     Unregisters the unit.
    /// </summary>
    /// <param name="unitGameObject">The unit game object.</param>
    public static void UnregisterUnit(GameObject unitGameObject) {
        var unit = unitGameObject.GetComponent<ISelectableUnit>();


        if (unit != null && unit.IsSelectable) {
            GetSelections.RemoveSelectable(unit);
        }


        // _units.Remove(unitGameObject);
    }
}