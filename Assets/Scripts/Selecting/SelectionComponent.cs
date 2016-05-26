using UnityEngine;

public class SelectionComponent : MonoBehaviour {
    private Transform _selectionVisual;
    private Camera _selectionVisualCamera;
    public float StaltDeltaThreshold = 3.0f;

    private void Awake() {
        _selectionVisualCamera = GetComponentInChildren<Camera>();
        _selectionVisual = GetComponentInChildren<MeshRenderer>().transform;
        ToggleEnable(false);
    }

    private void ToggleEnable(bool unitEnabled) {
        _selectionVisualCamera.enabled = unitEnabled;
        if (!unitEnabled) {
            _selectionVisual.localScale = Vector3.zero;
        }
    }

    internal void StartSelect() {
        ToggleEnable(true);
    }

    internal void EndSelect() {
        ToggleEnable(false);
    }

    internal bool HasSelection(Vector3 startScreen, Vector3 endScreen) {
        startScreen = Camera.main.WorldToScreenPoint(startScreen);

        if ((Mathf.Abs(startScreen.x - endScreen.x) < StaltDeltaThreshold) || (Mathf.Abs(startScreen.y - endScreen.y) < StaltDeltaThreshold)) {
            return false;
        }

        DrawSelectionRect(startScreen, endScreen);
        return true;
    }

    private void DrawSelectionRect(Vector3 startScreen, Vector3 endScreen) {
        var startWorld = _selectionVisualCamera.ScreenToWorldPoint(startScreen);
        var endWorld = _selectionVisualCamera.ScreenToWorldPoint(endScreen);

        var dx = endWorld.x - startWorld.x;
        var dy = endWorld.y - startWorld.y;

        _selectionVisual.position = new Vector3(startWorld.x + (dx/2.0f), startWorld.y + (dy/2.0f));
        _selectionVisual.localScale = new Vector3(Mathf.Abs(dx), Mathf.Abs(dy), 1.0f);
    }
}