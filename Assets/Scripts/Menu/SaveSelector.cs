using System.Collections;
using System.Collections.Generic;
using PyMods;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SaveSelector : MonoBehaviour
{
    public Save save;
    public Text nameText;

    public void Load()
    {
        Saves.saveName = save.name;
        GameController.mods = ModManager.instance.LoadMods(save.mods);
        SceneManager.LoadScene("MainGame");
    }

    public void setSave(Save save)
    {
        this.save = save;
        nameText.text = save.name;
    }
}
