using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class EnemyFactory : MonoBehaviour
{
    [SerializeField] private SpawnPoint[] spawnPoints;
    private DiContainer _diContainer;

    [Inject]
    private void Construct(DiContainer diContainer)
    {
        _diContainer = diContainer;
    }

    public void CreateEnemy()
    {
        foreach(SpawnPoint spawnPoint in spawnPoints)
        {
            Vector2 spawnPosition = spawnPoint.GetSpawnPosition();
            Enemy enemyPrefab = spawnPoint.GetEnemyPrefab();    

            Enemy enemy =
               _diContainer.InstantiatePrefabForComponent<Enemy>(enemyPrefab, spawnPosition, Quaternion.identity, null);
        }
    }
}
