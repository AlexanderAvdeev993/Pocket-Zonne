using UnityEngine;
using Zenject;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform _playerSpawnPoint;
    [SerializeField] private SpawnPoint[] spawnEnemyPoints;

    private Player _player;
    private EnemyFactory _enemyFactory;
    private Inventory _inventory;

    [Inject]
    private void Construct(Player player, EnemyFactory enemyFactory, Inventory inventory)
    {
        _player = player;
        _enemyFactory = enemyFactory;     
        _inventory = inventory;
    }

    private void Start()
    {
        StartGame();
    }
    private void StartGame()
    {
        ReturnPlayerToStartPos();
        _enemyFactory.CreateEnemy(spawnEnemyPoints);
        _player.LoadPlayerData();
        _inventory.LoadInventory();
    }
    private void EndGame()
    {

    }

    private void ReturnPlayerToStartPos()
    {
        _player.transform.position = _playerSpawnPoint != null ? _playerSpawnPoint.position : Vector2.zero;
        Physics.SyncTransforms();
    }

}
