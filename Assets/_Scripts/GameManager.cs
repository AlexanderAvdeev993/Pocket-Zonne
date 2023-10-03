using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform _playerSpawnPoint;
    [SerializeField] private SpawnPoint[] spawnEnemyPoints;
    [SerializeField] private Vector2 minSpawnPosition;
    [SerializeField] private Vector2 maxSpawnPosition;

    private Player _player;
    private EnemyFactory _enemyFactory;
    private Inventory _inventory;
    private Canvas _canvas;
    private PlayerDieMenu _playerDieMenu;
    public Button _button;

    [Inject]
    private void Construct(Player player, EnemyFactory enemyFactory, Inventory inventory, Canvas canvas )
    {
        _player = player;
        _enemyFactory = enemyFactory;     
        _inventory = inventory;
        _canvas = canvas;
    }

    private void Start()
    {
        _playerDieMenu = _canvas.GetComponent<PlayerDieMenu>();
        //_playerDieMenu.RestartButton.onClick.AddListener(StartNewGame);
        _button.onClick.AddListener(StartNewGame);

        StartGame(); 
    }
    private void StartGame()
    {
        ReturnPlayerToStartPos();
        _enemyFactory.CreateEnemy(spawnEnemyPoints, minSpawnPosition, maxSpawnPosition);
        _player.LoadPlayerData();
        _inventory.LoadInventory();
    }
    private void StartNewGame()
    {
        ReturnPlayerToStartPos();
        _player.RestartPlayer();
        _playerDieMenu.GameObjectFalse();

        _enemyFactory.RespawnEnemy();
    }   

    private void ReturnPlayerToStartPos()
    {
        _player.transform.position = _playerSpawnPoint != null ? _playerSpawnPoint.position : Vector2.zero;
        Physics.SyncTransforms();
    }

}
