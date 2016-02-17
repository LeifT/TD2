using UnityEngine;

// ReSharper disable once CheckNamespace
public static class GameManagerComponent  {
    private static readonly Selections Selections;

    static GameManagerComponent() {
        Selections = new Selections();
    }

    public static Selections GetSelections {
        get { return Selections; }
    }


}
