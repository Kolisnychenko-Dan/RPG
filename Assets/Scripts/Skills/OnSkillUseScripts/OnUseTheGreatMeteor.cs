using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniversalInventorySystem;

namespace RPG.Skills
{
    public class OnUseTheGreatMeteor : OnUseDirectSkillBase
    {
        public override void OnUse(object sender, InventoryHandler.UseItemEventArgs e) => base.OnUse(sender, e);
        public override void OnSkillUsed(Vector3 destination, Skill skill, GameObject player)
        {
            skill.dps.CastProjectileAOESpell(destination,player.transform);
        }
    }
}
