using UnityEngine;
using UnityEngine.AI;
using RPG.Core;
using RPG.Saving;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction, ISaveable
    {   
        NavMeshAgent navMeshAgent;
        
        private void Awake() 
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
        }
        private void Start()
        {
            if(navMeshAgent.enabled) navMeshAgent.isStopped = true;
        }
        void Update()
        {
            UpdateAnimator();
        }

        public void StartMoveAction(Vector3 destination)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(destination);
        }

        public void Stop()
        {
            GetComponent<ActionScheduler>().StartAction(this);
            navMeshAgent.isStopped = true;
        }

        public void MoveTo(Vector3 destination)
        {
            navMeshAgent.isStopped = false;
            navMeshAgent.destination = destination;
        }

        public void Cancel()
        {
            navMeshAgent.isStopped = true;
        }

        void UpdateAnimator(){
            if(navMeshAgent != null)
            {
                Vector3 velocity = navMeshAgent.velocity;
                float speed = transform.InverseTransformDirection(velocity).z;
                GetComponent<Animator>().SetFloat("forwardSpeed", speed);
            }
        }

        public object CaptureState()
        {
            return new SerializableVector3(transform.position);
        }

        public void RestoreState(object state)
        {
            if(GetComponent<NavMeshAgent>().enabled)
            {
                GetComponent<NavMeshAgent>().enabled = false;
                transform.position = ((SerializableVector3)state).ToVector();
                GetComponent<NavMeshAgent>().enabled = true;
            }
            transform.position = ((SerializableVector3)state).ToVector();
        }
    }
}

