using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsControler : MonoBehaviour
{
    public GameObject confirmPrefab, seedPrefab;
    public Toggle LongNotation;
    public Toggle RandNames;
    public Text SeedText;
    public GameObject ChangeLog;

    public void Save()
    {
        PlayerPrefs.SetString("LongNotation", "" + LongNotation.isOn);
        PlayerPrefs.SetString("RandNames", "" + RandNames.isOn);
    }

    public void ShowChangeLog()
    {
        Instantiate(ChangeLog);
    }

    public void Start()
    {
        ConsistantTPS.tps = new BigNumber(120);
        LongNotation.isOn = (PlayerPrefs.GetString("LongNotation") == "True");
        RandNames.isOn = (PlayerPrefs.GetString("RandNames") == "True");
    }

    public void SignIn()
    {
        PlayGamesScript.SignIn();
    }

    public static void Load()
    {
        BigNumber.LongNotation = (PlayerPrefs.GetString("LongNotation") == "True");
    }

    public void Reset()
    {
        Saves.savePath = Application.persistentDataPath + "/save.dat";
        Instantiate(confirmPrefab);
        GameObject confirm = GameObject.FindGameObjectsWithTag("ConfirmButton")[0];
        confirm.GetComponent<Button>().onClick.AddListener(StartNewGame);
    }

    public void ResetSeed()
    {
        Saves.savePath = Application.persistentDataPath + "/save.dat";
        Instantiate(seedPrefab);
        GameObject confirm = GameObject.FindGameObjectsWithTag("ConfirmButton")[0];
        confirm.GetComponent<Button>().onClick.AddListener(StartNewGame);
    }

    public void ResetSandbox()
    {
        Saves.savePath = Application.persistentDataPath + "/sandbox.dat";
        Instantiate(confirmPrefab);
        GameObject confirm = GameObject.FindGameObjectsWithTag("ConfirmButton")[0];
        confirm.GetComponent<Button>().onClick.AddListener(StartNewGame);
    }

    public void StartNewGame()
    {
        // Saves.Reset();
        // if (GameObject.FindGameObjectsWithTag("SeedBox").Length != 0)
        // {
        //     int.TryParse(GameObject.FindGameObjectsWithTag("SeedBox")[0].GetComponent<InputField>().text, out int seed);
        //     Saves.Reset(0, true, seed);
        // }
        Home();
    }

    public void Home()
    {
        Save();
        SceneManager.LoadScene("MainMenu");
        Load();
    }
}
