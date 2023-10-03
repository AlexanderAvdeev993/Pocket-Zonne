using UnityEngine;

[CreateAssetMenu(fileName = "ItemRegistr", menuName = "ScriptableObject")]
public class ItemRegistr : ScriptableObject
{
    [System.Serializable]
    public struct ItemPrefabPair
    {
        public int itemID;
        public Item itemPrefab;
    }

    public ItemPrefabPair[] itemPrefabs;

    public Item GetItemPrefab(int itemID)
    {
        foreach (var pair in itemPrefabs)
        {
            if (pair.itemID == itemID)
            {
                return pair.itemPrefab;
            }
        }
        return null; 
    }
}