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

        public event Action<float,HealthChangeType> OnHealthChanged;
        public event Action OnDeath;
        public event Action OnRiseFromTheDead;
        
        private void Awake() 
        {
            GetComponent<BaseStats>().OnAttributesChanged += OnMaxHealthUpdated;
        }

        private void Start() 
        {
            maxHealth = GetComponent<BaseStats>().GetStat(Stat.Health);
            maxHealth += GetComponent<BaseStats>().GetAdditiveModifiers(Stat.Health);

            if(health == -1) 
            {
                health = maxHealth;
            }
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

        public float GetHealthPercantage()
        {
            return health/maxHealth;
        }

        public void TakeDamage(float damage)
        {
            Health -= damage;

            if (health == 0) Die();
        }

        private void OnMaxHealthUpdated()
        {
            float currentMaxHealth = GetComponent<BaseStats>().GetStat(Stat.Health);
            currentMaxHealth += GetComponent<BaseStats>().GetAdditiveModifiers(Stat.Health);

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
            Damage
        }
    }
}
