using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using UnityEngine.UI;
using System.Linq;
using PyMods;
using UnityEngine.SceneManagement;

public class Mods : MonoBehaviour
{
    /// <summary>
    /// the mod manager instance
    /// </summary>
    private ModManager modManager;

    /// <summary>
    /// a dictionary of mods and items representing them
    /// </summary>
    private Dictionary<Mod, ModItem> modItems;

    /// <summary>
    /// The content panel where the menu items will be parented
    /// </summary>
    public Transform menuContentPanel;

    /// <summary>
    /// The prefab for the mod menu item
    /// </summary>
    public ModItem modItemPrefab;

    /// <summary>
    /// Are the enabled mods loaded?
    /// </summary>
    private bool isLoaded;

    /// <summary>
    /// saetup mod list
    /// </summary>
    void Start()
    {
        // init items
        modItems = new Dictionary<Mod, ModItem>();

        // get mod manager instance
        modManager = ModManager.instance;

        // get base mods if not there
        if (modManager.GetModList().Count == 0)
            GitControler.download("github.com/prestosilver/IP-BaseMods");

        // get mod list
        foreach (Mod mod in modManager.GetModList())
            OnModFound(mod);

        Application.runInBackground = true;

        // update mod list
        // StartCoroutine(UpdateMods());
    }

    // update mod list in background
    public IEnumerator UpdateMods()
    {
        // update mods every second
        while (true)
        {
            yield return new WaitForSeconds(1);

            // clear mods
            foreach (ModItem modItem in modItems.Values) Destroy(modItem.gameObject);
            modItems = new Dictionary<Mod, ModItem>();

            // add mods
            foreach (Mod mod in modManager.GetModList())
                OnModFound(mod);
        }
    }

    /// <summary>
    /// adds a mod to the list
    /// </summary>
    /// <param name="mod"></param>
    private void OnModFound(Mod mod)
    {
        ModItem modItem = Instantiate(modItemPrefab);
        modItem.Initialize(mod, menuContentPanel);
        modItem.SetToggleInteractable(!isLoaded);
        modItems.Add(mod, modItem);
    }

    /// <summary>
    /// remove a mod from list
    /// </summary>
    /// <param name="mod"></param>
    private void OnModRemoved(Mod mod)
    {
        ModItem modItem;

        if (modItems.TryGetValue(mod, out modItem))
        {
            modItems.Remove(mod);
            Destroy(modItem.gameObject);
        }
    }

    /// <summary>
    /// Toggle load or unload all mods.
    /// </summary>
    public void LoadButton()
    {
        if (isLoaded)
        {
            Unload();
        }
        else
        {
            Load();
        }
    }

    /// <summary>
    /// load mods
    /// </summary>
    private void Load()
    {
        // init modlist
        GameController.mods = new List<Mod>();
        foreach (Mod mod in modItems.Keys)
        {
            if (mod.isEnabled)
            {
                mod.Load();
                GameController.mods.Add(mod);
                GameController.mods.AddRange(modManager.LoadMods(mod.requires, 1));
            }
        }

        isLoaded = true;
    }

    /// <summary>
    /// unload loaded mods
    /// </summary>
    private void Unload()
    {
        //unload all mods - this will unload their scenes and destroy all their instantiated objects as well
        foreach (Mod mod in modItems.Keys)
        {
            mod.Unload();
        }
        GameController.mods = new List<Mod>();

        isLoaded = false;
    }

    /// <summary>
    /// cancel button
    /// </summary>
    public void Home()
    {
        Unload();
        SceneManager.LoadScene("MainMenu");
    }

    /// <summary>
    /// load the selected mods
    /// </summary>
    public void Play()
    {
        // load them
        Load();

        // init mods in save
        NamePick.modPickSave.mods = new List<string>();

        // add mods to the save
        foreach (Mod mod in modItems.Keys)
        {
            if (mod.isEnabled)
                NamePick.modPickSave.mods.Add(mod.name);
        }
        // create mod save
        NamePick.modPickSave.Create();

        // stop run in bg bc done with mods
        Application.runInBackground = false;

        // play game
        SceneManager.LoadScene("MainGame");
    }
}