using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
    public class Weapon : ScriptableObject 
    {
        [SerializeField] AnimatorOverrideController weaponOverride = null;
        [SerializeField] GameObject weaponPrefab = null;
        [SerializeField] float weaponDamage = 0;
        [SerializeField] float weaponRange = 0;

        public float WeaponDamage
        {
            get { return weaponDamage; }
        }

        public float WeaponRange
        {
            get { return weaponRange; }
        }

        public void SpawnWeapon(Transform handTransform, Animator animator)
        {
            if(weaponPrefab != null)Instantiate(weaponPrefab, handTransform);
            animator.runtimeAnimatorController = weaponOverride;
        }
    }
}