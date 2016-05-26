using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// ReSharper disable once CheckNamespace
public class UnitGUI : MonoBehaviour, IPointerClickHandler
{
    public IUnitFacade UnitFacade;
    public Image ImageComponent;


    public void OnPointerClick(PointerEventData eventData) {
        if (Input.GetKey(KeyCode.LeftShift)) {
            GameManagerComponent.Selection.ToggleSelected(UnitFacade, true);
        }
        else {
            GameManagerComponent.Selection.Select(UnitFacade, false);
        }
    }
}

