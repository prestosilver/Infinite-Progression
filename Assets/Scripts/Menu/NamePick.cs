using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NamePick : MonoBehaviour
{
    public static Save modPickSave;
    public InputField nameField;

    public void Cancel()
    {
        Destroy(gameObject);
    }

    public void Confirm()
    {
        Save s = new Save();
        s.name = nameField.text;
        Saves.saveName = s.name;
        modPickSave = s;
        SceneManager.LoadScene("ModPick");
        Destroy(gameObject);
    }
}
