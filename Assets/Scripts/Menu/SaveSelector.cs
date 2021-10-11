using System.Collections;
using System.Collections.Generic;
using PyMods;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SaveSelector : MonoBehaviour
{
    /// <summary>
    /// the save file this represents
    /// </summary>
    public Save save;

    /// <summary>
    /// the text displaying the name
    /// </summary>
    public Text nameText;

    /// <summary>
    /// the delete confirm prefab
    /// </summary>
    public GameObject confirmPrefab;

    /// <summary>
    /// loads the save
    /// </summary>
    public void Load()
    {
        // setup the save name
        Saves.saveName = save.name;

        // load the mods from the save
        GameController.mods = ModManager.instance.LoadMods(save.mods);

        // load the game
        SceneManager.LoadScene("MainGame");
    }

    /// <summary>
    /// setup the save name
    /// </summary>
    /// <param name="save">the name</param>
    public void setSave(Save save)
    {
        this.save = save;
        nameText.text = save.name;
    }

    /// <summary>
    /// confirm delete save
    /// </summary>
    public void deleteSave()
    {
        // the name of the save
        Saves.saveName = save.name;

        // create confirm dialog
        GameObject go = Instantiate(confirmPrefab);

        // setup delete action
        GameObject confirm = GameObject.FindGameObjectsWithTag("ConfirmButton")[0];
        confirm.GetComponent<Button>().onClick.AddListener(() =>
        {
            Saves.Delete();
            Destroy(go);
            SceneManager.LoadScene("MainMenu");
        });
    }

    /// <summary>
    /// shows the save info
    /// </summary>
    public void showInfo()
    {
        SaveMenuController.instance.ShowInfo(save);
    }
}
