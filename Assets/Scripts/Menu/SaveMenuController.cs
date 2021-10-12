using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SaveMenuController : MonoBehaviour
{
    // singleton instance
    public static SaveMenuController instance;
    void Awake() => instance = this;

    /// <summary>
    /// prefab for a save in the list
    /// </summary>
    public GameObject itemPrefab;

    /// <summary>
    /// the save info prefab
    /// </summary>
    public GameObject infoPrefab;

    /// <summary>
    /// the create new save prefab
    /// </summary>
    public GameObject newPrefab;

    /// <summary>
    /// the parent for the list prefabs
    /// </summary>
    public Transform parentTransform;

    /// <summary>
    /// the canvas to spawn popups on
    /// </summary>
    public Transform canvas;

    /// <summary>
    /// setup list
    /// </summary>
    void Start()
    {
        // init the list of the saves
        List<Save> result = new List<Save>();

        // setup the save search path
        string searchPath = Path.Combine(Application.persistentDataPath, "Saves");

        // create save path
        if (!Directory.Exists(searchPath)) Directory.CreateDirectory(searchPath);

        // create save list
        foreach (string dir in Directory.GetDirectories(searchPath))
        {
            // add a valid save
            if (File.Exists(dir + "/save.json"))
            {
                Save s = new Save(dir);
                GameObject obj = Instantiate(itemPrefab, parentTransform);
                obj.GetComponent<SaveSelector>().setSave(s);
            }
        }
    }

    /// <summary>
    /// creates a save
    /// </summary>
    public void Create()
    {
        Instantiate(newPrefab, canvas);
    }

    /// <summary>
    /// shows the save info
    /// </summary>
    /// <param name="s">the save file</param>
    public void ShowInfo(Save s)
    {
        // create the info object
        Transform t = Instantiate(infoPrefab).transform;

        // setup the info text
        t.GetChild(0).GetChild(1).GetChild(1).GetComponent<Text>().text = "About: " + s.name;
        t.GetChild(0).GetChild(1).GetChild(2).GetComponent<Text>().text = "Mods: ";

        // no mods
        if (s.mods.Count == 0) t.GetChild(0).GetChild(1).GetChild(2).GetComponent<Text>().text += "None";

        // add mods to list
        foreach (string m in s.mods)
        {
            if (m != s.mods[0]) t.GetChild(0).GetChild(1).GetChild(2).GetComponent<Text>().text += ", ";
            t.GetChild(0).GetChild(1).GetChild(2).GetComponent<Text>().text += m;
        }
    }

    public void Back()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
