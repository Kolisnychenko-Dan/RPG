using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniversalInventorySystem;
using RPG.Inventory;

namespace RPG.Item
{
    public class PickUpItem : MonoBehaviour
    {
        [SerializeField] ExpandedItem item;
        [SerializeField] int amount;
        private void OnTriggerEnter(Collider other) 
        {
            if (other.gameObject.name == "Player")
            {
                PlayerInventory pi = GameObject.FindObjectOfType<PlayerInventory>();
                pi.inventory.AddItem( (UniversalInventorySystem.Item)item, amount);
                Destroy(gameObject);
            }
        }
    }
}

