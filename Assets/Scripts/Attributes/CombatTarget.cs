using UnityEngine;
using UnityEngine.AI;
using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using RPG.Attributes;

namespace RPG.Combat
{    
    public class CombatTarget : MonoBehaviour, IAction, ISaveable
    {
        [SerializeField]float health = -1f;
        bool isDead = false;
        float maxHealth;

        private void Start() 
        {
            maxHealth = GetComponent<BaseStats>().GetStat(Stat.Health);

            if(health == -1) 
            {
                health = maxHealth;
            }

            GetComponent<BaseStats>().OnAttributesChanged += OnMaxHealthUpdated;
        }

        public bool IsDead 
        {
            get { return isDead; }
        }

        public void TakeDamage(float damage)
        {
            health = Mathf.Max(health - damage, 0);
        
            if(health == 0)
            {
                Die();
            }
        }

        public void OnMaxHealthUpdated()
        {
            float currentMaxHealth = GetComponent<BaseStats>().GetStat(Stat.Health);
            health = health * (currentMaxHealth / maxHealth);
            maxHealth = currentMaxHealth;
        }

        private void Die()
        {
            isDead = true;
            GetComponent<ActionScheduler>().StartAction(this);

            GetComponent<MobExperience>()?.Die();
            
            DieAnimation();
            GetComponent<Animator>().SetTrigger("die");
        }


        private void DieAnimation()
        {
            Rigidbody rb = gameObject.AddComponent<Rigidbody>();
            rb.velocity = new Vector3(0,-0.05f,0);
            rb.useGravity = false;             
            GetComponent<CapsuleCollider>().enabled = false;
            GetComponent<NavMeshAgent>().enabled = false;
        }

        private void RiseFromTheDead()
        {
            isDead = false;
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
            else RiseFromTheDead();
        }
    }
}
