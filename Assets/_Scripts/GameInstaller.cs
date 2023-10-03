using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private Player player;
    [SerializeField] private Canvas canvas;
    [SerializeField] private Inventory inventory;
    [SerializeField] private AmmoCounter ammoCounter;
    
    public override void InstallBindings()
    {
        BindPlayer();
        BindEnemyFactory();
        BindCanvas();
        BindItemFactory();
        BindSaveSystem();
        BindInventory();
        BindEnemyPool();
        BindAmmoCounter();
    }

    private void BindAmmoCounter()
    {
        Container.Bind<AmmoCounter>()
            .FromInstance(ammoCounter)
            .AsSingle()
            .NonLazy();
    }
    private void BindEnemyPool()
    {
        Container.Bind<EnemyPool>()
            .FromNew()
            .AsSingle()
            .NonLazy();
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