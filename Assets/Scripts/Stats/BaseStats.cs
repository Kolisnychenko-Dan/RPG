using System;
using UnityEngine;
using RPG.Saving;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour, ISaveable
    {
        [Range(1, 99)]
        [SerializeField] int level = 1;
        [SerializeField] string character;
        [SerializeField] Progression progression = null;

        public event Action OnAttributesChanged;
        public float GetStat(Stat stat)
        {
            return progression.GetStat(character, stat, level);
        }

        /* Should only be called by Player. Returns new required levelUp experience*/
        public float LevelUp()
        {
            level += 1;
            OnAttributesChanged.Invoke();
            return progression.GetStat(character, Stat.Experience ,level);
        }

        
        public object CaptureState()
        {
            return (object)level;
        }

        public void RestoreState(object state)
        {
            level = (int)state;
        }
    }

}
