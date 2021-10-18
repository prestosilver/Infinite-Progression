using System.Collections.Generic;
using UnityEngine;

public class SelectedData : MonoBehaviour
{
    public Dictionary<string, string> data = new Dictionary<string, string>();

    public string Get(string variable)
    {
        try
        {
            return data[variable];
        }
        catch (KeyNotFoundException)
        {
            data.Add(variable, "");
            return data[variable];
        }
    }

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