﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HelpController : MonoBehaviour
{
    /// <summary>
    /// this file probably is not used
    /// </summary>
    public void Home()
    {
        SceneManager.LoadSceneAsync("MainMenu");
    }
}
