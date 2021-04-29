using System.Collections;
using System.Collections.Generic;
using RPG.Skills;
using UnityEngine;

namespace RPG.UI
{
    public class CoolDownTinter : MonoBehaviour
    {
        [SerializeField] GameObject Tinter;
        PlayerSkills playerSkills;
        
        private void Awake()
        {
            playerSkills = FindObjectOfType<PlayerSkills>();
            Tinter.transform.localScale = Vector3.zero;
        }

        private void LateUpdate()
        {
            float coolDownProgress = playerSkills.GetCooldownProgress(int.Parse(gameObject.name));
            if(coolDownProgress != -1)
            {
                Tinter.transform.localScale = new Vector3(1, 1 - coolDownProgress,1);
            }
        }
    }   
}
