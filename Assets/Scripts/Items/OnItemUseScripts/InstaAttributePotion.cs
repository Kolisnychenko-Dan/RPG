using UnityEngine;
using UniversalInventorySystem;
using RPG.Stats;

namespace RPG.Inventory
{
    public class InstaAttributePotion : IUsable
    {
        public void OnUse(object sender, InventoryHandler.UseItemEventArgs e)
        {
            var unit = GameObject.Find("Player");
            string itemName = e.item.name;
            float heal = unit.GetComponent<ItemStats>().GetStat(Stat.HealthInstaHeal,itemName);

            unit.GetComponent<Combat.CombatTarget>().Health += heal;
        }
    }
}
