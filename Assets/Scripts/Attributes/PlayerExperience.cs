using System;
using UnityEngine;
using RPG.Stats;
using RPG.Saving;

namespace RPG.Attributes
{
    public class PlayerExperience : MonoBehaviour, ISaveable
    {
        [SerializeField] float LevelUpExperience;
        [SerializeField] float currentExperience;
        [SerializeField] GameObject levelUpEffect = null;
        BaseStats baseStats;

//        public event Action OnLevelUp;

        private void Awake() 
        {
            baseStats = GetComponent<BaseStats>();
        }
        private void Start() 
        {
            LevelUpExperience = baseStats.GetStat(Stat.Experience);
        }

        public void GainExperience(float experience)
        {
            currentExperience += experience;
            while(currentExperience > LevelUpExperience) 
            {
                currentExperience -= LevelUpExperience;
                LevelUp();
            }
        }

        private void LevelUp()
        {
//            OnLevelUp?.Invoke();
            LevelUpExperience = baseStats.LevelUp();

            LevelUpEffect();
        }

        private void LevelUpEffect()
        {
            Instantiate(levelUpEffect,transform);
        }

        public object CaptureState()
        {
            return (object)currentExperience;
        }

        public void RestoreState(object state)
        {
            currentExperience = (float)state;
        }
    }
}