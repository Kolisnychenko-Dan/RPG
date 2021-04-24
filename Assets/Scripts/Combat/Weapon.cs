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
        [Range(0,1)] [SerializeField] float critChance = 0;
        [Min(1)][SerializeField] float critMultiplier = 1;
        [SerializeField] float weaponRange = 0;
        [SerializeField] float attackClipLength;
        
        public bool IsRightHandedWeapon { get => isRightHandedWeapon; }
        public float GetWeaponDamage { get => weaponDamage; }
        public float GetWeaponRange { get => weaponRange; }
        public float AttackClipLength { get => attackClipLength; }
        public float CritChance { get => critChance; }
        public float WeaponDamage { get => weaponDamage; }
        public float CritMultiplier { get => critMultiplier; }

        public GameObject SpawnWeapon(Transform handTransform, Animator animator)
        {
            GameObject weaponObject = null;
            if(weaponPrefab != null) weaponObject = Instantiate(weaponPrefab, handTransform);
            animator.runtimeAnimatorController = weaponOverride;
            return weaponObject;
        }
    }
}