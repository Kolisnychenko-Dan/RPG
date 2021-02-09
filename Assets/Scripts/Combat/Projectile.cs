using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] float projectileSpeed = 3f;
        [SerializeField] float projectileHitTolerance = 0.5f;
        [SerializeField] Vector3 projectileFlyHeight = Vector3.up/2;
        [SerializeField] float projectileDamage = 0f;
        CombatTarget target;

        void Update()
        {
            transform.LookAt(target.transform.position + projectileFlyHeight);
            transform.Translate(Vector3.forward * projectileSpeed * Time.deltaTime);
            
            bool hasHitTheTarget = Vector3.Distance(transform.position,target.transform.position) < projectileHitTolerance; 
            if(hasHitTheTarget)
            {
                target.TakeDamage(projectileDamage);
                Destroy(gameObject);
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
    }
}
