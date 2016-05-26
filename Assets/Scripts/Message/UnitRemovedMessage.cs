using UnityEngine;
using System.Collections;

public class UnitRemovedMessage {
    public UnitRemovedMessage(IUnitFacade unit) {
        Unit = unit;
    }

    public IUnitFacade Unit { get; private set; }
}
