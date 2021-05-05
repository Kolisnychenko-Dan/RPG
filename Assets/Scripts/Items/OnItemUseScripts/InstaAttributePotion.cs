using UnityEngine;
using UniversalInventorySystem;
using RPG.Stats;
using RPG.Attributes;

namespace RPG.Inventory
{
    public class InstaAttributePotion : IUsable
    {
        public void OnUse(object sender, InventoryHandler.UseItemEventArgs e)
        {
            var unit = GameObject.Find("Player");
            string itemName = e.item.name;
            float heal = unit.GetComponent<ItemStats>().GetStat(Stat.HealthInstaHeal,itemName);
            float mana = unit.GetComponent<ItemStats>().GetStat(Stat.ManaInsta,itemName);

            unit.GetComponent<Combat.CombatTarget>().ChangeHealth(heal,Combat.CombatTarget.HealthChangeType.Heal);
            unit.GetComponent<Mana>().CurrentMana += mana;
        }
    }
}
