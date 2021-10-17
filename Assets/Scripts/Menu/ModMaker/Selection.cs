using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selection : MonoBehaviour
{
    public static Selection instance;
    void Awake() => instance = this;

    public int selectionKind;
    public SelectedData selectedData;

    private RectTransform selection;
    public GameObject selected;

    void Start()
    {
        selection = GetComponent<RectTransform>();
    }

    internal void ResetSelection()
    {
        GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
        GetComponent<RectTransform>().sizeDelta = new Vector3(0, 0, 0);
        selected = null;
        ToolBox.instance.UpdateFields();
    }

    public void SetSelection(RectTransform selection, GameObject go)
    {
        this.selected = go;
        this.selection = selection;
        GetComponent<RectTransform>().anchorMax = selection.anchorMax;
        GetComponent<RectTransform>().anchorMin = selection.anchorMin;
        GetComponent<RectTransform>().pivot = selection.pivot;
        GetComponent<RectTransform>().position = selection.position;
        GetComponent<RectTransform>().sizeDelta = selection.sizeDelta;
        ToolBox.instance.UpdateFields();
    }
}
