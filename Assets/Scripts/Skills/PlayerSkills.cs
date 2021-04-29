using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RPG.Skills
{
    public class PlayerSkills : MonoBehaviour
    {
        [SerializeField] UniversalInventorySystem.Inventory skills;

        [SerializeField] Transform rightHandTransform;
        [SerializeField] Transform leftHandTransform;
        coolDown[] coolDowns;

        public Transform RightHandTransform { get => rightHandTransform; }
        public Transform LeftHandTransform { get => leftHandTransform; }

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
                        MakeSkillActive(i);
                    }
                }
            }
        }

        public float GetCooldownProgress(int slot)
        {
            return coolDowns[slot].isTimerRunning ? coolDowns[slot].currentTimer/coolDowns[slot].itemCooldownTime : -1;
        }

        public void SkillCasted(int slot)
        {
            coolDowns[slot].StartTimer();
            MakeSkillUnActive(slot);  
        }

        private void MakeSkillActive(int slot)
        {
            UniversalInventorySystem.Slot newSlot = skills.slots[slot];
            newSlot.interative = UniversalInventorySystem.SlotProtection.Use;
            skills.slots[slot] = newSlot;
        }

        private void MakeSkillUnActive(int slot)
        {
            UniversalInventorySystem.Slot newSlot = skills.slots[slot];
            newSlot.interative = UniversalInventorySystem.SlotProtection.Locked;
            skills.slots[slot] = newSlot;
        }

        private void InitializeCoolDowns()
        {
            coolDowns = new coolDown[skills.SlotAmount];
            for(int i = 0; i < skills.SlotAmount; ++i)
            {
                coolDowns[i].isTimerRunning = false;
                coolDowns[i].currentTimer = Mathf.Infinity;
                if(skills.slots[i].HasItem) coolDowns[i].itemCooldownTime = ((Skill)skills.slots[i].item).CoolDown;
            }
        }

        struct coolDown
        {
            public bool isTimerRunning;
            public float currentTimer;
            public float itemCooldownTime;

            //public event Action OnCoolDownPassed;

            public void StartTimer()
            {
                isTimerRunning = true;
                currentTimer = 0;
            }
        }
    }
}

