using UnityEngine;
using UniversalInventorySystem;

namespace RPG.Inventory
{
    public class PlayerInventory : MonoBehaviour
    {   
        public UniversalInventorySystem.Inventory inventory;
         
        private void Start()
        {
            inventory.Initialize();
        }
    }
}
