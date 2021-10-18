using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AntiSelect : MonoBehaviour, IPointerDownHandler, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        Selection.instance.ResetSelection();
    }

    public void OnPointerDown(PointerEventData eventData) { }
}