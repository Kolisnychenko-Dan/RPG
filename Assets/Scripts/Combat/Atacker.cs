using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using RPG.Attributes;

namespace RPG.Combat
{
    [RequireComponent(typeof(Mover))]
    public class Atacker : MonoBehaviour, IAction, ISaveable
    {
        [SerializeField] Transform rightHandTransform = null;
        [SerializeField] Transform leftHandTransform = null;
        [SerializeField] string currentWeaponName = null;
        [SerializeField] string defaultWeaponName;
        [SerializeField] float attackNearestDistance = 5f;
        Weapon currentWeapon;
        GameObject currentWeaponObject;
        float waitTillAttackTime;
        float timePassedAfterAttack = Mathf.Infinity;
        float attackTime;
        CombatTarget target;
        BaseStats baseStats;
        Animator animator;
        bool isInAutoAttackMode;
        string enemyTag;

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
            if(target == null) 
                animator.SetTrigger("stopAttackLight");

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
            if(GetComponent<CombatTarget>().IsStunned)
            {
                animator.SetTrigger("stopAttack");
                return;
            }

            transform.LookAt(target.transform);
            if(timePassedAfterAttack < waitTillAttackTime) return;
            timePassedAfterAttack = 0;

            animator.ResetTrigger("attack");
            animator.SetTrigger("attack");

            animator.speed = currentWeapon.AttackClipLength / attackTime;
        }

        public void Attack(CombatTarget target)
        {
            // void Hit(), void Shoot() is triggered here
            GetComponent<ActionScheduler>().StartAction(this);
            this.target = target;
        }

        public void StartAutoAtacking(CombatTarget target)
        {
            isInAutoAttackMode = true;
            enemyTag = target.tag;

            Attack(target);
            target.OnDeath -= AttackNearestEnemy;
            target.OnDeath += AttackNearestEnemy;
        }

        private void AttackNearestEnemy()
        {
            if(isInAutoAttackMode)
            {
                var characters = GameObject.FindGameObjectsWithTag(enemyTag);

                target = null;
                float currentMinChaseDistance = Mathf.Infinity;

                foreach (var character in characters)
                {
                    if(character.GetComponent<CombatTarget>().IsDead) continue;

                    float distance = Vector3.Distance(transform.position,character.transform.position);
                    if((distance < attackNearestDistance && distance < currentMinChaseDistance))
                    {
                        currentMinChaseDistance = distance;

                        target = character.GetComponent<CombatTarget>();                        
                    }
                }

                if(target == null) 
                {
                    isInAutoAttackMode = false;
                    animator.speed = 1; 
                }
                else {
                    Attack(target);
                    target.OnDeath -= AttackNearestEnemy;
                    target.OnDeath += AttackNearestEnemy;
                }
            }
        }

        // Invoked by an Animator component
        void Hit()
        {
            float damageMultiplier = GetComponent<BaseStats>().GetStat(Stat.DamageMultiplier);
            float critMultiplier = currentWeapon.CritChance > Random.value ? currentWeapon.CritMultiplier : 1f;

            target?.TakeDamage(currentWeapon.GetWeaponDamage * damageMultiplier * critMultiplier,
                critMultiplier == 1f ? DamageType.Physical : DamageType.Critical);
        }
        
        // Invoked by an Animator component
        void Shoot()
        {
            if(target != null)
            {
                float damageMultiplier = GetComponent<BaseStats>().GetStat(Stat.DamageMultiplier);
                float critMultiplier = currentWeapon.CritChance > Random.value ? currentWeapon.CritMultiplier : 1f;

                ((RangeWeapon)currentWeapon).Shoot(target, GetTransformOfHandWithWeapon().position, damageMultiplier * critMultiplier,
                    critMultiplier == 1f ? DamageType.Physical : DamageType.Critical);
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