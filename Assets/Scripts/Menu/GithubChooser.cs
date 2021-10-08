using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GithubChooser : MonoBehaviour
{
    [SerializeField]
    private InputField nameField;
    [SerializeField]
    private Text reasonText;
    [SerializeField]
    private GameObject Popup;

    public void Cancel()
    {
        Destroy(gameObject);
    }

    public void Confirm()
    {
        if (!GitControler.CheckUrl(nameField.text))
        {
            string reason = GitControler.GetReason();
            reasonText.text = "Invalid URL:\n" + reason;
            Popup.SetActive(true);
            return;
        }
        Uri uri = GitControler.getDownloadUrl(nameField.text);
        Debug.Log($"Add Mod {uri.GetLeftPart(UriPartial.Path)}");
        // if (!Directory.Exists())
        // {

        // }
    }
}
