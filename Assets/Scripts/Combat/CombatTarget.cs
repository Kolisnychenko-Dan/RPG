using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using System.Collections;
using RPG.Core;
using RPG.Saving;

namespace RPG.Combat
{    
    public class CombatTarget : MonoBehaviour, IAction, ISaveable
    {
        [SerializeField]float health = 100f;
//        [SerializeField]float bodyDissapearTime = 10f;
        float timerBodyDissapear = 0f;
        bool isDead = false;
        
        private void Update()
        {
            // if(isDead)
            // {
            //     timerBodyDissapear += Time.deltaTime;
            //     if(timerBodyDissapear > bodyDissapearTime)
            //     {
            //         Destroy(gameObject);
            //     }
            // }
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

        private void Die()
        {
            isDead = true;
            GetComponent<ActionScheduler>().StartAction(this);
            
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

        public void Cancel() 
        {
            //Destroy(gameObject);
        }

        public object CaptureState()
        {
            return health;
        }

        public void RestoreState(object state)
        {
            health = (float)state;
            if(health == 0) isDead = true;
            else RiseFromTheDead();
        }
    }
}
