using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Skills
{
    public class PlayerSkills : MonoBehaviour
    {
        [SerializeField] UniversalInventorySystem.Inventory skills;

        [SerializeField] Transform rightHandTransform;
        [SerializeField] Transform leftHandTransform;

        public Transform RightHandTransform { get => rightHandTransform; }
        public Transform LeftHandTransform { get => leftHandTransform; }

        private void Start()
        {
            skills.Initialize();
        }
    }
}

