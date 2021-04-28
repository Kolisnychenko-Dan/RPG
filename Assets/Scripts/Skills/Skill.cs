﻿using System.Collections;
using System.Collections.Generic;
using RPG.Combat;
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
        [Min(0)] [SerializeField] float coolDown = 0f;
        [Min(0)] [SerializeField] float aOERadius = 0f;
        [Min(0)] [SerializeField] float damage = 0f;
        [SerializeField] DamageType type;
        [SerializeField] bool isCastedByRightHand;
        [SerializeField] GameObject projectilePrefab = null;

        public float CoolDown { get => coolDown; set => coolDown = value; }
        public float AOERadius { get => aOERadius; }
        public bool IsCastedByRightHand { get => isCastedByRightHand; }
        public DamageType Type { get => type; set => type = value; }
        public float Damage { get => damage; set => damage = value; }

        public void CastAOESpell(Vector3 destination, Transform casterTransform, float damage)
        {
            var projectile = Instantiate(projectilePrefab);
            projectile.transform.position = casterTransform.position;
            projectile.tag = casterTransform.tag;

            var projectileComponent = projectile.GetComponent<Projectile>();
            projectileComponent.SetUpAOEProjectile(damage, destination, type, AOERadius, casterTransform.tag);
        }
    }
}