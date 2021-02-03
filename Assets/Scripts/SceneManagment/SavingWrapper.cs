using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;

namespace RPG.SceneManagement
{
    public class SavingWrapper : MonoBehaviour
    {
        const string defaultSaveFile = "save";
        SavingSystem savingSystem;
        [SerializeField] float fadeInDuration = 1f;

        private void Start()
        {
            savingSystem = GetComponent<SavingSystem>();
        }
        /*IEnumerator Start()
        {
            Fader fader = FindObjectOfType<Fader>();
            fader.FadeOutImmediate();

            savingSystem = GetComponent<SavingSystem>();
            yield return savingSystem.LoadLastScene(defaultSaveFile);

            yield return fader.FadeIn(fadeInDuration);
        }*/

        private void Update()
        {
#if UNITY_EDITOR
            if(Input.GetKeyDown(KeyCode.L))
            {
                Load();
            }
            if(Input.GetKeyDown(KeyCode.S))
            {
                Save();
            }
#endif
            if(Input.GetKeyDown(KeyCode.L) && Input.GetKeyDown(KeyCode.LeftControl))
            {
                Load();
            }
            if(Input.GetKeyDown(KeyCode.S) && Input.GetKeyDown(KeyCode.LeftControl))
            {
                Save();
            }
        }

        public void Save()
        {
            savingSystem.Save(defaultSaveFile);
        }

        public void Load()
        {
            savingSystem.Load(defaultSaveFile);
        }
    }
}
