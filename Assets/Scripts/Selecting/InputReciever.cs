using UnityEngine;
using UnityEngine.EventSystems;

// ReSharper disable once CheckNamespace
public class InputReciever : MonoBehaviour {
    private Vector3 _lastSelectedDownPosition;
    private SelectionComponent _selectionComponent;
    // ReSharper disable once UnusedMember.Local
    private void Awake() {
        _selectionComponent = GetComponentInChildren<SelectionComponent>();

        if (_selectionComponent == null) {
            Debug.LogWarning("SelectionComponent is missing");
        }
    }

    // ReSharper disable once UnusedMember.Local
    private void Update() {
        Selection();

        if (Input.GetKeyDown(KeyCode.Tab)) {
            Debug.Log("Pressed TAB");
            return;
        }

        ManageGroups();
    }

    private void Selection() {
        // Abort if _selectionComponent is missig
        if (_selectionComponent == null) {
            return;
        }

        StartSelecting();
        WhileSelecting();
        EndSelecting();
    }

    private void StartSelecting() {
        // On left mouse button down
        if (Input.GetMouseButtonDown(0)) {
            if (EventSystem.current.IsPointerOverGameObject()) {
                return;
            }

            // Screen to world positon
            var plane = new Plane(Vector3.up, Vector3.zero);
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float distance;

            if (plane.Raycast(ray, out distance)) {
                _lastSelectedDownPosition = ray.GetPoint(distance);
            }

            // Start selecting
            _selectionComponent.StartSelect();
        }
    }

    private void WhileSelecting() {
        if (Input.GetMouseButton(0)) {
            // If mouse is over gui element
            if (EventSystem.current.IsPointerOverGameObject()) {
                return;
            }

            if (_selectionComponent.HasSelection(_lastSelectedDownPosition, Input.mousePosition)) {
                // TODO: Implement tentativ selection
            }
        }
    }

    private void EndSelecting() {
        if (Input.GetMouseButtonUp(0)) {
            // If mouse is over gui element
            if (EventSystem.current.IsPointerOverGameObject()) {
                return;
            }

            var mouseUpPosition = Input.mousePosition;
            var appendSelection = Input.GetKey(KeyCode.LeftShift);

            if (_selectionComponent.HasSelection(_lastSelectedDownPosition, mouseUpPosition)) {
                GameManagerComponent.Selection.SelectUnitsBetween(_lastSelectedDownPosition, mouseUpPosition,
                    appendSelection);
            } else {
                GameManagerComponent.Selection.SelectUnit(_lastSelectedDownPosition, appendSelection);
            }

            // End selecting
            _selectionComponent.EndSelect();
        }
    }

    private void ManageGroups() {
        var assignGroup = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.LeftAlt);
        var mergeGroup = Input.GetKey(KeyCode.LeftShift);

        for (var index = 0; index < 5; index++) {
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