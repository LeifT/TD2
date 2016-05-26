using System.Collections.Generic;
using UnityEngine;

// ReSharper disable once CheckNamespace
public class BreadthFirstSearch {
    public bool FindPath(GridComponent grid, Cell startPos, Cell targetPos) {
        var startNode = startPos;
        var pathFound = false;

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

    public bool PathExistOnBlock(GridComponent grid, Cell startPos, Cell targetPos, Vector3 blockPos) {
        var hitNode = grid.NodeFromWorldPoint(blockPos);

        if (hitNode == null || hitNode.Blocked) {
            return false;
        }

        // Check is enemy is inside the node
        for (var i = 0; i < EnemyManager.Instance.GetEnemies().Count; i++) {
            var enemyNode = grid.NodeFromWorldPoint(EnemyManager.Instance.GetEnemies()[i].transform.position);

            if (enemyNode == hitNode || enemyNode.Parent == hitNode) {
                return false;
            }
        }

        hitNode.Blocked = true;

        // Check if all enemies can get to the end if node is blocked
        if (FindPath(grid, startPos, targetPos)) {
            for (var i = 0; i < EnemyManager.Instance.GetEnemies().Count; i++) {
                var enemyNode = grid.NodeFromWorldPoint(EnemyManager.Instance.GetEnemies()[i].transform.position);

                var temp = enemyNode.Parent;

                while (temp != null) {
                    temp = temp.Parent;

                    if (temp == hitNode) {
                        hitNode.Blocked = false;
                        return false;
                    }
                }
            }


            hitNode.Parent = null;
            hitNode.GCost = 0;
            hitNode.HCost = 0;
        }
        else {
            hitNode.Blocked = false;
            return false;
        }

        return true;
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