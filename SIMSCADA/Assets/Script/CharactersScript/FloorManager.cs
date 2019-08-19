using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FloorManager : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    public bool pointerOnFloor;

    public void OnPointerUp(PointerEventData eventData)
    {
        pointerOnFloor = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        pointerOnFloor = true;
    }
}
