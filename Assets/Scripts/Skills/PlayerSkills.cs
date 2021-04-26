using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Skills
{
    public class PlayerSkills : MonoBehaviour
    {
        public UniversalInventorySystem.Inventory skills;
            
        private void Start()
        {
            skills.Initialize();
        }
    }
}

