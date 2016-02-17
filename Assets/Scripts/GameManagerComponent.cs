using UnityEngine;

// ReSharper disable once CheckNamespace
public class GameManagerComponent : Singleton<GameManagerComponent> {
    private Selections _selections;

    public Selections GetSelections {
        get { return _selections ?? (_selections = new Selections()); }
    }


}
