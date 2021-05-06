using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniversalInventorySystem;

namespace RPG.Skills
{
    public class OnUseFireBall : OnUseDirectSkillBase
    {
        public override void OnUse(object sender, InventoryHandler.UseItemEventArgs e) => base.OnUse(sender, e);
        public override void OnSkillUsed(Vector3 destination, Skill skill, GameObject player)
        {
            Transform castingPoint;
            if(skill.dps.IsCastedByRightHand)
            {
                castingPoint = player.GetComponent<PlayerSkills>().RightHandTransform;
            }
            else castingPoint = player.GetComponent<PlayerSkills>().LeftHandTransform;

            skill.dps.CastAOESpell(destination, castingPoint);
        }
    }
}
