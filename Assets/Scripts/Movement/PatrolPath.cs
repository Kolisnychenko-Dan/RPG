using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Movement
{
    public class PatrolPath : MonoBehaviour
    {
        const float waypointGizmoSize = 0.2f;
        private void OnDrawGizmos()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawSphere(GetWaypoint(i), waypointGizmoSize);
                Gizmos.DrawLine(GetWaypoint(i), GetNextWaypoint(i));
            }
        }

        public int CountWaypoints()
        {
            return transform.childCount;
        }

        public Vector3 GetNextWaypoint(int i)
        {
            if (i + 1 == transform.childCount)
            {
                return GetWaypoint(0);
            }
            else return GetWaypoint(i + 1);
        }

        public Vector3 GetWaypoint(int i)
        {
            return transform.GetChild(i).position;
        }
    }
}
