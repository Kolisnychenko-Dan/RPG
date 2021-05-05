using System.Collections;
using Febucci.UI;
using UnityEngine;
using RPG.Combat;
using TMPro;
using UnityEngine.UI;

namespace RPG.UI
{
    [RequireComponent(typeof(CombatTarget))]
    public class HealthTextSpawner : MonoBehaviour
    {
        [SerializeField] GameObject critTextPrefab;
        [SerializeField] GameObject TextPrefab;
        [SerializeField] string openingTags = "{size}<shake><fade>";
        [SerializeField] string closingTags = "{/size}</shake></fade>";
        [SerializeField] Color critDamageTextColor = new Color(240,36,36);
        [SerializeField] Color damageTextColor = new Color(200,200,200);
        [SerializeField] Color healTextColor = new Color(100,190,60);

        private void Awake() 
        {
            GetComponent<CombatTarget>().OnHealthChanged += Spawn; 
        }

        public void Spawn(float healthChangeValue, CombatTarget.HealthChangeType type)
        {
            healthChangeValue = Mathf.Round(healthChangeValue);

            switch (type)
            {
                case CombatTarget.HealthChangeType.CritDamage:
                {
                    var textObject = GameObject.Instantiate(critTextPrefab,transform);
                    var text = textObject.GetComponentInChildren<TextAnimatorPlayer>();
                    
                    textObject.GetComponentInChildren<TextMeshProUGUI>().color = critDamageTextColor;
                    
                    text.ShowText(openingTags + healthChangeValue.ToString() + closingTags);
                }
                break;
                case CombatTarget.HealthChangeType.Damage:
                {
                    var textObject = GameObject.Instantiate(TextPrefab,transform);
                    var text = textObject.GetComponentInChildren<Text>();
                    
                    text.text = healthChangeValue.ToString();
                    text.color = damageTextColor;
                }
                break;
                case CombatTarget.HealthChangeType.Heal:
                {
                    var textObject = GameObject.Instantiate(critTextPrefab,transform);
                    var text = textObject.GetComponentInChildren<TextAnimatorPlayer>();
                    
                    textObject.GetComponentInChildren<TextMeshProUGUI>().color = healTextColor;
                    
                    text.ShowText(openingTags + healthChangeValue.ToString() + closingTags);
                }
                break;
            }       
        }
    }
}
