using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullscreenManager : MonoBehaviour
{
    public static FullscreenManager Instance = new FullscreenManager();

    void Awake()
    {
        Screen.fullScreen = PlayerPrefs.GetString("Windowed") == "False";
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void OnToggleFullscreen()
    {
        Screen.fullScreen = !Screen.fullScreen;
        PlayerPrefs.SetString("Windowed", (!Screen.fullScreen).ToString());
        Debug.Log($"Fullscreen is {Screen.fullScreen}");
    }
}
