using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolBox : MonoBehaviour
{
    public static ToolBox instance;
    void Awake() => instance = this;

    private List<GameObject> Fields = new List<GameObject>();

    [SerializeField]
    private Transform fieldParent;

    [SerializeField]
    private GameObject fieldPrefab;

    public void Start()
    {
        UpdateFields();
    }

    public void UpdateFields()
    {
        Fields.ForEach((o) => Destroy(o));
        Fields.Clear();

        if (Selection.instance.selected == null) return;

        AddField("X", "0");
        AddField("Y", "0");
        AddField("Width", "0");
        AddField("Height", "0");

        switch (Selection.instance.selectionKind)
        {
            case 1:
                AddField("onClick", "onClick");
                AddField("enable", "isEnabled");
                break;
            default:
                break;
        }
    }
    private void AddField(string name, string def)
    {
        GameObject obj = Instantiate(fieldPrefab, fieldParent);
        string value = def;
        if (Selection.instance.selectedData.data.ContainsKey(name))
            value = Selection.instance.selectedData.data[name];
        else
            Selection.instance.selectedData.data[name] = value;
        obj.GetComponent<FieldEdit>().Setup(name, value);
        Fields.Add(obj);
    }
}
