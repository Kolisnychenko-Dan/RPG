using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] float projectileSpeed = 3f;
        [SerializeField] float projectileHitTolerance = 0.5f;
        [SerializeField] Vector3 projectileFlyHeight = Vector3.up;
        [SerializeField] GameObject impactEffect = null;
        [SerializeField] DamageType projectileDamageType = DamageType.Physical;
        float projectileDamage = 0f;
        CombatTarget target;

        void Update()
        {
            transform.LookAt(Target.transform.position + projectileFlyHeight);
            transform.Translate(Vector3.forward * projectileSpeed * Time.deltaTime);
            
            bool hasHitTheTarget = Vector3.Distance(transform.position,Target.transform.position) < projectileHitTolerance; 
            if(hasHitTheTarget)
            {
                OnHitTheTarget();
            }
        }


        public float ProjectileDamage
        {
            set { projectileDamage = Mathf.Max(value,0); }
            get => ProjectileDamage; 
        }

        public CombatTarget Target { get => target; set => target = value; }
        public DamageType ProjectileDamageType { get => projectileDamageType; set => projectileDamageType = value; }

        public void SetUpProjectile(float damage, CombatTarget target, DamageType type)
        {
            ProjectileDamage = damage;
            Target = target;
            ProjectileDamageType = type;
        }

        void OnHitTheTarget()
        {
            if(impactEffect != null)
            {
                Instantiate(impactEffect,Target.transform.position,transform.rotation);
            }

            if(!Target.IsDead) Target.TakeDamage(projectileDamage, ProjectileDamageType);
            Destroy(gameObject);
        }
    }
}
