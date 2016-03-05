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
                var hitNode = _gridManager.GridComponent.NodeFromWorldPoint(ray.GetPoint(hitdist));

                if (hitNode == null || hitNode.Blocked) {
                    return;
                }

                // Check is enemy is inside the node
                for (var i = 0; i < EnemyManager.Instance.GetEnemies().Count; i++) {
                    var enemyNode = _gridManager.GridComponent.NodeFromWorldPoint(EnemyManager.Instance.GetEnemies()[i].transform.position);

                    if (enemyNode == hitNode || enemyNode.Parent == hitNode) {
                        return;
                    }
                }

                hitNode.Blocked = true;

                // Check if all enemies can get to the end if node is blocked
                if (_gridManager.FindPath()) {
                    for (var i = 0; i < EnemyManager.Instance.GetEnemies().Count; i++) {
                        var enemyNode = _gridManager.GridComponent.NodeFromWorldPoint(EnemyManager.Instance.GetEnemies()[i].transform.position);

                        var temp = enemyNode.Parent;

                        while (temp != null) {
                            temp = temp.Parent;

                            if (temp == hitNode) {
                                hitNode.Blocked = false;
                                return;
                            }
                        }
                    }

                    
                    hitNode.Parent = null;
                    hitNode.GCost = 0;
                    hitNode.HCost = 0;
                }
                else {
                    hitNode.Blocked = false;
                    return;
                }

                
                Instantiate(Tower, hitNode.Position, Quaternion.identity);
             
            }
        }
    }
}