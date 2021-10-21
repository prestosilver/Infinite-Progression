using System.Collections;
using System.Collections.Generic;
using System.IO;
using PyMods;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OpenMod : MonoBehaviour
{
    public static Mod mod;

    public Transform ObjectParent;
    public InGameTextEditor.TextEditor textEditor;

    public InputField nameField;
    public InputField descField;
    public InputField chanceField;

    public GameObject savePopup;
    public GameObject editCanvas;

    public GameObject buttonPrefab, sliderPrefab, textPrefab;

    public void Start()
    {
        if (mod == null) return;
        nameField.text = mod.name;
        descField.text = mod.description;
        chanceField.text = $"{mod.chance}";
        textEditor.Text = File.ReadAllText(mod.mainFile);
        foreach (ModUIButton button in mod.UI.buttons)
        {
            SelectedData data = Instantiate(buttonPrefab, ObjectParent).GetComponent<SelectedData>();
            data.Set("X", button.x.ToString());
            data.Set("Y", (-button.y).ToString());
            data.Set("Width", button.w.ToString());
            data.Set("Height", button.h.ToString());
            data.Set("onClick", button.onClick);
            data.Set("enable", button.enable);
        }
        foreach (ModUIText text in mod.UI.text)
        {
            SelectedData data = Instantiate(textPrefab, ObjectParent).GetComponent<SelectedData>();
            data.Set("X", text.x.ToString());
            data.Set("Y", (-text.y).ToString());
            data.Set("Width", text.w.ToString());
            data.Set("Height", text.h.ToString());
            data.Set("static_text", text.static_text);
            data.Set("dynamic_text", text.dynamic_text);
        }
        foreach (ModUISlider slider in mod.UI.sliders)
        {
            SelectedData data = Instantiate(sliderPrefab, ObjectParent).GetComponent<SelectedData>();
            data.Set("X", slider.x.ToString());
            data.Set("Y", (-slider.y).ToString());
            data.Set("Width", slider.w.ToString());
            data.Set("Height", slider.h.ToString());
            data.Set("variable", slider.variable);
        }
        mod = null;
    }
}