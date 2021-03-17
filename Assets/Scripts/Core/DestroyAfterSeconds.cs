using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.UI
{
    public class DestroyAfterSeconds : MonoBehaviour
    {
        [SerializeField] float destroyDuration = 2f;

        private void Start() 
        {
            StartCoroutine(DestroyText());    
        }
        private IEnumerator DestroyText()
        {
            yield return new WaitForSeconds(destroyDuration);
            Destroy(gameObject);
        }
    }
}

