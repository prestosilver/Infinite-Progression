using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Selectable : MonoBehaviour, IPointerDownHandler, IPointerClickHandler
{
    public int kind;

    public void OnPointerClick(PointerEventData eventData)
    {
        Selection.instance.selectionKind = kind;
        Selection.instance.selectedData = GetComponent<SelectedData>();
        Selection.instance.SetSelection(GetComponent<RectTransform>(), gameObject);
        Debug.Log("select");
    }

    public void OnPointerDown(PointerEventData eventData) { }
}
