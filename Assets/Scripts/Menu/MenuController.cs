using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    /// <summary>
    /// the version info prefix
    /// </summary>
    const string VersionPrefix = "R";

    /// <summary>
    /// the text displaying the version
    /// </summary>
    public Text vtext;

    /// <summary>
    /// the menu animator instance
    /// </summary>
    public MenuAnimation ma;

    /// <summary>
    /// the transition animator
    /// </summary>
    public Animator transition;

    /// <summary>
    /// the changelog prefab
    /// </summary>
    public GameObject ChangeLog;

    /// <summary>
    /// the add mod from github popup
    /// </summary>
    public GameObject GHPopup;

    /// <summary>
    /// the object foreground
    /// </summary>
    public Transform Foreground;

    /// <summary>
    /// setup the menu
    /// </summary>
    public void Start()
    {
        // setup frame rate
        Application.targetFrameRate = 60;

        // gpgs login
        PlayGamesScript.SignIn();

        // setup version text
        vtext.text = VersionPrefix + "-" + Application.version;

        // Disable screen dimming
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        // show change log on new save / version
        if (PlayerPrefs.GetString("Version") != VersionPrefix + "-" + Application.version)
        {
            Instantiate(ChangeLog);
            PlayerPrefs.SetString("Version", VersionPrefix + "-" + Application.version);
        }
    }

    /// <summary>
    /// show the save select menu
    /// </summary>
    public void ContinueGame()
    {
        StartCoroutine(LoadScene("SaveSelect"));
    }

    /// <summary>
    /// show the settings menu
    /// </summary>
    public void Settings()
    {
        StartCoroutine(LoadScene("Settings"));
    }

    /// <summary>
    /// loads a scene from name
    /// </summary>
    /// <param name="name">the scene to load</param>
    public IEnumerator LoadScene(string name)
    {
        // load settings
        SettingsControler.Load();

        // show transition
        transition.SetTrigger("Start");
        yield return StartCoroutine(ma.Transition());

        // load the scene
        SceneManager.LoadScene(name);
    }

    /// <summary>
    /// starts a new game
    /// </summary>
    public void StartNewGame()
    {
        ContinueGame();
    }

    /// <summary>
    /// donate button click action
    /// </summary>
    public void Donate()
    {
        Application.OpenURL("https://ko-fi.com/prestosilver");
    }

    /// <summary>
    /// discord thing
    /// </summary>
    public void Discord()
    {
        Application.OpenURL("https://discord.gg/vrVVXktmfV");
    }

    /// <summary>
    /// clone the github template thing
    /// </summary>
    public void Clone()
    {
        Application.OpenURL("https://github.com/prestosilver/IP-Mod-Template/generate");
    }

    /// <summary>
    /// shows the gpgs leaderboards
    /// </summary>
    public void ShowLeaderboards()
    {
        PlayGamesScript.ShowLeaderboardsUI();
    }

    /// <summary>
    /// closes the game
    /// </summary>
    public void Quit()
    {
        Application.Quit();
    }

    /// <summary>
    /// show the github add mod thing
    /// </summary>
    public void ShowGHPopup()
    {
        Instantiate(GHPopup, Foreground);
    }
}
