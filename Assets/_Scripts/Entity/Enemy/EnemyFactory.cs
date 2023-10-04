using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class EnemyFactory 
{
    private DiContainer _diContainer;
    private EnemyPool _enemyPool;

    [Inject]
    private void Construct(DiContainer diContainer, EnemyPool enemyPool)
    {
        _diContainer = diContainer;
        _enemyPool = enemyPool;  
    }

    public void CreateEnemy(SpawnPoint[] spawnPoints)
    {
        _enemyPool.ClearEnemyList();
        foreach (SpawnPoint spawnPoint in spawnPoints)
        {
            Vector2 spawnPosition = spawnPoint.GetSpawnPosition();
            Enemy enemyPrefab = spawnPoint.GetEnemyPrefab();    

            Enemy enemy =
               _diContainer.InstantiatePrefabForComponent<Enemy>(enemyPrefab, spawnPosition, Quaternion.identity, null);
            _enemyPool.AddEnemy(enemy);
        }
    }

    public void CreateEnemy(SpawnPoint[] spawnPoints, Vector2 minPosition, Vector2 maxPosition)
    {
        _enemyPool.ClearEnemyList();
        foreach (SpawnPoint spawnPoint in spawnPoints)
        {
            Vector2 randomSpawnPosition = GetRandomPosition(minPosition, maxPosition);
            Enemy enemyPrefab = spawnPoint.GetEnemyPrefab();

            Enemy enemy =
                _diContainer.InstantiatePrefabForComponent<Enemy>(enemyPrefab, randomSpawnPosition, Quaternion.identity, null);
            _enemyPool.AddEnemy(enemy);
        }
    }

    public void RespawnEnemy()
    {
        List<Enemy> enemyList = _enemyPool.EnemyList();
        foreach(Enemy enemy in enemyList)
        {
            enemy.gameObject.SetActive(true);
            enemy.RespawnEnemy();

        }
    }


    private Vector2 GetRandomPosition(Vector2 minPosition, Vector2 maxPosition)
    {
        float randomX = Random.Range(minPosition.x, maxPosition.x);
        float randomY = Random.Range(minPosition.y, maxPosition.y);
        return new Vector2(randomX, randomY);
    } 
}
