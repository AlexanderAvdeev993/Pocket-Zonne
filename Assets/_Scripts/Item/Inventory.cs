using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

public class Inventory : MonoBehaviour 
{  
    [SerializeField] private Transform _inventoryPanels;
    [SerializeField] private float _searchRadius;
    [SerializeField] private LayerMask _itemLayer;

    [SerializeField] private List<Slot> slots;

    private void Start()
    {
        for (int i = 0; i < _inventoryPanels.childCount; i++)
        {
            slots.Add(_inventoryPanels.GetChild(i).GetComponent<Slot>());
        }
    }
    
    public void AddItem(Item item)
    {
        foreach (Slot slot in slots)
        {              
            if(slot.item != null && slot.item.itemID == item.itemID)
            {
                slot.amount += item.amount;
                item.gameObject.SetActive(false);
                return;
            }
        }
        foreach (Slot slot in slots)
        {
            if(slot.isEmpty == true)
            {
                slot.item = item;
                slot.amount = item.amount;

                slot.ChangeSprite(item.sprite);
                
                slot.isEmpty = false;
                item.gameObject.SetActive(false);
                return;
                
            }
        }
    }
}
