using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

namespace UniversalInventorySystem
{
    public class UIRaycaster : MonoBehaviour
    {
        GraphicRaycaster m_Raycaster;
        PointerEventData m_PointerEventData;
        EventSystem m_EventSystem;

        void Start()
        {
            m_Raycaster = GetComponent<GraphicRaycaster>();
            m_EventSystem = GetComponent<EventSystem>();
        }

        public List<RaycastResult> RaycastOnMousePosition()
        {
            m_PointerEventData = new PointerEventData(m_EventSystem);
            m_PointerEventData.position = Input.mousePosition;

            var results = new List<RaycastResult>();
            m_Raycaster.Raycast(m_PointerEventData, results);

            return results;
        }
    }
}
