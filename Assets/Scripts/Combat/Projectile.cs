using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] bool isInstantProjectile = false;
        [SerializeField] float projectileSpeed = 3f;
        [SerializeField] float projectileHitTolerance = 0.5f;
        [SerializeField] Vector3 projectileFlyHeight = Vector3.up;
        [SerializeField] GameObject impactEffect = null;
        [SerializeField] DamageType projectileDamageType = DamageType.Physical;
        [SerializeField] float damageDelay = 0;
        float damageDelayTimer;
        bool hasHit = false;
        Action delayedAction = null;
        float projectileDamage = 0f;
        CombatTarget target = null;
        Vector3 vectorTarget;
        float aOERAdius;
        string noDamageTag = null;

        void Update()
        {   
            if(!hasHit)
            {
                if(target != null)
                {
                    transform.LookAt(Target.transform.position + projectileFlyHeight);
                    transform.Translate(Vector3.forward * projectileSpeed * Time.deltaTime);

                    bool hasHitTheTarget = Vector3.Distance(transform.position,Target.transform.position) < projectileHitTolerance; 
                    if(hasHitTheTarget) OnHitTheTarget();
                }
                else
                {
                    if(isInstantProjectile) OnHitVectorTarget();
                    transform.LookAt(vectorTarget + projectileFlyHeight);
                    transform.Translate(Vector3.forward * projectileSpeed * Time.deltaTime);

                    bool hasHitTheVectorTarget = Vector3.Distance(transform.position,vectorTarget) < projectileHitTolerance;
                    if(hasHitTheVectorTarget) OnHitVectorTarget();
                }
            }

            if(delayedAction != null)
            {
                if(damageDelayTimer > damageDelay) 
                {
                    delayedAction.Invoke();
                    delayedAction = null;
                }
                damageDelayTimer += Time.deltaTime;
            }
        }

        public float ProjectileDamage
        {
            set { projectileDamage = Mathf.Max(value,0); }
            get => ProjectileDamage; 
        }

        public CombatTarget Target { get => target; set => target = value; }
        public DamageType ProjectileDamageType { get => projectileDamageType; set => projectileDamageType = value; }

        public void SetUpHomingProjectile(float damage, CombatTarget target, DamageType type)
        {
            ProjectileDamage = damage;
            Target = target;
            ProjectileDamageType = type;
        }

        public void SetUpAOEProjectile(float damage, Vector3 destination, DamageType type, float radius, string noDamageTag = null)
        {
            ProjectileDamage = damage;
            vectorTarget = destination;
            ProjectileDamageType = type;
            aOERAdius = radius;
            this.noDamageTag = noDamageTag;
        }

        void OnHitVectorTarget()
        {
            hasHit = true;

            if(impactEffect != null)
            {
                Instantiate(impactEffect,vectorTarget,transform.rotation);
            }
            
            ActionDelay( () => {
                foreach(var hit in Physics.SphereCastAll(vectorTarget, aOERAdius, Vector3.up,0))
                {
                    CombatTarget target = hit.transform.GetComponent<CombatTarget>();
                    if(target != null && !target.IsDead && target.tag != noDamageTag)
                    {
                        target.TakeDamage(projectileDamage, ProjectileDamageType);
                    }
                }
                Destroy(gameObject);
            });
            
        }

        void OnHitTheTarget()
        {
            hasHit = true;

            if(impactEffect != null)
            {
                Instantiate(impactEffect,Target.transform.position,transform.rotation);
            }

            ActionDelay( () => {
                if(!Target.IsDead) Target.TakeDamage(projectileDamage, ProjectileDamageType);
                Destroy(gameObject);
            });
        }

        void ActionDelay(Action delayedAction)
        {
            if(damageDelay == 0) delayedAction.Invoke();
            else
            {
                this.delayedAction = delayedAction;
                damageDelayTimer = 0;
            }
        }
    }
}
