using System;
using System.Collections.Generic;
using UnityEngine;
using UniversalInventorySystem;

namespace RPG.Stats
{
    public class ItemStats : MonoBehaviour, IAdditiveModifier
    {
        [SerializeField] Progression progression = null;
        [SerializeField] UniversalInventorySystem.Inventory inv = null;

        public IEnumerable<float> GetAdditiveModifier(Stat stat)
        {
            float statValue = 0;
            foreach (var slot in inv.slots)
            {
                if(slot.HasItem) statValue += GetStat(stat,slot.item.itemName);
            }
            yield return statValue;
        }

        public float GetStat(Stat stat, string item)
        {
            return progression.GetStat(item, stat, 1, false);
        }
    }
}