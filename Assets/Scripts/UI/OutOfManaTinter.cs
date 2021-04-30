using System.Collections;
using System.Collections.Generic;
using RPG.Skills;
using UnityEngine;

public class OutOfManaTinter : MonoBehaviour
{
    [SerializeField] GameObject Tinter;
    PlayerSkills playerSkills;
    
    private void Awake()
    {
        playerSkills = FindObjectOfType<PlayerSkills>();
    }

    private void LateUpdate()
    {

    }
}
