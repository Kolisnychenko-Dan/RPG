using System;
using UnityEngine;

namespace RPG.Stats
{
    public class ItemStats : MonoBehaviour
    {
        [SerializeField] Progression progression = null;

        public float GetStat(Stat stat, string item)
        {
            return progression.GetStat(item, stat, 1);
        }
    }
}