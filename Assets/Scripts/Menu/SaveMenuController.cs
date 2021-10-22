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
        // setup the mods text
        string modsText = "Mods: ";

        // no mods
        if (s.mods.Count == 0) modsText += "None";

        // add mods to list
        foreach (string m in s.mods)
        {
            if (m != s.mods[0]) modsText += ", ";
            modsText += m;
        }

        ModalWindowSpawner.instance.Spawn(new ModalWindow
        {
            isScroll = true,
            canClose = true,
            title = "About: " + s.name,
            content = new string[1] {
                modsText,
            },
            buttons = new List<ModalWindowButton>
            {
                new ModalWindowButton{
                    onClick = ()=>{},
                    text = "OK",
                    destroys = true
                }
            }
        });
    }

    public void Back()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
