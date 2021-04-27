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
        CombatTarget combatTarget;
        float suspicionTimeElapsed = Mathf.Infinity;
        float agroTimeElapsed = Mathf.Infinity;
        Vector3 guardPosition;
        Vector3 lastEnemyPosition;
        int currentPatrolWaypointNumber = 0;
        
        private void Awake() 
        {
            characters = GameObject.FindGameObjectsWithTag("Player");
            mover = GetComponent<Mover>();
            combatTarget = GetComponent<CombatTarget>();
            combatTarget.OnHealthChanged += Agro;
        }
        private void Start()
        {
            guardPosition = transform.position;
            lastEnemyPosition = transform.position;
        }
        
        void Update()
        {
            if (combatTarget.IsDead) return;
            if(target == null)
            {
                if (returnToGurdPos) ReturnToGuarding();
                if (patrolPath != null) Patrol();
            }
            if (startChaseAtChaseDistance) LocateNewTarget();

            UpdateTimers();
        }

        public void Agro(float damage, CombatTarget.HealthChangeType type)
        {
            if(CombatTarget.HealthChangeType.Heal == type || CombatTarget.HealthChangeType.IgnoreType == type) return;
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
                    currentMinChaseDistance = distance;

                    target = playersCharacter;
                    lastEnemyPosition = target.transform.position;
                    
                    AttackBehavior();
                }
            }

            if(target == null && !returnToGurdPos) MoveToTheLastEnemyPosition();
        }

        private void AttackBehavior(GameObject explicitTarget = null)
        {
            if(explicitTarget == null)
            {
                suspicionTimeElapsed = 0;

                GetComponent<Atacker>().Attack(target.GetComponent<CombatTarget>());
                AgroAllies();
            }
            else
            {
                suspicionTimeElapsed = 0;
                target = explicitTarget;
                lastEnemyPosition = target.transform.position;
                GetComponent<Atacker>().Attack(explicitTarget.GetComponent<CombatTarget>());
            }
        }

        private void AgroAllies()
        {
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, callAlliesRadius, Vector3.up,0);

            foreach(var hit in hits)
            {
                if(hit.transform != transform) hit.transform.GetComponent<AIController>()?.AttackBehavior(target);
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

