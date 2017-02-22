using System.Collections.Generic;

public class UnitsSelectedMessage {

    public UnitsSelectedMessage(List<IUnitFacade> units) {
        SelectedUnits = units;
    }

    public List<IUnitFacade> SelectedUnits {
        get;
        private set;
    }
}
