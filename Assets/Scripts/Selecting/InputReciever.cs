using UnityEngine;
using UnityEngine.EventSystems;

// ReSharper disable once CheckNamespace
public class InputReciever : MonoBehaviour {
    private Vector3 _lastSelectedDownPosition;
    private SelectionComponent _selectionComponent;

    // ReSharper disable once UnusedMember.Local
    private void Awake() {
        _selectionComponent = GetComponentInChildren<SelectionComponent>();

        if (_selectionComponent == null)
        {
            // TODO: Change text
            Debug.LogWarning("Missing SelectionRectangleComponent, this is required by the input receiver to handle unit selection.");
        }
    }

    // ReSharper disable once UnusedMember.Local
    private void Update() {
        Selection();
    }

    private void Selection() {
        // Abort if _selectionComponent is missig
        if (_selectionComponent == null) {
            return;
        }

        var selectAppend = Input.GetKey(KeyCode.LeftShift);

        if (Input.GetMouseButtonDown(0)) {
            if (EventSystem.current.IsPointerOverGameObject()) {
                return;
            }

            _lastSelectedDownPosition = Input.mousePosition;

            // Screen to worldpos
            Plane plane = new Plane(Vector3.up, Vector3.zero);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float distance;

            if (plane.Raycast(ray, out distance)) {
                _lastSelectedDownPosition =  ray.GetPoint(distance);
            }

            // Start select
            _selectionComponent.StartSelect();
            return;
        }

        if (Input.GetMouseButton(0)) {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }
            if (_selectionComponent.HasSelection(_lastSelectedDownPosition, Input.mousePosition)) {
                // TODO: Implement tentativ selection
            }
            return;
        }

        if (Input.GetMouseButtonUp(0)) {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }
            var mouseUpPonition = Input.mousePosition;

            if (_selectionComponent.HasSelection(_lastSelectedDownPosition, mouseUpPonition)) {
                GameManagerComponent.Selection.SelectUnitsBetween(_lastSelectedDownPosition, mouseUpPonition, selectAppend);
            }
            else {
                GameManagerComponent.Selection.SelectUnit(_lastSelectedDownPosition, selectAppend);
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
                    GameManagerComponent.Selection.AssignGroup(index);
                } else if (mergeGroup) {
                    GameManagerComponent.Selection.MergeGroup(index);
                } else {
                    GameManagerComponent.Selection.SelectGroup(index);
                }
            }
        }
    }
}