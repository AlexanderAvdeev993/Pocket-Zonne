using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private Player player;
    [SerializeField] private Canvas canvas;
    
    public override void InstallBindings()
    {
        BindPlayer();
        BindEnemyFactory();
        BindCanvas();
        BindItemFactory();
    }
    private void BindCanvas()
    {
        Container.Bind<Canvas>()
            .FromInstance(canvas)
            .AsSingle()
            .NonLazy();
    }
    private void BindEnemyFactory()
    {
        Container.Bind<EnemyFactory>()
            .FromNew()
            .AsSingle()
            .NonLazy();
    }
    private void BindItemFactory()
    {
        Container.Bind<ItemFactory>()
            .FromNew()
            .AsSingle()
            .NonLazy();
    }

    private void BindPlayer()
    {
        Container.Bind<Player>()
            .FromComponentInNewPrefab(player)
            .AsSingle()
            .NonLazy();
    }
}