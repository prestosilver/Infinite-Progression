using System.IO;
using UnityEngine;

public class Save
{
    public string infoFile;
    public string name;

    public Save(string path)
    {
        infoFile = path + "/save.json";
        Save s = JsonUtility.FromJson<Save>(File.ReadAllText(infoFile));
        name = s.name;
    }
}