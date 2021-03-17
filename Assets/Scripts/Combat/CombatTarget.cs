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

        public event Action<float> OnDamageTaken;
        public event Action OnDeath;
        public event Action OnRiseFromTheDead;
        
        private void Awake() 
        {
            GetComponent<BaseStats>().OnAttributesChanged += OnMaxHealthUpdated;
        }

        private void Start() 
        {
            maxHealth = GetComponent<BaseStats>().GetStat(Stat.Health);

            if(health == -1) 
            {
                health = maxHealth;
            }
        }

        public bool IsDead 
        {
            get { return isDead; }
        }

        public float MaxHealth
        {
            get { return maxHealth; }
        }

        public float Health
        {
            get { return health; }
        }

        public float GetHealthPercantage()
        {
            return health/maxHealth;
        }

        public void TakeDamage(float damage)
        {
            health = Mathf.Max(health - damage, 0);

            OnDamageTaken?.Invoke(damage);

            if (health == 0) Die();
        }

        private void OnMaxHealthUpdated()
        {
            float currentMaxHealth = GetComponent<BaseStats>().GetStat(Stat.Health);
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
    }
}
