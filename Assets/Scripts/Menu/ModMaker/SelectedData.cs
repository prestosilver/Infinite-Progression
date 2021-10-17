using System.Collections.Generic;
using UnityEngine;

public class SelectedData : MonoBehaviour
{
    public Dictionary<string, string> data = new Dictionary<string, string>();

    public void Set(string variable, string value)
    {
        if (data.ContainsKey(variable))
        {
            data[variable] = value;
            return;
        }
        data.Add(variable, value);
    }
}