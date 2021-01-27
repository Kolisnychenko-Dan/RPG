using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Core;

namespace RPG.Combat
{
    [RequireComponent(typeof(Mover))]
    public class Atacker : MonoBehaviour, IAction
    {
        [SerializeField] float weaponRange = 2f;
        [SerializeField] float waitTillAttackTime = 1f;
        [SerializeField] float damage = 10f;

        float timePassedAfterAttack;
        CombatTarget target;
        private void Update()
        {  
            timePassedAfterAttack += Time.deltaTime;
            if(target != null && !target.IsDead && target != GetComponent<CombatTarget>())
            {
                bool isInRangeOfAttack = Vector3.Distance(target.transform.position, transform.position) < weaponRange;
                if(!isInRangeOfAttack)
                {
                    GetComponent<Mover>().MoveTo(target.transform.position);
                }
                else
                {
                    GetComponent<Mover>().Cancel();

                    AttackBehaviour();
                }
            }
        }

        private void AttackBehaviour()
        {
            transform.LookAt(target.transform);
            if(timePassedAfterAttack < waitTillAttackTime) return;
            timePassedAfterAttack = 0;

            GetComponent<Animator>().ResetTrigger("attack");
            GetComponent<Animator>().SetTrigger("attack");
        }

        public void Atack(CombatTarget target)
        {
            // void Hit() is triggered here
            GetComponent<ActionScheduler>().StartAction(this);
            this.target = target;
        }

        // Invoked by an animator
        void Hit()
        {
            if(target != null) target.TakeDamage(damage);
        }

        public void Cancel()
        {
            GetComponent<Animator>().ResetTrigger("attack");
            GetComponent<Animator>().SetTrigger("stopAttack");
            target = null;
        }
    }
}

