using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Save
{
    private string infoFile;
    public string name;
    public List<string> mods;

    public Save() { }
    public Save(string path)
    {
        infoFile = path + "/save.json";
        Save s = JsonUtility.FromJson<Save>(File.ReadAllText(infoFile));
        name = s.name;
        mods = s.mods;
    }

    public void Create()
    {
        infoFile = Application.persistentDataPath + $"/Saves/{name}/save.json";
        var path = Application.persistentDataPath + $"/Saves/{name}";
        if (!Directory.Exists(path)) Directory.CreateDirectory(path);
        File.WriteAllText(infoFile, JsonUtility.ToJson(this));
        mods = new List<string>();
    }
}