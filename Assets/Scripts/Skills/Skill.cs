using System;
using System.Collections.Generic;
using RPG.Combat;
using RPG.Stats;
using UnityEngine;
using UniversalInventorySystem;

namespace RPG.Skills
{
    [ 
        CreateAssetMenu(fileName = "Skill", menuName = "Skill"), 
        System.Serializable
    ]
    public class Skill : UniversalInventorySystem.Item
    {
        public DamagingProjectileStats dps = null;
        public BuffStats bs = null;
        public PassiveStats ps = null;

        [Min(0)] [SerializeField] private float manaRequired;
        [Min(0)] [SerializeField] private float coolDown;

        public float ManaRequired { get => manaRequired; }
        public float CoolDown { get => coolDown; }

        [Serializable]
        public class DamagingProjectileStats
        {
            [Min(0)] [SerializeField] private float aOERadius;
            [Min(0)] [SerializeField] private float damage;

            [SerializeField] private DamageType type;
            [SerializeField] private bool isCastedByRightHand;
            [SerializeField] private GameObject projectilePrefab;

            public float AOERadius { get => aOERadius; }
            public float Damage { get => damage; }
            public DamageType Type { get => type; }
            public bool IsCastedByRightHand { get => isCastedByRightHand; }
            public GameObject ProjectilePrefab { get => projectilePrefab; }

            public void CastAOESpell(Vector3 destination, Transform casterTransform)
            {
                var projectile = Instantiate(ProjectilePrefab);
                projectile.transform.position = casterTransform.position;
                projectile.tag = casterTransform.tag;

                var projectileComponent = projectile.GetComponent<Projectile>();
                projectileComponent.SetUpAOEProjectile(Damage, destination, Type, AOERadius, casterTransform.tag);
            }
        } 

        [Serializable]
        public class BuffStats
        {
            [SerializeField] GameObject effect;
            [SerializeField] StatFloatDictionary buffs;
            [Min(0)] [SerializeField] private float duration;  

            public GameObject Effect { get => effect; }
            public float Duration { get => duration; }
            public StatFloatDictionary Buffs { get => buffs; set => buffs = value; }

            public void CastBuffSpell(Transform casterTransform)
            {
                var effectObj = Instantiate(effect, casterTransform.position, casterTransform.rotation);
                effectObj.GetComponent<BuffEffectAbstract>().SetUpBuffEffect(casterTransform,this);
            }
        }

        [Serializable]
        public class PassiveStats
        {
            [SerializeField] StatFloatDictionary buffs;

            public StatFloatDictionary Buffs { get => buffs; set => buffs = value; }

            public void CreateBuff(Transform casterTransform)
            {
                var characterBuff = casterTransform.gameObject.AddComponent<CharacterBuff>();
                characterBuff.IntitializeBuff(Buffs);
            }
        }
    }
}