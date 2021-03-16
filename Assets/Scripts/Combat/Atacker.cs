using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Core;
using RPG.Saving;
using RPG.Stats;

namespace RPG.Combat
{
    [RequireComponent(typeof(Mover))]
    public class Atacker : MonoBehaviour, IAction, ISaveable
    {
        [SerializeField] float waitTillAttackTime = 1f;
        [SerializeField] Transform rightHandTransform = null;
        [SerializeField] Transform leftHandTransform = null;
        [SerializeField] string currentWeaponName = null;
        [SerializeField] string defaultWeaponName;
        Weapon currentWeapon;
        GameObject currentWeaponObject;
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
                bool isInRangeOfAttack = Vector3.Distance(target.transform.position, transform.position) < currentWeapon.GetWeaponRange;
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
                Destroy(currentWeaponObject);
                currentWeapon = null;
            }

            currentWeapon = Resources.Load<Weapon>(weaponName);

            if(currentWeapon == null) 
            {
                Debug.Log("Weapon name doesn't correspond any weapon in resources directory");
                currentWeapon = Resources.Load<Weapon>(defaultWeaponName);
            }

            currentWeaponObject = currentWeapon.SpawnWeapon(GetTransformOfHandWithWeapon(),GetComponent<Animator>());
        }

        private Transform GetTransformOfHandWithWeapon()
        {
            return currentWeapon.IsRightHandedWeapon ? rightHandTransform : leftHandTransform;
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
            float damageMultiplier = GetComponent<BaseStats>().GetStat(Stat.DamageMultiplier);
            target?.TakeDamage(currentWeapon.GetWeaponDamage * damageMultiplier);
        }
        
        // Invoked by an Animator component
        void Shoot()
        {
            if(target != null)
            {
                float damageMultiplier = GetComponent<BaseStats>().GetStat(Stat.DamageMultiplier);
                ((RangeWeapon)currentWeapon).Shoot(target, GetTransformOfHandWithWeapon().position, damageMultiplier);
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

