using UnityEngine;
using UnityEngine.UI;
using PyMods;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class ModItem : MonoBehaviour
{
    /// <summary>
    /// the prefab that shows the error message
    /// </summary>
    public GameObject aboutPrefab;

    /// <summary>
    /// the name of the mod
    /// </summary>
    public Text modName;

    /// <summary>
    /// the mod description
    /// </summary>
    public Text modType;

    /// <summary>
    /// the mod enabled toggle
    /// </summary>
    public Toggle toggle;

    /// <summary>
    /// the mod this represents
    /// </summary>
    public Mod mod;

    /// <summary>
    /// Initialze this ModItem with a Mod and ModMenu.
    /// </summary>
    /// <param name="mod">the mod</param>
    /// <param name="modMenu">the mod menu object</param>
	public void Initialize(Mod mod, Transform menuContentPanel)
    {
        this.mod = mod;

        transform.SetParent(menuContentPanel);

        modName.text = mod.name;
        modType.text = mod.description;

        toggle.isOn = mod.isEnabled;
        toggle.onValueChanged.AddListener(value => Toggle(value));
        transform.localScale = new Vector3(1, 1, 1);
    }

    /// <summary>
    /// Toggle whether the mod should be loaded
    /// </summary>
    /// <param name="isEnabled">sets the mod enabled</param>
    public void Toggle(bool isEnabled)
    {
        mod.isEnabled = isEnabled;
    }

    /// <summary>
    /// Enable or disable this ModItem's toggle.
    /// </summary>
    /// <param name="interactable">the value</param>
    public void SetToggleInteractable(bool interactable)
    {
        toggle.interactable = interactable;
    }

    /// <summary>
    /// removes a mod
    /// </summary>
    public void Remove()
    {

        ModalWindowSpawner.instance.Spawn(new ModalWindow
        {
            isScroll = false,
            canClose = true,
            title = "Are You Sure",
            content = new string[1] {
                $"This will remove the mod '{mod.name}'"
            },
            buttons = new List<ModalWindowButton>
            {
                new ModalWindowButton{
                    onClick = () =>{
                        string error = mod.Remove();
                        if (error != "")
                        {
                            spawnError(error);
                        }
                    },
                    text = "Confirm",
                    destroys = true
                },
                new ModalWindowButton
                {
                    onClick = () => { },
                    text = "Cancel",
                    destroys = true
                }
            }
        });
    }

    public void spawnError(string error)
    {
        ModalWindowSpawner.instance.Spawn(new ModalWindow
        {
            isScroll = false,
            canClose = true,
            title = "There was an error removing the mod",
            content = new string[1] {
                error
            },
            buttons = new List<ModalWindowButton>
            {
                new ModalWindowButton{
                    onClick = () => {},
                    text = "OK",
                    destroys = true
                }
            }
        });
    }

    public void Edit()
    {
        OpenMod.mod = mod;
        SceneManager.LoadScene("ModMaker");
    }
}
