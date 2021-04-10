using System.Collections;
using Febucci.UI;
using UnityEngine;
using RPG.Combat;
using TMPro;

namespace RPG.UI
{
    [RequireComponent(typeof(CombatTarget))]
    public class HealthTextSpawner : MonoBehaviour
    {
        [SerializeField] GameObject damageTextPrefab;
        [SerializeField] string openingTags = "{size}<shake><fade>";
        [SerializeField] string closingTags = "{/size}</shake></fade>";
        [SerializeField] Color damageTextColor = new Color(240,36,36);
        [SerializeField] Color healTextColor = new Color(100,190,60);

        private void Awake() 
        {
            GetComponent<CombatTarget>().OnHealthChanged += Spawn; 
        }

        public void Spawn(float healthChangeValue, CombatTarget.HealthChangeType type)
        {
            if(type == CombatTarget.HealthChangeType.IgnoreType) return;

            healthChangeValue = Mathf.Round(healthChangeValue);
            var textObject = GameObject.Instantiate(damageTextPrefab,transform);
            var text = textObject.GetComponentInChildren<TextAnimatorPlayer>();
            
            if(CombatTarget.HealthChangeType.Damage == type)
            {
                textObject.GetComponentInChildren<TextMeshProUGUI>().color = damageTextColor;
            }
            else textObject.GetComponentInChildren<TextMeshProUGUI>().color = healTextColor;

            text.ShowText(openingTags + healthChangeValue.ToString() + closingTags);
        }
    }
}
