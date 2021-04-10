using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Attributes
{
    public static class AttributeFormulas
    {
        public static float ArmorDamageMultiplier(float armor)
        {
            return 1 - (0.052f * armor)/(0.9f + 0.048f * Mathf.Abs(armor));
        }

        public static float AttacksPerSecond(float basicAttackTime,float attackSpeed)
        {
            return (0.01f * attackSpeed)/basicAttackTime;
        }
        
        public static float AttackTime(float basicAttackSpeed,float attackSpeed)
        {
            return 1/AttacksPerSecond(basicAttackSpeed,attackSpeed);
        }
    }
}
