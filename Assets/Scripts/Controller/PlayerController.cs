using UnityEngine;
using RPG.Movement;
using RPG.Combat;
using System;
using UnityEngine.AI;
using UnityEngine.EventSystems;

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
        [SerializeField] bool autoAttack;
        CombatTarget combatTarget;
        Action<Vector3> pointChoosingMode = null;

        public bool AutoAttack { get => autoAttack; set => autoAttack = value; }
        public Action<Vector3> PointChoosingMode { get => pointChoosingMode; set => pointChoosingMode = value; }

        private void Awake()
        {
            combatTarget = GetComponent<CombatTarget>();
        }

        private void Update()
        {
            if(combatTarget.IsDead) return;
            if(InteractWithUI()) return;
            if(InteractWithAreaChosingMode()) return;
            if(InteractWithComponent()) return;
            if(InteractWithMovement(false)) return;

            SetCursor(CursorType.None);
        }

        public bool InteractWithMovement(bool ignoreClick)
        {
            Vector3 destination;
            if (RayCastNavMesh(out destination))
            {
                if (Input.GetMouseButtonDown(0) || ignoreClick)
                {
                    GetComponent<Mover>().StartMoveAction(destination);
                }
                SetCursor(CursorType.Movement);
                return true;
            }
            return false;
        }

        public bool InteractWithAreaChosingMode()
        {
            if(pointChoosingMode != null)
            {
                if (Input.GetMouseButtonDown(1)) pointChoosingMode = null;

                Vector3 pointClicked;
                if (RayCastNavMesh(out pointClicked))
                {
                    SetCursor(CursorType.Attack);

                    if (Input.GetMouseButtonDown(0))
                    {
                        pointChoosingMode.Invoke(pointClicked);
                        pointChoosingMode = null;
                    }
                }
                else SetCursor(CursorType.None);
                return true;
            }
            return false;
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
            Array.Sort<RaycastHit>(raycasts, (RaycastHit f, RaycastHit l) => 
                (int)(f.distance - l.distance));
            return raycasts;
        }
        
        private bool InteractWithUI()
        {
            if(EventSystem.current.IsPointerOverGameObject())
            {
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

