using UnityEngine;
using UnityEngine.EventSystems;

// ReSharper disable once CheckNamespace
public class PlaceTower : MonoBehaviour {
    private GridManager _gridManager;
    public GameObject Tower;

    // ReSharper disable once UnusedMember.Local
    private void Start() {
        _gridManager = GameObject.Find("Grid").GetComponent<GridManager>();
    }

    // ReSharper disable once UnusedMember.Local
    private void Update() {
        if (Input.GetMouseButtonDown(1)) {
            if (EventSystem.current.IsPointerOverGameObject()) {
                return;
            }

            var playerPlane = new Plane(Vector3.up, new Vector3());
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float hitdist;

            if (playerPlane.Raycast(ray, out hitdist)) {

                if (_gridManager.FindPathBlocked(ray.GetPoint(hitdist))) {
                    Instantiate(Tower, _gridManager.GridComponent.NodeFromWorldPoint(ray.GetPoint(hitdist)).Position, Quaternion.identity);
                }
            }
        }
    }
}