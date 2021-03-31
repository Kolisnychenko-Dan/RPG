using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniversalInventorySystem;

namespace RPG.Item
{    
    [RequireComponent(typeof(PickUpItem))]
    public class ItemDropBehavior : DropBehaviour
    {
        [SerializeField] float timeBeforePickUp = 5f;
        float pickupTimer = 0;
        bool startTimer = false; 
        
        public bool StartPickUpTimer
        {
            get => startTimer;
            set => startTimer = value;
        }
        
        private void Update()
        {
            if(startTimer)
            {
                pickupTimer += Time.deltaTime;
                if(pickupTimer > timeBeforePickUp)
                {
                    GetComponent<SphereCollider>().enabled = true;
                    startTimer = false;
                }
            }
        }

        public override void OnDropItem(object sender, InventoryHandler.DropItemEventArgs e)
        {
            var player = GameObject.Find("Player");
            var droppedItem = Instantiate(((ExpandedItem)e.item).itemPrefab,player.transform.position,Quaternion.identity);
            droppedItem.GetComponent<ItemDropBehavior>().StartPickUpTimer = true;
            droppedItem.GetComponent<SphereCollider>().enabled = false;
        }
    }
}
