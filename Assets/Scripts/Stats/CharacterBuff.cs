using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    public class CharacterBuff : MonoBehaviour, IAdditiveModifier
    {
        float duration = -1;
        float durationTimer;
        [SerializeField] StatFloatDictionary buffs = new StatFloatDictionary();
        
        private void Update()
        {
            if(duration > 0)
            {
                if(duration < durationTimer)
                {
                    GameObject.Destroy(this);
                }
                durationTimer += Time.deltaTime;
            }
        }

        public void IntitializeBuff(StatFloatDictionary buffs, float duration = -1)
        {
            foreach(var buff in buffs)
            {
                this.buffs.Add(buff.Key,buff.Value);
            }
            this.duration = duration;
            GetComponent<BaseStats>().InvokeOnAttributesChanged();
        }

        public void AddBuff(Stat stat, float value)
        {
            buffs.Add(stat,value);
            GetComponent<BaseStats>().InvokeOnAttributesChanged();
        }

        public IEnumerable<float> GetAdditiveModifier(Stat stat)
        {
            if(buffs.ContainsKey(stat)) yield return buffs[stat];
        }
    }
}
