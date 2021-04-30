using System.Collections;
using System.Collections.Generic;
using RPG.Attributes;
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
            var playerSkills = player.GetComponent<PlayerSkills>();

            if(playerSkills.CanCastSkill(e.slot))
            {
                float damage = skill.Damage;

                player.GetComponent<PlayerController>().PointChoosingMode = (Vector3 destination) => 
                    {
                        Transform castingPoint;
                        if(skill.IsCastedByRightHand)
                        {
                            castingPoint = player.GetComponent<PlayerSkills>().RightHandTransform;
                        }
                        else castingPoint = player.GetComponent<PlayerSkills>().LeftHandTransform;

                        playerSkills.SkillCasted(e.slot);
                        skill.CastAOESpell(destination, castingPoint, damage);
                    };
            }
        }
    }
}
