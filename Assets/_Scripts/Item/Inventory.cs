
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

public class Inventory : MonoBehaviour
{
    [SerializeField] private Transform _inventoryPanels;
    [SerializeField] private float _searchRadius;
    [SerializeField] private LayerMask _itemLayer;

    [SerializeField] private List<Slot> slots;
    [SerializeField] private ItemRegistr ItemRegistr;
    private List<SlotData> inventoryDataList = new List<SlotData>();
    private SaveSystem _saveSystem;


    [Inject]
    private void Construct(SaveSystem saveSystem)
    {
        _saveSystem = saveSystem;
    }

    private void Start()
    {
        slots = _inventoryPanels.GetComponentsInChildren<Slot>().ToList();     
    }

    private void SaveInventory()
    {
        inventoryDataList.Clear();
        foreach (Slot slot in slots)
        {          
            if(!slot.IsEmpty)
            {
                SlotData slotData = new SlotData
                {
                    itemID = slot.Item.itemID,
                    amount = slot.Amount,
                    isEmpty = false,
                };
                inventoryDataList.Add(slotData);
            }
            else
            {
                SlotData slotData = new SlotData
                {
                    isEmpty = true,
                };
                inventoryDataList.Add(slotData);
            }            
        }

        _saveSystem.Save("inventory_data", inventoryDataList, (success) =>
        {
            if (success)
            {
                Debug.Log("inventory_data saved successfully.");
            }
            else
            {
                Debug.LogError("Failed to save inventory_data.");
            }
        });


    }
    

    public void LoadInventory()
    {
        _saveSystem.Load<List<SlotData>>("inventory_data", (data) =>
        {
            
            if (data != null)
            {
                for (int i = 0; i < Mathf.Min(data.Count, slots.Count); i++)
                {
                    SlotData slotData = data[i];

                    if (slotData.isEmpty == false)
                    {                                              
                        Item item = ItemRegistr.GetItemPrefab(slotData.itemID);
                      
                        if(slotData.amount > 1)
                            slots[i].AmountText.text = slotData.amount.ToString();

                        slots[i].Amount = slotData.amount;
                        slots[i].ChangeSprite(item.sprite);
                        slots[i].Item = item;
                        slots[i].IsEmpty = false;
                        slots[i].Item.isStacked = item.isStacked;
                    }
                    else
                    {
                        slots[i].IsEmpty = true;
                    }                    
                }
            }
            else
                Debug.Log("inventory_data null");
        });
    }

    private void OnDestroy()
    {
        SaveInventory();
    }

    public void AddItem(Item item)
    {
        foreach (Slot slot in slots)
        {              
            if(slot.Item != null && slot.Item.itemID == item.itemID && item.isStacked)
            {                 
                slot.Amount += item.amount;
                slot.AmountText.text = slot.Amount.ToString();
                item.gameObject.SetActive(false);
                return;
            }
        }
        foreach (Slot slot in slots)
        {
            if(slot.IsEmpty == true)
            {
                slot.Item = item;
                slot.Amount = item.amount;

                if(slot.Amount > 1)
                    slot.AmountText.text = item.amount.ToString(); 

                slot.ChangeSprite(item.sprite);
                
                slot.IsEmpty = false;
                item.gameObject.SetActive(false);
                return;          
            }
        }
    }
}
