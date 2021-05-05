using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RPG.Attributes;
using UnityEngine;

namespace RPG.Skills
{
    public class PlayerSkills : MonoBehaviour
    {
        [SerializeField] UniversalInventorySystem.Inventory skills;

        [SerializeField] Transform rightHandTransform;
        [SerializeField] Transform leftHandTransform;
        CoolDown[] coolDowns;
        Mana mana;
        

        public Transform RightHandTransform { get => rightHandTransform; }
        public Transform LeftHandTransform { get => leftHandTransform; }

        private void Awake()
        {
            mana = GetComponent<Mana>();
        }

        private void Start()
        {
            skills.Initialize();
            InitializeCoolDowns();
        }

        private void Update()
        {
            for(int i = 0; i < skills.SlotAmount; ++i)
            {
                if(coolDowns[i].isTimerRunning)
                {
                    coolDowns[i].currentTimer += Time.deltaTime;
                    if(coolDowns[i].currentTimer > coolDowns[i].itemCooldownTime)
                    {
                        coolDowns[i].isTimerRunning = false;
                    }
                }
            }
        }

        public float GetCooldownProgress(int slot)
        {
            return coolDowns[slot].isTimerRunning ? coolDowns[slot].currentTimer/coolDowns[slot].itemCooldownTime : -1;
        }

        public bool IsEnaughManaForCast(int slot)
        {
            return skills.slots[slot].HasItem && mana.TryConsuming(((Skill)skills[slot].item).ManaRequired);
        }

        public bool CanCastSkill(int slot)
        {
            return !coolDowns[slot].isTimerRunning && IsEnaughManaForCast(slot); 
        }

        public void SkillCasted(int slot)
        {
            coolDowns[slot].StartTimer();
            mana.Consume(((Skill)skills[slot].item).ManaRequired);
        }

        private void InitializeCoolDowns()
        {
            coolDowns = new CoolDown[skills.SlotAmount];
            for(int i = 0; i < skills.SlotAmount; ++i)
            {
                coolDowns[i].isTimerRunning = false;
                coolDowns[i].currentTimer = Mathf.Infinity;
                if(skills.slots[i].HasItem) coolDowns[i].itemCooldownTime = ((Skill)skills.slots[i].item).CoolDown;
            }
        }

        public struct CoolDown
        {
            public bool isTimerRunning;
            public float currentTimer;
            public float itemCooldownTime;

            public void StartTimer()
            {
                isTimerRunning = true;
                currentTimer = 0;
            }
        }
    }
}

