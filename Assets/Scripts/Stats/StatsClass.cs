using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    [Serializable]
    public class StatsClass
    {
        [SerializeField] Stats[] stats;

        public float GetStat(Stat stat,int level)
        {
            foreach (var el in stats)
            {
                if(stat == el.Stat) return el.LevelStat(level-1);
            }
            return 0;
        }

        [Serializable]
        class Stats 
        {
            [SerializeField] Stat stat;
            [SerializeField] float[] levelStats;

            public Stat Stat
            {
                get { return stat; }
            }

            public float LevelStat(int index)
            {
                if(index < levelStats.Length) {
                    return levelStats[index];
                }
                Debug.Log($"The value for level {index + 1} of stat {stat} is not defined");
                return levelStats[levelStats.Length - 1];
            }
        }
    }
}
