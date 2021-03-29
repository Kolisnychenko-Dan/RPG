using UnityEngine;
using RPG.Movement;
using RPG.Combat;
using System;
using UnityEngine.AI;

namespace RPG.Controller
{
    public class PlayerController : MonoBehaviour
    {
        [Serializable]
        struct CursorMapping
        {
            public CursorType type;
            public Texture2D texture;
            public Vector2 hotspot;
        }

        [SerializeField] CursorMapping[] cursorMappings = null;
        [SerializeField] float maxNavMeshProjectionDisctance = 1f;

        private void Update()
        {
            if(InteractWithUI()) return;
            if(InteractWithComponent()) return;
            if(InteractWithMovement()) return;

            SetCursor(CursorType.None);
        }

        public bool InteractWithComponent()
        {
            RaycastHit[] hits = GetSortedRaycast();
            foreach (var hit in hits)
            {
                var components = hit.transform.GetComponents<IRayCastable>();
                foreach(IRayCastable component in components)
                {
                    if(Input.GetKey(KeyCode.LeftShift) && components.Length > 1)
                        continue;

                    SetCursor(component.GetHoverCursor());
                    if(component.HandleRaycast(this)) 
                        return true;
                }
            }
            return false;
        }

        private RaycastHit[] GetSortedRaycast()
        {
            var raycasts = Physics.RaycastAll(GetMouseRay());
            Array.Sort<RaycastHit>(raycasts, (RaycastHit f, RaycastHit l) => {
                return (int)(f.distance - l.distance);
            });
            return raycasts;
        }
        
        private bool InteractWithUI()
        {

            SetCursor(CursorType.Movement);
            return false;
        }
        private bool InteractWithMovement()
        {
            Vector3 destination;
            if (RayCastNavMesh(out destination))
            {
                if (Input.GetMouseButton(0))
                {
                    GetComponent<Mover>().StartMoveAction(destination);
                }
                SetCursor(CursorType.Movement);
                return true;
            }
            return false;
        }

        private bool RayCastNavMesh(out Vector3 target)
        {
            RaycastHit hit;
            target = new Vector3();

            if(!Physics.Raycast(GetMouseRay(), out hit)) 
                return false;
            
            NavMeshHit navMeshHit;
            bool hasCastToNavMesh = NavMesh.SamplePosition(
                hit.point, out navMeshHit, maxNavMeshProjectionDisctance, NavMesh.AllAreas);
            
            if(!hasCastToNavMesh)
                return false;

            target = navMeshHit.position;
            return true;
        }

        private void SetCursor(CursorType type)
        {
            CursorMapping mapping = GetCursorMapping(type);
            Cursor.SetCursor(mapping.texture, mapping.hotspot, CursorMode.Auto);
        }

        private CursorMapping GetCursorMapping(CursorType type)
        {
            foreach(var v in cursorMappings)
            {
                if(v.type == type) return v;
            }
            return cursorMappings[0];
        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}

