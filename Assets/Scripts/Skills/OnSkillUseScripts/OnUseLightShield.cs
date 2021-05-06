using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniversalInventorySystem;

namespace RPG.Skills
{
    public class OnUseLightShield : IUsable
    {
        public void OnUse(object sender, InventoryHandler.UseItemEventArgs e)
        {
            var skill = (Skill)e.item;
            var player = GameObject.Find("Player");
            var playerSkills = player.GetComponent<PlayerSkills>();

            if(playerSkills.CanCastSkill(e.slot))
            {
                playerSkills.SkillCasted(e.slot);
                skill.bs.CastBuffSpell(player.transform);
            }
        }
    }
}
