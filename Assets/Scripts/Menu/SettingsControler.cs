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
    public Toggle LongNotation;

    /// <summary>
    /// the random names toggle
    /// </summary>
    public Toggle RandNames;

    /// <summary>
    /// the changelog prefab
    /// </summary>
    public GameObject ChangeLog;

    /// <summary>
    /// saves settings
    /// </summary>
    public void Save()
    {
        PlayerPrefs.SetString("LongNotation", "" + LongNotation.isOn);
        PlayerPrefs.SetString("RandNames", "" + RandNames.isOn);
    }

    /// <summary>
    /// shows the change log
    /// </summary>
    public void ShowChangeLog()
    {
        Instantiate(ChangeLog);
    }

    /// <summary>
    /// setup the settings
    /// </summary>
    public void Start()
    {
        ConsistantTPS.tps = new BigNumber(120);
        LongNotation.isOn = (PlayerPrefs.GetString("LongNotation") == "True");
        RandNames.isOn = (PlayerPrefs.GetString("RandNames") == "True");
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
        BigNumber.LongNotation = (PlayerPrefs.GetString("LongNotation") == "True");
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
