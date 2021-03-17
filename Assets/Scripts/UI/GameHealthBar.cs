using System;
using RPG.Combat;
using UnityEngine;

namespace RPG.UI
{
    public class GameHealthBar : MonoBehaviour
    {
        CombatTarget health;
        private void Awake()
        {
            health = GetComponentInParent<CombatTarget>();

            health.OnDamageTaken += UpdateHealthBar;
            health.OnDeath += DisableBar;
            health.OnRiseFromTheDead += EnableBar;
        }

        private void Update()
        {
            DisableBar();
            if(Input.GetKey(KeyCode.LeftAlt))
            {
                EnableBar();
            }
        }

        private void UpdateHealthBar(float damage)
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

