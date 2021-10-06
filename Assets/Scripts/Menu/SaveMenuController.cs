using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveMenuController : MonoBehaviour
{
    public GameObject itemPrefab;
    public Transform parentTransform;

    // Start is called before the first frame update
    void Start()
    {
        List<Save> result = new List<Save>();
        string SearchPath = Application.persistentDataPath + "/Saves/";
        foreach (string dir in Directory.GetDirectories(SearchPath))
        {
            if (File.Exists(dir + "/save.json"))
            {
                Save s = new Save(dir);
                GameObject obj = Instantiate(itemPrefab, parentTransform);
                obj.GetComponent<SaveSelector>().setSave(s);
            }
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}
