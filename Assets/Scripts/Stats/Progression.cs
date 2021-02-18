using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/Progression", order = 0)]
    public class Progression : ScriptableObject
    {
        [SerializeField] StringStatsClass[] characterClasses; 

        public float GetStat(string character, Stat stat, int level)
        {
            foreach (var el in characterClasses)
            {
              if(character == el.character) return el.statsClass.GetStat(stat,level);
            }
            throw new Exception($"Character: \"{character}\" doesn't exist in progression");
        }

        [Serializable]
        class StringStatsClass
        {
            public string character;
            public StatsClass statsClass;
        }
    }
}
