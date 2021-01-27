using UnityEngine;
using RPG.Core;

namespace RPG.Combat
{    
    public class CombatTarget : MonoBehaviour ,IAction
    {
        [SerializeField]float health = 100f;
        [SerializeField]float bodyDissapearTime = 10f;
        float timerBodyDissapear = 0f;
        bool isDead = false;
        
        private void Update()
        {
            if(isDead)
            {
                timerBodyDissapear += Time.deltaTime;
                if(timerBodyDissapear > bodyDissapearTime)
                {
                    Destroy(gameObject);
                }
            }
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
            Destroy(GetComponent<CapsuleCollider>());
            Destroy(GetComponent("NavMeshAgent"));
        }

        public void Cancel() 
        {
            //Destroy(gameObject);
        }
    }
}
