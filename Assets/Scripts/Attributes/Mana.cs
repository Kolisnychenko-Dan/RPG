using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Stats;
using UnityEngine;

namespace RPG.Attributes
{
    public class Mana : MonoBehaviour
    {
        float mana = -1f;
        float maxMana;
        BaseStats baseStats;

        public float CurrentMana 
        { 
            get => mana; 
            set => Mathf.Min( maxMana, Mathf.Max( 0, value ));
        }

        private void Awake()
        {
            baseStats = GetComponent<BaseStats>();
        }

        private void Update()
        {
            RegenerateMana();
        }

        private void RegenerateMana()
        {
            CurrentMana += Time.deltaTime * baseStats.GetCalculatedStat(Stat.ManaRegen);
        }
    }
}