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
        }

        private void Start() 
        {
            maxHealth = baseStats.GetCalculatedStat(Stat.Health);

            if(health == -1) 
            {
                health = maxHealth;
            }
        }

        private void Update()
        {
            RegenerateHealth();
        }

        public bool IsDead 
        {
            get => isDead;
        }

        public float MaxHealth
        {
            get => maxHealth;
        }

        public float Health
        {
            get => health; 
            set {   
                    HealthChangeType type = value > health ? HealthChangeType.Heal : HealthChangeType.Damage;
                    float calculatedHealth = Mathf.Max( 0, Mathf.Min( maxHealth, value));
                    float healthChange = Mathf.Abs(calculatedHealth - health);
                    
                    health = calculatedHealth; 

                    if(healthChange != 0) 
                        OnHealthChanged.Invoke(healthChange, type);
                }
        }

        private void RegenerateHealth()
        {
            health = Mathf.Min(health + Time.deltaTime * baseStats.GetCalculatedStat(Stat.HealthRegen), maxHealth);
        }

        public float GetHealthPercantage()
        {
            return health/maxHealth;
        }

        public void TakeDamage(float damage, DamageType damageType)
        {
            damage *= AttributeFormulas.ArmorDamageMultiplier(baseStats.GetCalculatedStat(Stat.Armor));
            Health -= damage;
 
            if (health == 0) Die();
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
                controllerToHandle.transform.GetComponent<Atacker>().Atack(this);
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
            IgnoreType
        }
    }
}
