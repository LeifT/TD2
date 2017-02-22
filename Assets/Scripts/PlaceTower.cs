using UnityEngine;
using UnityEngine.EventSystems;

// ReSharper disable once CheckNamespace
public class PlaceTower : MonoBehaviour {
    private GridManager _gridManager;
    public GameObject Tower1;
    public GameObject Tower2;
    public GameObject Tower3;

    private GameObject SelectedTower;

    // ReSharper disable once UnusedMember.Local
    private void Start() {
        _gridManager = GameObject.Find("Grid").GetComponent<GridManager>();
        SelectedTower = Tower1;
    }

    // ReSharper disable once UnusedMember.Local
    private void Update() {
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            SelectedTower = Tower1;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            SelectedTower = Tower2;
        }

        if (Input.GetKeyDown(KeyCode.Alpha3)) {
            SelectedTower = Tower3;
        }

        if (Input.GetMouseButtonDown(1)) {
            if (SelectedTower != null) {
                Place(SelectedTower);
            }
        }
    }

    private void Place(GameObject tower) {
        if (EventSystem.current.IsPointerOverGameObject()) {
            return;
        }

        var playerPlane = new Plane(Vector3.up, new Vector3());
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float hitdist;

        if (playerPlane.Raycast(ray, out hitdist)) {
            if (_gridManager.FindPathBlocked(ray.GetPoint(hitdist))) {
                Instantiate(tower, _gridManager.GridComponent.NodeFromWorldPoint(ray.GetPoint(hitdist)).Position, Quaternion.identity);
            }
        }
    }
}