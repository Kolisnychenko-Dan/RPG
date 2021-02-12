using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Core;
using RPG.Saving;

namespace RPG.Combat
{
    [RequireComponent(typeof(Mover))]
    public class Atacker : MonoBehaviour, IAction, ISaveable
    {
        [SerializeField] float waitTillAttackTime = 1f;
        [SerializeField] Transform rightHandTransform = null;
        [SerializeField] Transform leftHandTransform = null;
        Weapon currentWeapon;
        [SerializeField] string currentWeaponName = null;
        [SerializeField] string defaultWeaponName;
        float timePassedAfterAttack = Mathf.Infinity;
        CombatTarget target;

        private void Start()
        {
            if(currentWeaponName == null) currentWeaponName = defaultWeaponName;
            EquipWeapon(currentWeaponName);
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

        private void EquipWeapon(string weaponName)
        {
            if(currentWeapon != null)
            {
                currentWeapon.DestroyWeapon();
                currentWeapon = null;
            }

            currentWeapon = Resources.Load<Weapon>(weaponName);

            if(currentWeapon == null) 
            {
                Debug.Log("Weapon name doesn't correspond any weapon in resources directory");
                currentWeapon = Resources.Load<Weapon>(defaultWeaponName);
            }

            if(currentWeapon.IsRightHandedWeapon)
            {
                currentWeapon.SpawnWeapon(rightHandTransform,GetComponent<Animator>());
            }
            else
            {
                currentWeapon.SpawnWeapon(leftHandTransform,GetComponent<Animator>());
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
                if(currentWeapon.IsRightHandedWeapon)
                {
                    ((RangeWeapon)currentWeapon).Shoot(target,rightHandTransform.position);
                }
                else
                {
                    ((RangeWeapon)currentWeapon).Shoot(target,rightHandTransform.position);
                }
                
            }
        }
        public void Cancel()
        {
            GetComponent<Animator>().ResetTrigger("attack");
            GetComponent<Animator>().SetTrigger("stopAttack");
            target = null;
        }

        public object CaptureState()
        {
            return (object)currentWeaponName;
        }

        public void RestoreState(object state)
        {
            currentWeaponName = (string)state;
            EquipWeapon(currentWeaponName);
        }
    }
}

