using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MagicalFX;
using RPG.Combat;
using RPG.Stats;

namespace RPG.Skills
{
    public class LightShield : BuffEffectAbstract
    {
        float damageToBlock;
        CombatTarget targetToBlock;

        private void Start()
        {
            damageToBlock = unit.GetComponent<BaseStats>().GetCalculatedStat(Stat.DamageBlock);
            targetToBlock = unit.GetComponent<CombatTarget>();
            targetToBlock.OnHealthChanged += BlockDamage;
        }

        private void BlockDamage(float value, CombatTarget.HealthChangeType type)
        {
            if(damageToBlock > 0 && CombatTarget.HealthChangeType.Heal != type && CombatTarget.HealthChangeType.IgnoreType != type)
            {
                targetToBlock.TakeDamage(-value, DamageType.IgnoreType);
                damageToBlock -= value;
                if(damageToBlock < value )
                {
                    lifeTime?.OnSpawnAfterDead();
                }
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            targetToBlock.OnHealthChanged -= BlockDamage;
        }
    }
}
