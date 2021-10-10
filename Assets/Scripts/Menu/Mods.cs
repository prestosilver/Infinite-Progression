using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using UnityEngine.UI;
using System.Linq;
using PyMods;
using UnityEngine.SceneManagement;

/// <summary>
/// Example mod manager. This menu displays all mods and lets you enable/disable them.
/// </summary>
public class Mods : MonoBehaviour
{
    private ModManager modManager;

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

    void Start()
    {
        modItems = new Dictionary<Mod, ModItem>();

        modManager = ModManager.instance;

        if (modManager.GetModList().Count == 0)
            GitControler.download("github.com/prestosilver/IP-BaseMods");

        foreach (Mod mod in modManager.GetModList())
            OnModFound(mod);

        Application.runInBackground = true;
        StartCoroutine(UpdateMods());
    }

    public IEnumerator UpdateMods()
    {
        while (true)
        {
            yield return new WaitForSeconds(5);
            foreach (ModItem modItem in modItems.Values) Destroy(modItem.gameObject);
            modItems = new Dictionary<Mod, ModItem>();
            foreach (Mod mod in modManager.GetModList())
                OnModFound(mod);
        }
    }

    private void OnModFound(Mod mod)
    {
        ModItem modItem = Instantiate(modItemPrefab);
        modItem.Initialize(mod, menuContentPanel);
        modItem.SetToggleInteractable(!isLoaded);
        modItems.Add(mod, modItem);
    }

    private void OnModRemoved(Mod mod)
    {
        ModItem modItem;

        if (modItems.TryGetValue(mod, out modItem))
        {
            modItems.Remove(mod);
            Destroy(modItem.gameObject);
        }
    }

    private void SetTogglesInteractable(bool interactable)
    {
        foreach (ModItem item in modItems.Values)
        {
            item.SetToggleInteractable(interactable);
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

    private void Load()
    {
        //load mods
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

        SetTogglesInteractable(false);

        isLoaded = true;
    }

    private void Unload()
    {
        //unload all mods - this will unload their scenes and destroy all their instantiated objects as well
        foreach (Mod mod in modItems.Keys)
        {
            mod.Unload();
        }
        GameController.mods = new List<Mod>();

        SetTogglesInteractable(true);

        isLoaded = false;
    }

    private void OnModLoaded(Mod mod)
    {
        Debug.Log("Loaded Mod: " + mod.name);
    }

    private void OnModUnloaded(Mod mod)
    {
        Debug.Log("Unloaded Mod: " + mod.name);
    }

    public void Home()
    {
        Unload();
        SceneManager.LoadScene("MainMenu");
    }

    public void Play()
    {
        Load();
        NamePick.modPickSave.mods = new List<string>();
        foreach (Mod mod in modItems.Keys)
        {
            if (mod.isEnabled)
                NamePick.modPickSave.mods.Add(mod.name);
        }
        NamePick.modPickSave.Create();
        SceneManager.LoadScene("MainGame");
    }
}