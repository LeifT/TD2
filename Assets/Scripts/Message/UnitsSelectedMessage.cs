using System.Collections.Generic;

// ReSharper disable once CheckNamespace
public class UnitsSelectedMessage
{

    public UnitsSelectedMessage(List<IUnitFacade> units)
    {
        //Ensure.ArgumentNotNull(Slots, "Slots");
        SelectedUnits = units;
    }


    public List<IUnitFacade> SelectedUnits
    {
        get;
        private set;
    }
}
