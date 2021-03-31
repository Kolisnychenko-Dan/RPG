using UnityEngine;
using UniversalInventorySystem;

namespace RPG.Item
{
    [ 
        CreateAssetMenu(fileName = "Item", menuName = "UniversalInventorySystem/ExpandedItem"), 
        System.Serializable
    ]
    public class ExpandedItem : UniversalInventorySystem.Item
    {
        [SerializeField] public GameObject itemPrefab = null;
    }
}
