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
        var temp = ObjectPool.Instance.GetObjectForType("Enemie", false);
        temp.transform.position = Start.position;
        temp.GetComponent<Movement>().Target = End.position;
        temp.GetComponent<Movement>().Cell = _gridManager.GridComponent.NodeFromWorldPoint(temp.transform.position);
        _enemies.Add(temp);
    }
}