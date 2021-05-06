using System;
using UnityEngine;
using UnityEngine.AI;
using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using RPG.Attributes;
using RPG.Controller;

namespace RPG.Combat
{    
    public class CombatTarget : MonoBehaviour, IAction, ISaveable, IRayCastable
    {
        [SerializeField]float health = -1f;
        [SerializeField]float dieAnimSpeed = 0.05f;
        bool isDead = false;
        float maxHealth;
        float healthRegen;
        BaseStats baseStats;

        public event Action<float,HealthChangeType> OnHealthChanged;
        public event Action OnDeath;
        public event Action OnRiseFromTheDead;
        
        private void Awake() 
        {
            baseStats = GetComponent<BaseStats>();
            baseStats.OnAttributesChanged += OnMaxHealthUpdated;
            baseStats.OnAttributesChanged += () => healthRegen = baseStats.GetCalculatedStat(Stat.HealthRegen);
        }

        private void Start() 
        {
            maxHealth = baseStats.GetCalculatedStat(Stat.Health);
            healthRegen = baseStats.GetCalculatedStat(Stat.HealthRegen);

            if(health == -1) 
            {
                health = maxHealth;
            }
        }

        private void Update()
        {
            RegenerateHealth();
        }

        public bool IsDead { get => isDead; }

        public float MaxHealth { get => maxHealth; }

        public void ChangeHealth(float value, HealthChangeType type)
        {
            float calculatedHealth = 0;

            if(type == HealthChangeType.Heal)
            {
                calculatedHealth = Mathf.Min( maxHealth, health + value);
            }
            else calculatedHealth = Mathf.Max( 0, health - value);
            
            float healthChange = Mathf.Abs(calculatedHealth - health);
            health = calculatedHealth; 

            if(healthChange != 0) OnHealthChanged.Invoke(healthChange, type);
        }

        public float GetHealthPercantage()
        {
            return health/maxHealth;
        }

        public void TakeDamage(float damage, DamageType damageType)
        {
            if(damage < 0 && damageType != DamageType.IgnoreType) return;

            switch (damageType)
            {
                case DamageType.Critical:
                {
                    damage *= AttributeFormulas.ArmorDamageMultiplier(baseStats.GetCalculatedStat(Stat.Armor));
                    ChangeHealth(damage,HealthChangeType.CritDamage);
                }
                break;
                case DamageType.Physical:
                {
                    damage *= AttributeFormulas.ArmorDamageMultiplier(baseStats.GetCalculatedStat(Stat.Armor));
                    ChangeHealth(damage,HealthChangeType.Damage);
                }
                break;
                case DamageType.Heal:
                {
                    ChangeHealth(damage,HealthChangeType.Heal);
                }
                break;
                case DamageType.Magical:
                {
                    damage *= 1 - baseStats.GetCalculatedStat(Stat.MagDefence);
                    ChangeHealth(damage,HealthChangeType.Damage);
                }
                break;
                case DamageType.Pure:
                {
                    ChangeHealth(damage,HealthChangeType.Damage);
                }
                break;
                case DamageType.IgnoreType:
                {
                    ChangeHealth(damage,HealthChangeType.IgnoreType);
                }
                break;
            }
 
            if (health == 0) Die();
        }

        private void RegenerateHealth()
        {
            health = Mathf.Min(health + Time.deltaTime * healthRegen, maxHealth);
        }

        private void OnMaxHealthUpdated()
        {
            float currentMaxHealth = baseStats.GetCalculatedStat(Stat.Health);

            health = health * (currentMaxHealth / maxHealth);
            maxHealth = currentMaxHealth;
        }

        private void Die()
        {
            isDead = true;
            GetComponent<ActionScheduler>().StartAction(this);
            OnDeath?.Invoke();

            GetComponent<MobExperience>()?.Die();
            
            DieAnimation();
            GetComponent<Animator>().SetTrigger("die");
        }

        // Too much of a pain to do it through animation
        private void DieAnimation()
        {
            Rigidbody rb = gameObject.AddComponent<Rigidbody>();
            rb.velocity = new Vector3(0,-dieAnimSpeed,0);
            rb.useGravity = false;             
            GetComponent<CapsuleCollider>().enabled = false;
            GetComponent<NavMeshAgent>().enabled = false;
        }

        private void RiseFromTheDeadAnimation()
        {
            isDead = false;
            OnRiseFromTheDead?.Invoke();

            Destroy(GetComponent<Rigidbody>());
            GetComponent<Animator>().SetTrigger("riseFromTheDead");
            GetComponent<CapsuleCollider>().enabled = true;
            GetComponent<NavMeshAgent>().enabled = true;
        }

        public void Cancel() {}

        public object CaptureState()
        {
            return health;
        }

        public void RestoreState(object state)
        {
            health = (float)state;
            if(health == 0) Die();
            else RiseFromTheDeadAnimation();
        }

        public bool HandleRaycast(PlayerController controllerToHandle)
        {
            if(Input.GetMouseButton(0))
            {
                if(controllerToHandle.AutoAttack) 
                {
                   controllerToHandle.gameObject.GetComponent<Atacker>().StartAutoAtacking(this);
                }
                else controllerToHandle.transform.GetComponent<Atacker>().Attack(this);
            }
            return true;
        }

        public CursorType GetHoverCursor()
        {
            return tag == "Player" ? CursorType.Movement : CursorType.Attack; 
        }

        public enum HealthChangeType
        {
            Heal,
            Damage,
            CritDamage,
            IgnoreType
        }
    }
}
