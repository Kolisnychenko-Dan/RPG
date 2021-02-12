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
        float projectileDamage = 0f;
        CombatTarget target;

        void Update()
        {
            transform.LookAt(target.transform.position + projectileFlyHeight);
            transform.Translate(Vector3.forward * projectileSpeed * Time.deltaTime);
            
            bool hasHitTheTarget = Vector3.Distance(transform.position,target.transform.position) < projectileHitTolerance; 
            if(hasHitTheTarget)
            {
                OnHitTheTarget();
            }
        }

        public CombatTarget Target
        {
            set { target = value; }
            get { return target; }
        }

        public float ProjectileDamage
        {
            set { projectileDamage = Mathf.Max(value,0); }
            get { return ProjectileDamage; }
        }

        void OnHitTheTarget()
        {
            if(impactEffect != null)
            {
                Instantiate(impactEffect,target.transform.position,transform.rotation);
            }

            target.TakeDamage(projectileDamage);
            Destroy(gameObject);
        }
    }
}
