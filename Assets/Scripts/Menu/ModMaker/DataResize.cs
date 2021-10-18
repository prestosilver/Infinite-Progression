using System.Collections.Generic;
using UnityEngine;

class DataResize : MonoBehaviour
{
    SelectedData data;

    void Start()
    {
        data = GetComponent<SelectedData>();
        Update();
    }

    public void Update()
    {
        if (data == null) return;
        if (data.data == null) data.data = new Dictionary<string, string>();
        if (!data.data.ContainsKey("X"))
        {
            data.data.Add("X", "0");
            data.data.Add("Y", "0");
            data.data.Add("Width", "30");
            data.data.Add("Height", "30");
        }
        GetComponent<RectTransform>().anchoredPosition = new Vector2(int.Parse(data.data["X"]), -int.Parse(data.data["Y"]));
        GetComponent<RectTransform>().sizeDelta = new Vector2(int.Parse(data.data["Width"]), int.Parse(data.data["Height"]));
    }
}