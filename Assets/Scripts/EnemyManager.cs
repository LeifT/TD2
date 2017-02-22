using System.Collections.Generic;
using UnityEngine;

// ReSharper disable once CheckNamespace
public class EnemyManager : MonoBehaviour {
    public static EnemyManager Instance;
    private List<GameObject> _enemies;
    private GridManager _gridManager;
    public GameObject Enemie;
    public Transform Start;
    public Transform End;

    public List<GameObject> GetEnemies() {
        return _enemies;
    }

    public void Remove(GameObject enemie) {
        ObjectPool.Instance.PoolObject(enemie);
        _enemies.Remove(enemie);
    }

    // ReSharper disable once UnusedMember.Local
    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        else if (Instance != this) {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
        InitGame();
    }

    private void InitGame() {
        _gridManager = GameObject.Find("Grid").GetComponent<GridManager>();
        _enemies = new List<GameObject>();
    }

    public void SpawnEnemie() {
        var enemy = ObjectPool.Instance.GetObjectForType("Enemie", false);
        enemy.transform.position = Start.position;
        enemy.GetComponent<Movement>().Target = End.position;
        enemy.GetComponent<Movement>().Cell = _gridManager.GridComponent.NodeFromWorldPoint(enemy.transform.position);
        _enemies.Add(enemy);
    }
}