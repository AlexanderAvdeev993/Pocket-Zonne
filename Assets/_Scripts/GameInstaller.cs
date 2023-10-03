using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private Player player;
    [SerializeField] private Canvas canvas;
    [SerializeField] private Inventory inventory;
    
    public override void InstallBindings()
    {
        BindPlayer();
        BindEnemyFactory();
        BindCanvas();
        BindItemFactory();
        BindSaveSystem();
        BindInventory();
    }
    private void BindInventory()
    {
        Container.Bind<Inventory>()
            .FromInstance(inventory)
            .AsSingle()
            .NonLazy();
    }
    private void BindSaveSystem()
    {
        Container.Bind<SaveSystem>()
            .FromNew()
            .AsSingle()
            .NonLazy();
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