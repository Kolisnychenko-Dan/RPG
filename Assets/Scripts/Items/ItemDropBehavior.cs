using System.Collections;
using System.Collections.Generic;
using RPG.Controller;
using RPG.Movement;
using UnityEngine;
using UnityEngine.EventSystems;
using UniversalInventorySystem;

namespace RPG.Item
{    
    [RequireComponent(typeof(PickUpItem))]
    public class ItemDropBehavior : DropBehaviour
    {
        [SerializeField] float timeBeforePickUp = 5f;
        [SerializeField] float moveItemUp = 0.5f;
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
            if(EventSystem.current.IsPointerOverGameObject()) return;

            e.inv.RemoveItem(e.item,1);

            var player = GameObject.Find("Player");
            player.GetComponent<PlayerController>().InteractWithMovement(true);
            player.GetComponent<Mover>().OnDestinationReached += destination => SpawnItem(e, destination);
        }

        private void SpawnItem(InventoryHandler.DropItemEventArgs e, Vector3 destination)
        {
            var droppedItem = Instantiate(((ExpandedItem)e.item).itemPrefab, destination, Quaternion.identity);
            
            droppedItem.transform.position += new Vector3(0,droppedItem.GetComponent<ItemDropBehavior>().moveItemUp,0);

            droppedItem.GetComponent<ItemDropBehavior>().StartPickUpTimer = true;
            droppedItem.GetComponent<SphereCollider>().enabled = false;
        }
    }
}
