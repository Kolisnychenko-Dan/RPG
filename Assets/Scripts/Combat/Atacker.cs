using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using RPG.Attributes;
using System;

namespace RPG.Combat
{
    [RequireComponent(typeof(Mover))]
    public class Atacker : MonoBehaviour, IAction, ISaveable
    {
        [SerializeField] Transform rightHandTransform = null;
        [SerializeField] Transform leftHandTransform = null;
        [SerializeField] string currentWeaponName = null;
        [SerializeField] string defaultWeaponName;
        [SerializeField] string attackStateName = "Attack";
        Weapon currentWeapon;
        GameObject currentWeaponObject;
        float waitTillAttackTime;
        float timePassedAfterAttack = Mathf.Infinity;
        float attackTime;
        CombatTarget target;
        BaseStats baseStats;
        Animator animator;

        private void Awake()
        {
            baseStats = GetComponent<BaseStats>();
            animator = GetComponent<Animator>();
            baseStats.OnAttributesChanged += CalulateAttackSpeed;
        }

        private void Start()
        {
            CalulateAttackSpeed();

            if(currentWeaponName == null) currentWeaponName = defaultWeaponName;
            EquipWeapon(currentWeaponName);
        }

        private void Update()
        {  
            if(target == null) animator.SetTrigger("stopAttack");

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

        private void CalulateAttackSpeed()
        {
            waitTillAttackTime = 1/AttributeFormulas.AttacksPerSecond(baseStats.GetCalculatedStat(Stat.BasicAttackTime),baseStats.GetCalculatedStat(Stat.AttackSpeed));
            attackTime = AttributeFormulas.AttackTime(baseStats.GetCalculatedStat(Stat.BasicAttackTime),baseStats.GetCalculatedStat(Stat.AttackSpeed));
        }

        private float? GetAttackAnimClipLength()
        {
            var stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            if(stateInfo.IsName(attackStateName))
            {
                return stateInfo.length;
            }
            return null;
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

            currentWeaponObject = currentWeapon.SpawnWeapon(GetTransformOfHandWithWeapon(),animator);
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

            animator.ResetTrigger("attack");
            animator.SetTrigger("attack");
            animator.speed = (GetAttackAnimClipLength() ?? attackTime)/attackTime;
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
            target?.TakeDamage(currentWeapon.GetWeaponDamage * damageMultiplier, DamageType.Physical);
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
            animator.ResetTrigger("attack");
            animator.SetTrigger("stopAttack");
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

