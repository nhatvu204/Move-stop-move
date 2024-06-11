using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] Collider floor;
    [SerializeField] EnemyController enemy;
    public bool alreadySpawned;

    // Start is called before the first frame update
    void Start()
    {
        alreadySpawned = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (EnemyCount.Instance.enemies.Count < 11 && PlayerController.Instance.alive && !alreadySpawned && CanvasManager.Instance.aliveCounter > 10)
        {
            SpawnEnemy();
        }
    }

    public void SpawnEnemy()
    {
        Bounds bounds = floor.bounds;
        float offsetX = Random.Range(-bounds.extents.x, bounds.extents.x);
        float offsetZ = Random.Range(-bounds.extents.z, bounds.extents.z);

        Vector3 spawnPt = new Vector3(offsetX, floor.transform.position.y, offsetZ);

        EnemyController e = Instantiate(enemy, spawnPt, Quaternion.identity);

        e.level = Random.Range(1, PlayerController.Instance.level);

        EnemyCount.Instance.enemies.Add(e);

        Invoke(nameof(ResetSpawn), 3f);
    }

    private void ResetSpawn()
    {
        alreadySpawned = false;
    }
}
