using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniversalInventorySystem;

namespace RPG.Inventory
{
    public class PickUpItem : MonoBehaviour
    {
        [SerializeField] Item item;
        private void OnTriggerEnter(Collider other) 
        {
            if (other.gameObject.name == "Player")
            {
                PlayerInventory pi = GameObject.FindObjectOfType<PlayerInventory>();
                pi.inventory.AddItem(item,1);
                Destroy(gameObject);
            }
        }
    }
}

