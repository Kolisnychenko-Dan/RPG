using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MagicalFX;
using RPG.Stats;

namespace RPG.Skills
{
    public abstract class BuffEffectAbstract : MonoBehaviour
    {
        protected Transform unit;
        protected FX_LifeTime lifeTime;
        protected Skill.BuffStats skill;
        protected CharacterBuff characterBuff;

        protected virtual void Awake()
        {
            lifeTime = GetComponent<FX_LifeTime>();
        }

        protected virtual void Update()
        {
            transform.position = unit.transform.position;
        }

        public virtual void SetUpBuffEffect(Transform character, Skill.BuffStats skill)
        {
            unit = character;
            this.skill = skill;
            lifeTime.LifeTime = skill.Duration;
            if(skill.Buffs.Count != 0)
            {
                characterBuff = character.gameObject.AddComponent<CharacterBuff>();
                characterBuff.IntitializeBuff(skill.Buffs,skill.Duration);
            }
        }

        protected virtual void OnDestroy()
        {
            Destroy(characterBuff);
        }
    }
}