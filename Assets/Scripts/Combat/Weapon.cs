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
        
        const string fileName = "Weapon";
        public bool IsRightHandedWeapon
        {
            get { return isRightHandedWeapon; }
        }

        public float GetWeaponDamage
        {
            get { return weaponDamage; }
        }

        public float GetWeaponRange
        {
            get { return weaponRange; }
        }

        public GameObject SpawnWeapon(Transform handTransform, Animator animator)
        {
            GameObject weaponObject = null;
            if(weaponPrefab != null) weaponObject = Instantiate(weaponPrefab, handTransform);
            animator.runtimeAnimatorController = weaponOverride;
            return weaponObject;
        }
    }
}