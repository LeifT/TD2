using System.Collections.Generic;

public class SelectionChangedMessage {
    public SelectionChangedMessage(List<IUnitFacade> units, List<IUnitFacade> added, List<IUnitFacade> removed) {
        SelectedUnits = units;
        Added = added;
        Removed = removed;
    }

    public List<IUnitFacade> SelectedUnits {
        get;
        private set;
    }

    public List<IUnitFacade> Added {
        get;
        private set;
    }

    public List<IUnitFacade> Removed {
        get;
        private set;
    }
}
