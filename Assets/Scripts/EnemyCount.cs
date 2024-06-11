using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class EnemyCount : MonoBehaviour
{
    public static EnemyCount Instance { get; private set; }

    public List<EnemyController> enemies = new List<EnemyController>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        RegisterEnemy();
    }

    public void RegisterEnemy()
    {
        foreach (Transform enemy in transform)
        {
            EnemyController enemyController = enemy.GetComponent<EnemyController>();
            enemies.Add(enemyController);
        }
    }
}
