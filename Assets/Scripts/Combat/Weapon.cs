using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
    public class Weapon : ScriptableObject 
    {
        [SerializeField] AnimatorOverrideController weaponOverride = null;
        [SerializeField] bool isRightHandedWeapon = true;
        [SerializeField] GameObject weaponPrefab = null;
        [SerializeField] float weaponDamage = 0;
        [SerializeField] float weaponRange = 0;
        GameObject weaponObject = null;
        
        public bool IsRightHandedWeapon
        {
            get { return isRightHandedWeapon; }
        }

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
            if(weaponPrefab != null) Instantiate(weaponPrefab, handTransform);
            animator.runtimeAnimatorController = weaponOverride;
        }

        public void DestroyWeapon()
        {
            Destroy(weaponObject);
        }
    }
}