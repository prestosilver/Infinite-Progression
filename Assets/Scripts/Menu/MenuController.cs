using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    const string VersionPrefix = "Release";
    public Text vtext;
    public MenuAnimation ma;
    public Animator transition;
    public GameObject ChangeLog;

    public void Start()
    {
        PlayGamesScript.SignIn();
        vtext.text = VersionPrefix + "-" + Application.version;
        // Disable screen dimming
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Application.targetFrameRate = 60;
        if (PlayerPrefs.GetString("Version") != VersionPrefix + "-" + Application.version)
        {
            Instantiate(ChangeLog);
            PlayerPrefs.SetString("Version", VersionPrefix + "-" + Application.version);
        }
    }

    public void ContinueGame()
    {
        StartCoroutine(LoadScene("SaveSelect"));
    }

    public void Settings()
    {
        StartCoroutine(LoadScene("Settings"));
    }

    public void Help()
    {
        StartCoroutine(LoadScene("Help"));
    }

    public void SandBox()
    {
        StartCoroutine(LoadScene("SandBox"));
    }

    public void Modded()
    {
        StartCoroutine(LoadScene("ModMenu"));
    }

    public IEnumerator LoadScene(string name)
    {
        SettingsControler.Load();
        transition.SetTrigger("Start");
        yield return StartCoroutine(ma.Transition());
        SceneManager.LoadScene(name);
    }

    public void StartNewGame()
    {
        // Saves.Reset();
        ContinueGame();
    }

    public void Donate()
    {
        Application.OpenURL("https://ko-fi.com/prestosilver");
    }

    public void Discord()
    {
        Application.OpenURL("https://discord.gg/vrVVXktmfV");
    }

    public void Clone()
    {
        Application.OpenURL("https://github.com/prestosilver/IP-Mod-Template/generate");
    }

    public void ShowLeaderboards()
    {
        PlayGamesScript.ShowLeaderboardsUI();
    }

    public void Quit()
    {
        Application.Quit();
    }
}
