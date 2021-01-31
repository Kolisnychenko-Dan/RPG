using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    [SerializeField] int sceneToLoad = -1;
    [SerializeField] int indexOfPortal = 0;
//    [SerializeField] bool isOneWayTicket = false;
    [SerializeField] Transform spawnPoint;
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
        yield return SceneManager.LoadSceneAsync(sceneToLoad);

        Portal otherPortal = GetOtherPortal();
        otherPortal.MovePlayerToSpawnpoint();
    
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
        Transform player = GameObject.FindWithTag("Player").transform;
        player.position = spawnPoint.position;
        player.rotation = spawnPoint.rotation;        
    }
}
