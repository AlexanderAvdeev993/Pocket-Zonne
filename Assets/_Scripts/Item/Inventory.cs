
using System.Collections.Generic;

using UnityEngine;


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
            if(slot.item != null && slot.item.itemID == item.itemID && item.isStacked)
            {                 
                slot.amount += item.amount;
                slot._amountText.text = slot.amount.ToString();
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

                if(slot.amount > 1)
                    slot._amountText.text = item.amount.ToString(); 

                slot.ChangeSprite(item.sprite);
                
                slot.isEmpty = false;
                item.gameObject.SetActive(false);
                return;          
            }
        }
    }
}
