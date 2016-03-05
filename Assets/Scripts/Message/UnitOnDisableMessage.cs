using UnityEngine;
using System.Collections;

public class UnitOnDisableMessage {
    public UnitOnDisableMessage(IUnitFacade unit) {
        Unit = unit;
    }

    public IUnitFacade Unit { get; private set; }
}
