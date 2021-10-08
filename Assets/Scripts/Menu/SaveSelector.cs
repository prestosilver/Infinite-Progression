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
    public GameObject confirmPrefab;

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

    public void deleteSave()
    {
        Saves.saveName = save.name;
        GameObject go = Instantiate(confirmPrefab);
        GameObject confirm = GameObject.FindGameObjectsWithTag("ConfirmButton")[0];
        confirm.GetComponent<Button>().onClick.AddListener(() =>
        {
            Saves.Delete();
            Destroy(go);
            SceneManager.LoadScene("MainMenu");
        });
    }
}
