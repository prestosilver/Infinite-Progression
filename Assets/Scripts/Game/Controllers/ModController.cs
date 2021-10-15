using System.Collections;
using System.Collections.Generic;
using PyMods;
using UnityEngine;
using UnityEngine.UI;

public class ModController : GenericController
{
    /// <summary>
    /// the mod that gives this controller its behaviors
    /// </summary>
    public Mod mod;

    /// <summary>
    /// the root panel in the ui
    /// </summary>
    private GameObject panel;

    /// <summary>
    /// a ui prefab
    /// </summary>
    [SerializeField]
    private GameObject panelPrefab, ButtonPrefab, sliderPrefab, textPrefab;

    /// <summary>
    /// the sliders to update per frame
    /// </summary>
    private Dictionary<Slider, string> sliders;

    /// <summary>
    /// the text to update per frame
    /// </summary>
    private Dictionary<Text, string> dyntext;

    /// <summary>
    /// the buttons to update per frame
    /// </summary>
    private Dictionary<Button, string> btns;

    /// <summary>
    /// the mod data
    /// </summary>
    public dynamic data;

    /// <summary>
    /// constructs the ui, based on the mods ModUI
    /// </summary>
    /// <param name="ui">the mod UI</param>
    /// <returns>the root panel</returns>
    public GameObject ConstructUI(ModUI ui)
    {
        // init variables
        sliders = new Dictionary<Slider, string>();
        dyntext = new Dictionary<Text, string>();
        btns = new Dictionary<Button, string>();

        // setup a result
        GameObject result = Instantiate(panelPrefab, transform);

        // instantiate buttons
        foreach (ModUIButton btn in ui.buttons)
        {
            GameObject button = Instantiate(ButtonPrefab, result.transform);
            button.GetComponent<RectTransform>().anchoredPosition = new Vector2(btn.x, btn.y);
            button.GetComponent<RectTransform>().sizeDelta = new Vector2(btn.w, btn.h);
            button.GetComponent<Button>().onClick.AddListener(() => mod.onClick(data, btn.onClick));
            btns.Add(button.GetComponent<Button>(), btn.enable);
        }

        // instantiate slider
        foreach (ModUISlider sl in ui.sliders)
        {
            GameObject slider = Instantiate(sliderPrefab, result.transform);
            slider.GetComponent<RectTransform>().anchoredPosition = new Vector2(sl.x, sl.y);
            slider.GetComponent<RectTransform>().sizeDelta = new Vector2(sl.w, sl.h);
            sliders.Add(slider.GetComponent<Slider>(), sl.variable);
        }

        // instantiate text
        foreach (ModUIText txt in ui.text)
        {
            GameObject slider = Instantiate(textPrefab, result.transform);
            slider.GetComponent<RectTransform>().anchoredPosition = new Vector2(txt.x, txt.y);
            slider.GetComponent<RectTransform>().sizeDelta = new Vector2(txt.w, txt.h);
            // detect if theres dyntext
            if (txt.dynamic_text != null)
                dyntext.Add(slider.GetComponent<Text>(), txt.dynamic_text);
            else
                slider.GetComponent<Text>().text = txt.static_text;
        }

        return result;
    }

    /// <summary>
    /// setup the variables
    /// </summary>
    /// <param name="id">the module id</param>
    /// <param name="sliders">all the other modules</param>
    public override void SetupVars(int id, List<GameObject> sliders)
    {
        // create the ui
        panel = ConstructUI(mod.UI);

        // create the data
        data = mod.createModule(id);
    }

    /// <summary>
    /// process multiple ticks
    /// </summary>
    /// <param name="ticks">the amount of ticks to process</param>
    /// <returns>true if success, rn always true</returns>
    public override bool BulkTick(BigNumber ticks)
    {
        data = mod.BulkTick(data, ticks);
        return true;
    }

    /// <summary>
    /// update the ui to reflect the data
    /// </summary>
    public override void UpdateDisplay()
    {
        foreach (Slider slider in sliders.Keys)
        {
            slider.value = mod.GetVar(data, sliders[slider]).mantissa;
        }

        foreach (Text textf in dyntext.Keys)
        {
            textf.text = mod.GetVar(data, dyntext[textf]);
        }

        foreach (Button btn in btns.Keys)
        {
            btn.interactable = mod.GetFunc(data, btns[btn]);
        }
    }

    /// <summary>
    /// the type of the module
    /// </summary>
    public override string typeName => mod.name;

    /// <summary>
    /// loads a save
    /// </summary>
    /// <param name="save">the save data</param>
    public override void LoadSave(string save)
    {
        data = mod.LoadSave(save, id);
    }

    /// <summary>
    /// gets the save data for saving
    /// </summary>
    /// <returns>the save data</returns>
    public override string saveData() => mod.saveData(data);

    /// <summary>
    /// processes a prestige
    /// </summary>
    public override void Prestige()
    {
        data = mod.onPrestige(data);
    }

    public void SetVar<T>(string name, T value)
    {
        mod.SetVar<T>(data, name, value);
    }
}
