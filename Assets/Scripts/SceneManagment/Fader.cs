using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class Fader : MonoBehaviour
    {
        CanvasGroup canvasGroup;
        Coroutine currentActiveCoroutine = null;

        void Start()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public void FadeOutImmediate()
        {
            canvasGroup.alpha = 1;
        }

        public IEnumerator FadeOut(float time)
        {
            if(currentActiveCoroutine != null) StopCoroutine (currentActiveCoroutine);

            yield return currentActiveCoroutine = StartCoroutine(FadeOutCoroutine(time));
        }

        private IEnumerator FadeOutCoroutine(float time)
        {
            while(canvasGroup.alpha < 1)
            {
                canvasGroup.alpha += Time.deltaTime / time;
                yield return null;
            }
        }

        public IEnumerator FadeIn(float time)
        {
            if(currentActiveCoroutine != null) StopCoroutine (currentActiveCoroutine);
            
            yield return currentActiveCoroutine = StartCoroutine(FadeInCoroutine(time));
        }

        private IEnumerator FadeInCoroutine(float time)
        {
            while(canvasGroup.alpha > 0)
            {
                canvasGroup.alpha -= Time.deltaTime / time;
                yield return null;
            }
        }
    }    
}
