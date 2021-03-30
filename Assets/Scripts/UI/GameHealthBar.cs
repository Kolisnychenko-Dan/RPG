﻿using System;
using RPG.Combat;
using UnityEngine;

namespace RPG.UI
{
    public class GameHealthBar : MonoBehaviour
    {
        CombatTarget health;
        bool isEnabled = true;

        private void Awake()
        {
            health = GetComponentInParent<CombatTarget>();

            health.OnHealthChanged += UpdateHealthBar;
            health.OnDeath += () => {
                isEnabled = false;
                DisableBar();
            };
            health.OnRiseFromTheDead += () => isEnabled = true;
        }

        private void Update()
        {
            if(isEnabled)
            {
                if(Input.GetKey(KeyCode.LeftAlt))
                {
                    EnableBar();
                }
                else DisableBar();
            }
            
        }

        private void UpdateHealthBar(float healtChange, CombatTarget.HealthChangeType type)
        {
            transform.localScale = new Vector3(health.GetHealthPercantage(),1,1);
        }

        private void EnableBar()
        {
            GetComponentInParent<Canvas>().enabled = true;
        }

        private void DisableBar()
        {
            GetComponentInParent<Canvas>().enabled = false;
        }
    }
}

