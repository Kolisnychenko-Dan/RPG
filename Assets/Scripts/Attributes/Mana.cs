using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Skills;
using RPG.Stats;
using UnityEngine;

namespace RPG.Attributes
{
    [RequireComponent(typeof(PlayerSkills))]
    public class Mana : MonoBehaviour
    {
        [SerializeField]float mana = -1f;
        float maxMana;
        float manaRegen;
        BaseStats baseStats;

        public float CurrentMana 
        { 
            get => mana; 
            set {
                mana = Mathf.Min( maxMana, Mathf.Max( 0, value));
            }
        }

        private void Awake()
        {
            baseStats = GetComponent<BaseStats>();
            baseStats.OnAttributesChanged += OnMaxManaUpdated;
            baseStats.OnAttributesChanged += () => manaRegen = baseStats.GetCalculatedStat(Stat.ManaRegen);
        }

        private void Start()
        {
            maxMana = baseStats.GetCalculatedStat(Stat.Mana);
            manaRegen = baseStats.GetCalculatedStat(Stat.ManaRegen);
            
            if(mana == -1f)
            {
                mana = maxMana;
            }
        }

        private void Update()
        {
            RegenerateMana();
        }

        public bool TryConsuming(float requiredMana)
        {
            if(requiredMana > mana) return false;
            return true;
        }

        public void Consume(float mana)
        {
            this.mana -= mana;
        }

        private void RegenerateMana()
        {
            CurrentMana += Time.deltaTime * manaRegen;
        }

        private void OnMaxManaUpdated()
        {
            float currentMaxMana = baseStats.GetCalculatedStat(Stat.Mana);

            mana = mana * (currentMaxMana / maxMana);
            maxMana = currentMaxMana;
        }

    }
}