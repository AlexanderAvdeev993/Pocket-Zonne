using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ItemFactory 
{
    private DiContainer _diContainer;

    [Inject]
    private void Construct(DiContainer diContainer)
    {
        _diContainer = diContainer;
    }

    /// <summary>
    ///  Дропает предмет из списка с равной вероятностью.
    /// </summary>
    public void DropItem(Item[] itemsForDrops, Vector2 spawnPosition)  
    {
        int randomItem = Random.Range(0, itemsForDrops.Length);

        //if(randomItem > 0 && itemsForDrops[randomItem] != null && spawnPosition != null)
        //{
            
        //}
        Item item = _diContainer.InstantiatePrefabForComponent<Item>
                (itemsForDrops[randomItem], spawnPosition, Quaternion.identity, null);

    }
}
