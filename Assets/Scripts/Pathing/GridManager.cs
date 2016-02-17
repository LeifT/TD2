using System.Collections.Generic;
using UnityEngine;

// ReSharper disable once CheckNamespace
public class GridManager : MonoBehaviour {
    [HideInInspector]
    public GridComponent GridComponent;
    private BreadthFirstSearch _bfs;

    public Transform StartPosition;
    public Transform TargetPosition;

    public void Start() {
        _bfs = new BreadthFirstSearch();
        GridComponent = GetComponent<GridComponent>();
        _bfs.FindPath(GridComponent, GridComponent.NodeFromWorldPoint(StartPosition.position), GridComponent.NodeFromWorldPoint(TargetPosition.position));
    }

    public bool FindPath() {
        return _bfs.FindPath(GridComponent, GridComponent.NodeFromWorldPoint(StartPosition.position), GridComponent.NodeFromWorldPoint(TargetPosition.position));
    }
}