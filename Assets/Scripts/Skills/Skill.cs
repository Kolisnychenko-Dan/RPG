using System.Collections;
using System.Collections.Generic;
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
        [SerializeField] public float coolDown = 0f;       
    }
}