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
        [SerializeField] float reAgroTime = 10f;
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
        bool isAgrevated = false;
        Vector3 guardPosition;
        Vector3 lastEnemyPosition;
        int currentPatrolWaypointNumber = 0;

        private void Awake() 
        {
            characters = Array.FindAll<GameObject>(GameObject.FindGameObjectsWithTag("Player"), obj => {
                if (obj.GetComponent<CombatTarget>() != null) return gameObject;
                else return false;
            });
            
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
            if(!combatTarget.IsStunned)
            {
                if (target == null)
                {
                    if (returnToGurdPos) ReturnToGuarding();
                    if (patrolPath != null) Patrol();
                }
                if (startChaseAtChaseDistance) LocateNewTarget();
            }
            UpdateTimers();
        }

        public void Agro(float damage, CombatTarget.HealthChangeType type)
        {
            if(CombatTarget.HealthChangeType.Heal == type || CombatTarget.HealthChangeType.IgnoreType == type) return;
            agroTimeElapsed = 0;
        }

        public void Agrevate() => isAgrevated = true;

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
                if((distance < chaseDistance || agroTimeElapsed < agroTime || isAgrevated) && distance < currentMinChaseDistance)
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

            GetComponent<Atacker>().Attack(target.GetComponent<CombatTarget>());
            
            if(!isAgrevated) AgroAllies();
            isAgrevated = false;
        }

        private void AgroAllies()
        {
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, callAlliesRadius, Vector3.up,0);

            foreach(var hit in hits)
            {
                if(hit.transform != transform) hit.transform.GetComponent<AIController>()?.Agrevate();
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

