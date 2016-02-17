using UnityEngine;
using UnityEngine.EventSystems;

// ReSharper disable once CheckNamespace
public class InputReciever : MonoBehaviour {
    private Vector3 _lastSelectedDownPosition;
    private SelectionComponent _selectionComponent;

    // ReSharper disable once UnusedMember.Local
    private void Awake() {
        _selectionComponent = GetComponentInChildren<SelectionComponent>();
    }

    // ReSharper disable once UnusedMember.Local
    private void Update() {
        Selection();
    }

    private void Selection() {
        var selectAppend = Input.GetKey(KeyCode.LeftShift);

        //if (EventSystem.current.IsPointerOverGameObject()) return;

        if (Input.GetMouseButtonDown(0)) {
            _lastSelectedDownPosition = Input.mousePosition;

            Plane plane = new Plane(Vector3.up, Vector3.zero);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float distance;

            if (plane.Raycast(ray, out distance)) {
                _lastSelectedDownPosition =  ray.GetPoint(distance);
            }

            _selectionComponent.StartSelect();
            return;
        }

        if (Input.GetMouseButton(0)) {
            if (_selectionComponent.HasSelection(_lastSelectedDownPosition, Input.mousePosition)) {
                // TODO: Implement tentativ selection
            }
            return;
        }

        if (Input.GetMouseButtonUp(0)) {
            var mouseUpPonition = Input.mousePosition;

            if (_selectionComponent.HasSelection(_lastSelectedDownPosition, mouseUpPonition)) {
                GameManagerComponent.Instance.GetSelections.SelectUnitsBetween(_lastSelectedDownPosition, mouseUpPonition, selectAppend);
            }
            else {
                GameManagerComponent.Instance.GetSelections.SelectUnit(_lastSelectedDownPosition, selectAppend);
            }

            _selectionComponent.EndSelect();
            return;
        }

        var assignGroup = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.LeftAlt);
        var mergeGroup = Input.GetKey(KeyCode.LeftShift);

        for (int index = 0; index < 5; index++) {
            var code = KeyCode.Alpha1 + index;
            if (Input.GetKeyDown(code)) { 
                if (assignGroup) {
                    GameManagerComponent.Instance.GetSelections.AssignGroup(index);
                } else if (mergeGroup) {
                    GameManagerComponent.Instance.GetSelections.MergeGroup(index);
                } else {
                    GameManagerComponent.Instance.GetSelections.SelectGroup(index);
                }
            }
        }
    }
}