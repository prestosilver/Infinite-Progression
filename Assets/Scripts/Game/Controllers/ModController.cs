using System.Collections;
using System.Collections.Generic;
using PyMods;
using UnityEngine;
using UnityEngine.UI;

public class ModController : GenericController
{
    public Mod mod;

    private GameObject panel;

    [SerializeField]
    private GameObject panelPrefab, ButtonPrefab, sliderPrefab, textPrefab;

    private Dictionary<Slider, string> sliders;
    private Dictionary<Text, string> dyntext;
    private dynamic data;

    public GameObject ConstructUI(ModUI ui)
    {
        sliders = new Dictionary<Slider, string>();
        dyntext = new Dictionary<Text, string>();
        GameObject result = Instantiate(panelPrefab, transform);

        foreach (ModUIButton btn in ui.buttons)
        {
            GameObject button = Instantiate(ButtonPrefab, result.transform);
            button.GetComponent<RectTransform>().anchoredPosition = new Vector2(btn.x, btn.y);
            button.GetComponent<RectTransform>().sizeDelta = new Vector2(btn.w, btn.h);
            button.GetComponent<Button>().onClick.AddListener(() => mod.onClick(data, btn.onClick));
        }

        foreach (ModUISlider sl in ui.sliders)
        {
            GameObject slider = Instantiate(sliderPrefab, result.transform);
            slider.GetComponent<RectTransform>().anchoredPosition = new Vector2(sl.x, sl.y);
            slider.GetComponent<RectTransform>().sizeDelta = new Vector2(sl.w, sl.h);
            sliders.Add(slider.GetComponent<Slider>(), sl.variable);
        }

        foreach (ModUIText txt in ui.text)
        {
            GameObject slider = Instantiate(textPrefab, result.transform);
            slider.GetComponent<RectTransform>().anchoredPosition = new Vector2(txt.x, txt.y);
            slider.GetComponent<RectTransform>().sizeDelta = new Vector2(txt.w, txt.h);
            if (txt.dynamic_text != "")
                dyntext.Add(slider.GetComponent<Text>(), txt.dynamic_text);
            else
                slider.GetComponent<Text>().text = txt.static_text;
        }

        return result;
    }

    public override void SetupVars(int id, List<GameObject> sliders)
    {
        panel = ConstructUI(mod.UI);
        data = mod.createModule();
    }

    public override bool BulkTick(BigNumber ticks)
    {
        data = mod.BulkTick(data, ticks);
        return true;
    }

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
    }
}
