using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/Progression", order = 0)]
    public class Progression : ScriptableObject
    {
        [SerializeField] StringStatsClass[] statobjectClasses; 

        public float GetStat(string statobject, Stat stat, int level)
        {
            foreach (var el in statobjectClasses)
            {
              if(statobject == el.statobject) return el.statsClass.GetStat(stat,level);
            }
            throw new Exception($"StatObject: \"{statobject}\" doesn't exist in progression");
        }

        [Serializable]
        class StringStatsClass
        {
            public string statobject;
            public StatsClass statsClass;
        }
    }
}
