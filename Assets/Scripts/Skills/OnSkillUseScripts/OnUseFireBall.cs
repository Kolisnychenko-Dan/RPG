﻿using System.Collections;
using System.Collections.Generic;
using RPG.Controller;
using RPG.Stats;
using UnityEngine;
using UniversalInventorySystem;

namespace RPG.Skills
{
    public class OnUseFireBall : IUsable
    {
        public void OnUse(object sender, InventoryHandler.UseItemEventArgs e)
        {
            var skill = (Skill)e.item;
            var player = GameObject.Find("Player");
            float damage = 5;//player.GetComponent<SkillStats>().GetStat(Stat.HealthInstaHeal,skillName);

            player.GetComponent<PlayerController>().PointChoosingMode = (Vector3 destination) => 
                {
                    Vector3 castingPoint;
                    if(skill.IsCastedByRightHand)
                    {
                        castingPoint = player.GetComponent<PlayerSkills>().RightHandTransform.position;
                    }
                    else castingPoint = player.GetComponent<PlayerSkills>().LeftHandTransform.position;
                    skill.CastAOESpell(destination, castingPoint, damage);
                };
        }
    }
}
