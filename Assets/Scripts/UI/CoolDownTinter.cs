using System.Collections;
using System.Collections.Generic;
using RPG.Skills;
using UnityEngine;

namespace RPG.UI
{
    public class CoolDownTinter : MonoBehaviour
    {
        [SerializeField] GameObject tinter;
        Animation coolDownPassedAnimation;
        PlayerSkills playerSkills;
        int slot;
        float lastTinterValue = 0;
        
        private void Awake()
        {
            playerSkills = FindObjectOfType<PlayerSkills>();
            coolDownPassedAnimation = GetComponentInChildren<Animation>();

            tinter.transform.localScale = Vector3.zero;
        }

        private void Start()
        {
            slot = int.Parse(gameObject.name);
        }

        private void Update()
        {
            float coolDownProgress = playerSkills.GetCooldownProgress(slot);
            if(coolDownProgress == -1)
            {
                if(lastTinterValue != -1)
                {
                    coolDownPassedAnimation.Play();
                }
            }
            else tinter.transform.localScale = new Vector3(1, 1 - coolDownProgress,1);

            lastTinterValue = coolDownProgress;
        }
    }   
}
