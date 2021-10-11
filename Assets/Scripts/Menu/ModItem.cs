using UnityEngine;
using UnityEngine.UI;
using PyMods;

public class ModItem : MonoBehaviour
{
    /// <summary>
    /// the prefab that shows the error message
    /// </summary>
    public GameObject aboutPrefab;

    /// <summary>
    /// the prefab that shows the error message
    /// </summary>
    public GameObject confirmPrefab;

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
        // create confirm dialog
        GameObject go = Instantiate(confirmPrefab);

        // setup delete action
        GameObject confirm = GameObject.FindGameObjectsWithTag("ConfirmButton")[0];
        confirm.GetComponent<Button>().onClick.AddListener(() =>
        {
            string error = mod.Remove();
            if (error != "")
            {
                // create the info object
                Transform t = ModItem.Instantiate(aboutPrefab).transform;

                // setup the info text
                t.GetChild(0).GetChild(1).GetChild(1).GetComponent<Text>().text = "Error:";
                t.GetChild(0).GetChild(1).GetChild(2).GetComponent<Text>().text = $"The mod {error} requires this mod.";
            }
            Destroy(go);
        });
    }
}
