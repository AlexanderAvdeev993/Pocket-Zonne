using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform _playerSpawnPoint;



     private Player _player;


    [Inject]
    private void Construct(Player player)
    {
        _player = player;
    }

    private void Start()
    {
        ReturnPlayerToStartPos();   
    }

    private void ReturnPlayerToStartPos()
    {
        _player.transform.position = _playerSpawnPoint != null ? _playerSpawnPoint.position : Vector2.zero;
        Physics.SyncTransforms();
    }

}
