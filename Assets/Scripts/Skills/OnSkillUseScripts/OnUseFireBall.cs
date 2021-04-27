using System.Collections;
using System.Collections.Generic;
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

            Vector3 castingPoint;
            if(skill.IsCastedByRightHand)
            {
                castingPoint = player.GetComponent<PlayerSkills>().RightHandTransform.position;
            }
            else castingPoint = player.GetComponent<PlayerSkills>().LeftHandTransform.position;
            skill.CastAOE2Spell();
        }
    }
}
