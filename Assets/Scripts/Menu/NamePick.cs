using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NamePick : MonoBehaviour
{
    /// <summary>
    /// the current save
    /// </summary>
    public static Save modPickSave;

    /// <summary>
    /// save name input
    /// </summary>
    public InputField nameField;

    /// <summary>
    /// cancel create save
    /// </summary>
    public void Cancel()
    {
        Destroy(gameObject);
    }

    /// <summary>
    /// creates save
    /// </summary>
    public void Confirm()
    {
        // make sure name is not empty
        if (nameField.text == "") return;

        // init save
        Save s = new Save();

        // set name to user input
        s.name = nameField.text;

        // setup save name in saves class
        Saves.saveName = s.name;

        // setup mod picker
        modPickSave = s;

        //load mod picker
        SceneManager.LoadScene("ModPick");
    }
}
