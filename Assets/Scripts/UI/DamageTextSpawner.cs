using System.Collections;
using Febucci.UI;
using UnityEngine;
using RPG.Combat;

namespace RPG.UI
{
    [RequireComponent(typeof(CombatTarget))]
    public class DamageTextSpawner : MonoBehaviour
    {
        [SerializeField] GameObject damageTextPrefab;
        [SerializeField] string openingTags = "{size}<shake><fade>";
        [SerializeField] string closingTags = "{/size}</shake></fade>";

        private void Awake() 
        {
            GetComponent<CombatTarget>().OnDamageTaken += Spawn; 
        }

        public void Spawn(float damage)
        {
            GameObject damageText = GameObject.Instantiate(damageTextPrefab,transform);
            TextAnimatorPlayer text = damageText.GetComponentInChildren<TextAnimatorPlayer>();
            text.ShowText(openingTags + damage.ToString() + closingTags);
        }
    }
}
