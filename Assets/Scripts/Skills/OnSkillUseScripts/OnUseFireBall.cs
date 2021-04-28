using System.Collections;
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
            
            float damage = skill.Damage;

            player.GetComponent<PlayerController>().PointChoosingMode = (Vector3 destination) => 
                {
                    Transform castingPoint;
                    if(skill.IsCastedByRightHand)
                    {
                        castingPoint = player.GetComponent<PlayerSkills>().RightHandTransform;
                    }
                    else castingPoint = player.GetComponent<PlayerSkills>().LeftHandTransform;
                    skill.CastAOESpell(destination, castingPoint, damage);
                };
        }
    }
}
