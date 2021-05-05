using System.Collections;
using System.Collections.Generic;
using RPG.Skills;
using UnityEngine;

namespace RPG.UI
{
    public class OutOfManaTinter : MonoBehaviour
    {
        [SerializeField] GameObject tinter;
        PlayerSkills playerSkills;
        int slot;
        
        private void Awake()
        {
            playerSkills = FindObjectOfType<PlayerSkills>();
            tinter.transform.localScale = Vector3.zero;
        }

        private void Start()
        {
            slot = int.Parse(gameObject.name);
        }

        private void Update()
        {
            if(playerSkills.IsEnaughManaForCast(slot))
            {
                tinter.transform.localScale = new Vector3(1, 0 ,1);
            }
            else tinter.transform.localScale = Vector3.one;
        }
    }
}
