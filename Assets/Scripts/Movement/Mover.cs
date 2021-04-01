using UnityEngine;
using UnityEngine.AI;
using RPG.Core;
using RPG.Saving;
using System;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction, ISaveable
    {   
        [SerializeField] float destinationPointTolerance = 1f;
        NavMeshAgent navMeshAgent;
        Vector3? currentDestination;
        public event Action<Vector3> OnDestinationReached = null;
        
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
            if(OnDestinationReached != null && DestinationReached())
            {
                OnDestinationReached.Invoke((Vector3)currentDestination);
                OnDestinationReached = null;
            }
        }

        public void StartMoveAction(Vector3 destination)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(destination);
            currentDestination = destination;
        }

        public void Stop()
        {
            GetComponent<ActionScheduler>().StartAction(this);
            navMeshAgent.isStopped = true;
            currentDestination = null;
        }

        public void MoveTo(Vector3 destination)
        {
            navMeshAgent.isStopped = false;
            navMeshAgent.destination = destination;
        }

        public bool DestinationReached()
        {
            float distance = Vector3.Distance(transform.position, (Vector3)currentDestination);
            return distance < destinationPointTolerance;
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

