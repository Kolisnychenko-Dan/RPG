using UnityEngine;

namespace RPG.Core
{
    public class FollowCamera : MonoBehaviour
    {
        GameObject target;

        private void Start()
        {
            target = GameObject.Find("Player");
        }
        void LateUpdate()
        {
            if(target != null) transform.position = target.transform.position;
        }
    }
}

