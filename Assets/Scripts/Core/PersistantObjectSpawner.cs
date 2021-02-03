using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class PersistantObjectSpawner : MonoBehaviour
    {
        [SerializeField] GameObject persistandObjectPrefab;
        static bool isSpawned = false;

        private void Awake()
        {
            if(isSpawned) return;

            SpawnPersistandObjects();
            isSpawned = true;    
        }

        private void SpawnPersistandObjects()
        {
            GameObject persistandObject = Instantiate(persistandObjectPrefab);
            DontDestroyOnLoad(persistandObject);
        }
    }
}
