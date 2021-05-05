using System.Collections;
using System.Collections.Generic;
using RPG.Controller;
using UnityEngine;
using UniversalInventorySystem;

namespace RPG.Skills
{
    public abstract class OnUseDirectSkillBase : IUsable
    {
        public virtual void OnUse(object sender, InventoryHandler.UseItemEventArgs e)
        {
            var skill = (Skill)e.item;
            var player = GameObject.Find("Player");
            var playerSkills = player.GetComponent<PlayerSkills>();

            if(playerSkills.CanCastSkill(e.slot))
            {
                player.GetComponent<PlayerController>().PointChoosingMode = (Vector3 destination) => {
                    OnSkillUsed(destination,skill,player);
                    playerSkills.SkillCasted(e.slot);
                };
            }
        }

        public abstract void OnSkillUsed(Vector3 destination, Skill skill, GameObject player);
    }
}

