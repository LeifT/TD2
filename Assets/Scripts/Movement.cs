using UnityEngine;

// ReSharper disable once CheckNamespace
public class Movement : MonoBehaviour {
    [HideInInspector]
    public Cell Cell;

    [HideInInspector]
    public Vector3 Target;

	
    // ReSharper disable once UnusedMember.Local
	void Update () {
	    if (Vector3.Distance(transform.position, Target) < 0.2f) {
            EnemyManager.Instance.Remove(gameObject);
	        return;
	    }

	    if (Cell.Parent == null) {
            EnemyManager.Instance.Remove(gameObject);
	        return;
	    }

	    transform.position = Vector3.MoveTowards(transform.position, Cell.Position, Time.deltaTime);

	    if (Vector3.Distance(transform.position, Cell.Position) < 0.2) {
	        Cell = Cell.Parent;
	    }
	}
}
