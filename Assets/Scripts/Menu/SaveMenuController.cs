using System.Collections;
using System.Collections.Generic;
using System.IO;
using PyMods;
using UnityEngine;
using UnityEngine.UI;

public class SaveMenuController : MonoBehaviour
{
    public static SaveMenuController instance;
    void Awake() => instance = this;

    public GameObject itemPrefab, infoPrefab;
    public GameObject newPrefab;
    public Transform parentTransform;
    public Transform canvas;

    // Start is called before the first frame update
    void Start()
    {
        List<Save> result = new List<Save>();
        string searchPath = Application.persistentDataPath + "/Saves/";
        if (!Directory.Exists(searchPath)) Directory.CreateDirectory(searchPath);
        foreach (string dir in Directory.GetDirectories(searchPath))
        {
            if (File.Exists(dir + "/save.json"))
            {
                Save s = new Save(dir);
                GameObject obj = Instantiate(itemPrefab, parentTransform);
                obj.GetComponent<SaveSelector>().setSave(s);
            }
        }

    }

    public void Create()
    {
        Instantiate(newPrefab, canvas);
    }

    public void ShowInfo(Save s)
    {
        Transform t = Instantiate(infoPrefab).transform;
        t.GetChild(0).GetChild(1).GetChild(1).GetComponent<Text>().text = "About: " + s.name;
        t.GetChild(0).GetChild(1).GetChild(2).GetComponent<Text>().text = "Mods: ";
        foreach (string m in s.mods)
        {
            if (m != s.mods[0]) t.GetChild(0).GetChild(1).GetChild(2).GetComponent<Text>().text += ", ";
            t.GetChild(0).GetChild(1).GetChild(2).GetComponent<Text>().text += m;
        }
    }
}
