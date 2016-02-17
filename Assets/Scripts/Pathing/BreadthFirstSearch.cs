using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ReSharper disable once CheckNamespace
public class BreadthFirstSearch {
    public bool FindPath(GridComponent grid, Cell startPos, Cell targetPos) {
        var startNode = startPos;
        bool pathFound = false;

        if (startNode.Blocked) {
            return false;
        }

        var openSet = new Heap<Cell>(grid.MaxSize);
        var closedSet = new HashSet<Cell>();

        openSet.Add(startNode);

        while (openSet.Count > 0) {
            var currentNode = openSet.RemoveFirst();

            if (currentNode == targetPos) {
                pathFound = true;
            }

            closedSet.Add(currentNode);

            foreach (var neighbour in grid.GetNeighbours(currentNode)) {
                if (neighbour.Blocked || closedSet.Contains(neighbour)) {
                    continue;
                }

                var moveCost = currentNode.GCost + GetDistance(currentNode, neighbour);

                if (moveCost < neighbour.GCost || !openSet.Contains(neighbour)) {
                    //closedSet.Add(neighbour);
                    neighbour.GCost = moveCost;
                    neighbour.Parent = currentNode;

                    if (!openSet.Contains(neighbour)) {
                        openSet.Add(neighbour);
                    }
                }
            }
        }

        return pathFound;
    }

    private int GetDistance(Cell lhs, Cell rhs) {
        var distanceX = Mathf.Abs(lhs.PosX - rhs.PosX);
        var distanceY = Mathf.Abs(lhs.PosZ - rhs.PosZ);

        if (distanceX > distanceY) {
            return (14*distanceY + 10*(distanceX - distanceY));
        }

        return (14*distanceX + 10*(distanceY - distanceX));
    }
}