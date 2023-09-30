using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform _playerSpawnPoint;
    [SerializeField] private SpawnPoint[] spawnEnemyPoints;

    private Player _player;
     private EnemyFactory _enemyFactory;

    [Inject]
    private void Construct(Player player, EnemyFactory enemyFactory)
    {
        _player = player;
        _enemyFactory = enemyFactory;
    }

    private void Start()
    {
        ReturnPlayerToStartPos();
        _enemyFactory.CreateEnemy(spawnEnemyPoints);
    }

    private void ReturnPlayerToStartPos()
    {
        _player.transform.position = _playerSpawnPoint != null ? _playerSpawnPoint.position : Vector2.zero;
        Physics.SyncTransforms();
    }

}
