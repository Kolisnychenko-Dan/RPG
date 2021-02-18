using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Stats;

namespace RPG.Attributes
{
    public class MobExperience : MonoBehaviour
    {
        [SerializeField] float experienceGainRadius = 8f;
        float DieExperience;

        private void Start() 
        {
            DieExperience = GetComponent<BaseStats>().GetStat(Stat.Experience);
        }

        public void Die() 
        {
            var playerExperience = FindObjectOfType<PlayerExperience>();
            
            bool isInGainExperienceRadius = Vector3.Distance(playerExperience.transform.position, transform.position) < experienceGainRadius;
            if(isInGainExperienceRadius)
            {
                playerExperience.GainExperience(DieExperience);
            }
        }
    }
}

