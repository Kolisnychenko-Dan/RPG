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
        [SerializeField] bool returnToGurdPos;
        [SerializeField] PatrolPath patrolPath;
        [SerializeField] float waypointTolerance = 1f;

        GameObject[] characters;
        GameObject target;
        Mover mover;
        Vector3 guardPosition;
        Vector3 lastEnemyPosition;
        int currentPatrolWaypointNumber = 0;
        
        private void Start()
        {
            characters = GameObject.FindGameObjectsWithTag("Player");
            guardPosition = transform.position;
            mover = GetComponent<Mover>();
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
            mover.StartMoveAction(guardPosition);
        }

        private void LocateNewTarget()
        {
            target = null;
            foreach (var player in characters)
            {
                float distance = Vector3.Distance(transform.position,player.transform.position);
                if(distance < chaseDistance)
                {
                    if (target == null || InAttackRangeOfPlayer(distance))
                    {
                        target = player;
                        lastEnemyPosition = target.transform.position;
                        GetComponent<Atacker>().Atack(target.GetComponent<CombatTarget>());
                    }
                }
            }
            if(target == null && !returnToGurdPos) MoveToTheLastEnemyPosition();
        }

        private bool InAttackRangeOfPlayer(float distance)
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

