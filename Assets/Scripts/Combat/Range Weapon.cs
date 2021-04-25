using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Range Weapon", menuName = "Weapons/Make New Range Weapon", order = 1)]
    public class RangeWeapon : Weapon
    {
        [SerializeField] GameObject projectilePrefab = null;
        
        public void Shoot(CombatTarget target, Vector3 weaponPosition, float damageMultiplier, DamageType type)
        {
            var projectile = Instantiate(projectilePrefab);
            projectile.transform.position = weaponPosition;
            
            var projectileComponent = projectile.GetComponent<Projectile>();
            projectileComponent.SetUpProjectile(base.GetWeaponDamage * damageMultiplier, target, type);
        }
    }
}
