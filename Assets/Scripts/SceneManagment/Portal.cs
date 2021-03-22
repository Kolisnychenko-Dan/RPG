using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Controller;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        [SerializeField] int sceneToLoad = -1;
        [SerializeField] int indexOfPortal = 0;
        [SerializeField] Transform spawnPoint;
        [SerializeField] float fadeInDuration = 0.5f;
        [SerializeField] float fadeWaitTime = 0.3f;
        [SerializeField] float fadeOutDuration = 1f;

        public int IndexOfPortal { get; }

        private void Start()
        {
            spawnPoint = GetComponentInChildren<Transform>();
        }
        private void OnTriggerEnter(Collider other)
        {
            if(other.name == "Player")
            {
                StartCoroutine(Transition());
            }
        }

        private IEnumerator Transition()
        {
            GameObject.DontDestroyOnLoad(gameObject);

            Fader fader = FindObjectOfType<Fader>();

            PlayerController pc = FindObjectOfType<PlayerController>();
            pc.enabled = false;
            
            yield return fader.FadeOut(fadeOutDuration);

            SavingWrapper wrapper = FindObjectOfType<SavingWrapper>();
            wrapper.Save();

            yield return SceneManager.LoadSceneAsync(sceneToLoad);
            
            wrapper.Load();

            Portal otherPortal = GetOtherPortal();
            otherPortal.MovePlayerToSpawnpoint();

            wrapper.Save();

            pc = FindObjectOfType<PlayerController>();
            pc.enabled = false;

            yield return new WaitForSeconds(fadeWaitTime);
            yield return fader.FadeIn(fadeInDuration);

            pc.enabled = true;

            GameObject.Destroy(gameObject);
        }

        private Portal GetOtherPortal()
        {
            foreach(var portal in FindObjectsOfType<Portal>())
            {
                if(portal != this && portal.IndexOfPortal == indexOfPortal)
                {
                    return portal;
                }
            }
            return null;
        }

        private void MovePlayerToSpawnpoint()
        {
            Transform player = GameObject.Find("Player").transform;

            player.GetComponent<NavMeshAgent>().enabled = false;
            player.position = spawnPoint.position;
            player.rotation = spawnPoint.rotation;
            player.GetComponent<NavMeshAgent>().enabled = true; 
        }
    }
}
