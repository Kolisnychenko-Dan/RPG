using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Range Weapon", menuName = "Weapons/Make New Range Weapon", order = 1)]
    public class RangeWeapon : Weapon
    {
        [SerializeField] GameObject projectilePrefab = null;
        
        public void Shoot(CombatTarget target, Vector3 weaponPosition, float damageMultiplier)
        {
            GameObject projectile = Instantiate(projectilePrefab);
            projectile.transform.position = weaponPosition;
            
            Projectile projectileComponent = projectile.GetComponent<Projectile>();
            projectileComponent.ProjectileDamage = base.GetWeaponDamage * damageMultiplier;
            projectileComponent.Target = target;
        }
    }
}
