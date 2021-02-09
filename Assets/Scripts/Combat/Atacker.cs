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
        [SerializeField] float waitTillAttackTime = 1f;
        [SerializeField] Transform handTransform = null;
        [SerializeField] Weapon currentWeapon = null;
        [SerializeField] Weapon defaultWeapon;
        float timePassedAfterAttack = Mathf.Infinity;
        CombatTarget target;

        private void Start()
        {
            if(currentWeapon == null) currentWeapon = defaultWeapon;
            EquipWeapon(currentWeapon);
        }

        private void Update()
        {  
            timePassedAfterAttack += Time.deltaTime;
            if(target != null && !target.IsDead && target != GetComponent<CombatTarget>())
            {
                bool isInRangeOfAttack = Vector3.Distance(target.transform.position, transform.position) < currentWeapon.WeaponRange;
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

        private void EquipWeapon(Weapon weapon)
        {
            weapon.SpawnWeapon(handTransform,GetComponent<Animator>());
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
            // void Hit(), void Shoot() is triggered here
            GetComponent<ActionScheduler>().StartAction(this);
            this.target = target;
        }

        // Invoked by an Animator component
        void Hit()
        {
            target?.TakeDamage(currentWeapon.WeaponDamage);
        }
        
        // Invoked by an Animator component
        void Shoot()
        {
            if(target != null)
            {
                ((RangeWeapon)currentWeapon).Shoot(target,handTransform.position);
            }
        }
        public void Cancel()
        {
            GetComponent<Animator>().ResetTrigger("attack");
            GetComponent<Animator>().SetTrigger("stopAttack");
            target = null;
        }
    }
}

