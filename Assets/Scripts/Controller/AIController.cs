using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Combat;
using RPG.Movement;
using System;

namespace RPG.Controller
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] bool startChaseAtChaseDistance;
        [SerializeField] float chaseDistance = 5f;
        [SerializeField] float suspicionTime = 3f;
        [SerializeField] float agroTime = 5f;
        [SerializeField] float callAlliesRadius = 5f;
        [SerializeField] bool returnToGurdPos;
        [SerializeField] PatrolPath patrolPath;
        [SerializeField] float waypointTolerance = 1f;

        GameObject[] characters;
        GameObject target;
        Mover mover;
        float suspicionTimeElapsed = Mathf.Infinity;
        float agroTimeElapsed = Mathf.Infinity;
        Vector3 guardPosition;
        Vector3 lastEnemyPosition;
        int currentPatrolWaypointNumber = 0;
        
        private void Awake() 
        {
            characters = GameObject.FindGameObjectsWithTag("Player");
            mover = GetComponent<Mover>();
            GetComponent<CombatTarget>().OnDamageTaken += Agro;
        }
        private void Start()
        {
            guardPosition = transform.position;
            lastEnemyPosition = transform.position;
        }
        
        void Update()
        {
            if (GetComponent<CombatTarget>().IsDead) return;
            if(target == null)
            {
                if (returnToGurdPos) ReturnToGuarding();
                if (patrolPath != null) Patrol();
            }
            if (startChaseAtChaseDistance) LocateNewTarget();

            UpdateTimers();
        }

        public void Agro(float f)
        {
            agroTimeElapsed = 0;
        }

        void UpdateTimers()
        {
            suspicionTimeElapsed += Time.deltaTime;
            agroTimeElapsed += Time.deltaTime;
        }

        private void Patrol()
        {
            if(currentPatrolWaypointNumber == patrolPath.CountWaypoints())
            {
                currentPatrolWaypointNumber = 0;
            }

            if(AtWaypoint())
            {
                mover.StartMoveAction(patrolPath.GetNextWaypoint(currentPatrolWaypointNumber));
                currentPatrolWaypointNumber ++;
            }
            else mover.StartMoveAction(patrolPath.GetWaypoint(currentPatrolWaypointNumber));
        }

        private bool AtWaypoint()
        {
            float distance = Vector3.Distance(transform.position, patrolPath.GetWaypoint(currentPatrolWaypointNumber)); 
            return distance < waypointTolerance;
        }

        private void ReturnToGuarding()
        {
            mover.Stop();
            if(suspicionTimeElapsed > suspicionTime)
                mover.StartMoveAction(guardPosition);
        }

        private void LocateNewTarget()
        {
            target = null;
            float currentMinChaseDistance = Mathf.Infinity;

            foreach (var playersCharacter in characters)
            {
                float distance = Vector3.Distance(transform.position,playersCharacter.transform.position);
                if((distance < chaseDistance && distance < currentMinChaseDistance) || agroTimeElapsed < agroTime)
                {
                    if (target == null || IsInAttackRange(distance))
                    {
                        currentMinChaseDistance = distance;

                        target = playersCharacter;
                        lastEnemyPosition = target.transform.position;
                        
                        AttackBehavior();
                    }
                }
            }
            if(target == null && !returnToGurdPos) MoveToTheLastEnemyPosition();
        }

        private void AttackBehavior()
        {
            suspicionTimeElapsed = 0;

            GetComponent<Atacker>().Atack(target.GetComponent<CombatTarget>());
            AgroAllies();
        }

        private void AgroAllies()
        {
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, callAlliesRadius, Vector3.up,0);

            foreach(var hit in hits)
            {
                hit.transform.GetComponent<AIController>()?.Agro(0f);
            }
        }

        private bool IsInAttackRange(float distance)
        {
            return Vector3.Distance(transform.position, target.transform.position) < distance;
        }

        private void MoveToTheLastEnemyPosition()
        {
            mover.StartMoveAction(lastEnemyPosition);
        }

        // Gizmos
        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position,chaseDistance);
        }
    }
}

