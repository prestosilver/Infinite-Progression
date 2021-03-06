using System.Collections;
using System.Collections.Generic;
using System.IO;
using PyMods;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SaveMod : MonoBehaviour
{
    public Transform ObjectParent;
    public InGameTextEditor.TextEditor textEditor;

    public InputField nameField;
    public InputField descField;
    public InputField chanceField;

    public GameObject savePopup;
    public GameObject editCanvas;

    public ModUI GetModUI()
    {
        ModUI ui = new ModUI();
        ui.buttons = new List<ModUIButton>();
        ui.sliders = new List<ModUISlider>();
        ui.text = new List<ModUIText>();
        foreach (SelectedData data in ObjectParent.GetComponentsInChildren<SelectedData>())
        {
            if (!data.GetComponent<SelectedData>()) continue;
            switch (data.GetComponent<Selectable>().kind)
            {
                case 1:
                    ModUIButton button = new ModUIButton();
                    button.x = int.Parse(data.Get("X"));
                    button.y = int.Parse(data.Get("Y"));
                    button.w = int.Parse(data.Get("Width"));
                    button.h = int.Parse(data.Get("Height"));
                    button.onClick = data.Get("onClick");
                    button.enable = data.Get("enable");
                    ui.buttons.Add(button);
                    break;
                case 2:
                    ModUISlider slider = new ModUISlider();
                    slider.x = int.Parse(data.Get("X"));
                    slider.y = int.Parse(data.Get("Y"));
                    slider.w = int.Parse(data.Get("Width"));
                    slider.h = int.Parse(data.Get("Height"));

                    slider.variable = data.Get("Variable");
                    ui.sliders.Add(slider);
                    break;
                case 3:
                    ModUIText text = new ModUIText();
                    text.x = int.Parse(data.Get("X"));
                    text.y = int.Parse(data.Get("Y"));
                    text.w = int.Parse(data.Get("Width"));
                    text.h = int.Parse(data.Get("Height"));

                    text.dynamic_text = data.Get("dynamic_text");
                    text.static_text = data.Get("static_text");
                    ui.text.Add(text);
                    break;
            }
        }
        return ui;
    }

    public void SaveClick()
    {
        textEditor.disableInput = true;
        savePopup.SetActive(true);
    }

    public void SaveBack()
    {
        textEditor.disableInput = false;
        savePopup.SetActive(false);
    }

    public void SaveFinish()
    {
        Save(nameField.text, descField.text, int.Parse(chanceField.text));
        textEditor.disableInput = false;
        savePopup.SetActive(false);
    }

    public void Upload()
    {
        UploadSteam(nameField.text, descField.text, int.Parse(chanceField.text));
    }

    public void Save(string name, string description = "", int chance = 1)
    {
        Directory.CreateDirectory(Application.persistentDataPath + $"/Mods/{name}/");
        ModUI UIData = GetModUI();
        File.WriteAllText(Application.persistentDataPath + $"/Mods/{name}/ui.json", JsonUtility.ToJson(UIData, true));
        JSONModData data = new JSONModData();
        data.name = name;
        data.description = description;
        data.main_file = "main.py";
        data.ui_file = "ui.json";
        data.requires = new List<string> { };
        data.chance = chance;
        File.WriteAllText(Application.persistentDataPath + $"/Mods/{name}/info.json", JsonUtility.ToJson(data, true));
        File.WriteAllText(Application.persistentDataPath + $"/Mods/{name}/main.py", textEditor.Text);
        SnapshotCamera sc = SnapshotCamera.MakeSnapshotCamera();
        GameObject go = Instantiate(ObjectParent.gameObject, editCanvas.transform);
        Texture2D texture = sc.TakeObjectSnapshot(editCanvas);
        File.WriteAllBytes(Application.persistentDataPath + $"/Mods/{name}/image.png", texture.EncodeToPNG());
    }

    public void UploadSteam(string name, string description = "", int chance = 1)
    {
        Save(name, description, chance);
        SteamWorkshop.Instance.UploadContent(name, description, Application.persistentDataPath + $"/Mods/{name}", new string[1] { "Mods" }, Application.persistentDataPath + $"/Mods/{name}/image.png");
    }

    public void Home()
    {
        SceneManager.LoadScene("ModMenu");
    }
}
