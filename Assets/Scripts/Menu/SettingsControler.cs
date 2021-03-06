using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsControler : MonoBehaviour
{
    /// <summary>
    /// the confirm dialog
    /// </summary>
    public GameObject confirmPrefab;

    /// <summary>
    /// the toggle that changes long notation
    /// </summary>
    public Toggle LogNotation;

    /// <summary>
    /// the random names toggle
    /// </summary>
    public Toggle RandNames;

    /// <summary>
    /// the debug toggle
    /// </summary>
    public Toggle EnableDebug;

    /// <summary>
    /// the menu animation toggle
    /// </summary>
    public Toggle MenuAnimation;

    /// <summary>
    /// the changelog
    /// </summary>
    public TextAsset changeLog;

    /// <summary>
    /// saves settings
    /// </summary>
    public void Save()
    {
        PlayerPrefs.SetString("LogNotation", "" + LogNotation.isOn);
        PlayerPrefs.SetString("RandNames", "" + RandNames.isOn);
        PlayerPrefs.SetString("MenuAnimation", "" + MenuAnimation.isOn);
        PlayerPrefs.SetString("Debug", "" + EnableDebug.isOn);
    }

    /// <summary>
    /// shows the change log
    /// </summary>
    public void ShowChangeLog()
    {
        ModalWindowSpawner.instance.Spawn(new ModalWindow
        {
            isScroll = true,
            canClose = true,
            title = "ChangeLog",
            content = new string[1] {
                    changeLog.text,
                },
            buttons = new List<ModalWindowButton>
                {
                    new ModalWindowButton{
                        onClick = ()=>{},
                        text = "OK",
                        destroys = true
                    }
                }
        });
    }

    /// <summary>
    /// setup the settings
    /// </summary>
    public void Start()
    {
        ConsistantTPS.tps = new BigNumber(120);
        LogNotation.isOn = (PlayerPrefs.GetString("LogNotation") == "True");
        RandNames.isOn = (PlayerPrefs.GetString("RandNames") == "True");
        MenuAnimation.isOn = (PlayerPrefs.GetString("MenuAnimation") == "True");
        EnableDebug.isOn = (PlayerPrefs.GetString("Debug") == "True");
    }

    /// <summary>
    /// signin gpgs
    /// </summary>
    public void SignIn()
    {
        PlayGamesScript.SignIn();
    }

    /// <summary>
    /// load settings
    /// </summary>
    public static void Load()
    {
        BigNumber.LogNotation = (PlayerPrefs.GetString("LogNotation") == "True");
    }

    /// <summary>
    /// go home
    /// </summary>
    public void Home()
    {
        Save();
        SceneManager.LoadScene("MainMenu");
        Load();
    }
}
