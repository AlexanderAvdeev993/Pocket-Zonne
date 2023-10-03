
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PlayerDieMenu : MonoBehaviour
{
    [SerializeField] private Transform _menu;
    [SerializeField] private Button _restartButton;
    private Player _player;

    public Button RestartButton { get; set; }

    [Inject]
    private void Construct(Player player)
    {
        _player = player;
    }

    private void OnEnable()
    {
        _player.OnPlayerDie += PlayMenuDie;
    }
    private void OnDisable()
    {
        _player.OnPlayerDie -= PlayMenuDie;
    }

    private void PlayMenuDie()
    {
        _menu.gameObject.SetActive(true);
    }
    public void GameObjectFalse()
    {
        _menu.gameObject.SetActive(false);    
    }

}
