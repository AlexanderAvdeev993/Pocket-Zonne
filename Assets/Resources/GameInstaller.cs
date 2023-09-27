using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private Player player;
    
    public override void InstallBindings()
    {
        BindPlayer();
    }

    private void BindPlayer()
    {
        Container.Bind<Player>()
            .FromComponentInNewPrefab(player)
            .AsSingle()
            .NonLazy();
    }
}