using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

// ReSharper disable once CheckNamespace
public class UnitGUI : MonoBehaviour, IPointerClickHandler
{
    public IUnitFacade UnitFacade;

    public void OnPointerClick(PointerEventData eventData) {
        GameManagerComponent.Selection.Select(UnitFacade, false);
    }
}

