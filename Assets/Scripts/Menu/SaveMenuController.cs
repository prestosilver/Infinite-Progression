using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveMenuController : MonoBehaviour
{
    public GameObject itemPrefab;
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
}
