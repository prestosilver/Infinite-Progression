using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolBox : MonoBehaviour
{
    public static ToolBox instance;
    void Awake() => instance = this;

    private List<GameObject> Fields = new List<GameObject>();

    [SerializeField]
    private Transform fieldParent;
    [SerializeField]
    private Transform objectParent;

    [SerializeField]
    private GameObject fieldPrefab;
    [SerializeField]
    private Dropdown prefabSelect;
    [SerializeField]
    private List<GameObject> prefabs;

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
        AddField("Width", "30");
        AddField("Height", "30");
        Selection.instance.selected.GetComponent<DataResize>().Update();

        switch (Selection.instance.selectionKind)
        {
            case 1:
                AddField("onClick", "onClick");
                AddField("enable", "isEnabled");
                break;
            case 2:
                AddField("Variable", "progress");
                break;
            case 3:
                AddField("dynamic_text", "");
                AddField("static_text", "Text");
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

    public void Add()
    {
        GameObject go = Instantiate(prefabs[prefabSelect.value], objectParent);
        Selection.instance.selected = go;
        Selection.instance.selectedData = go.GetComponent<SelectedData>();
        Selection.instance.selectionKind = prefabs[prefabSelect.value].GetComponent<Selectable>().kind;
        UpdateFields();
    }

    public void Remove()
    {
        Destroy(Selection.instance.selected);
        Selection.instance.ResetSelection();
    }

}
