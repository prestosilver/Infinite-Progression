using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GithubChooser : MonoBehaviour
{
    /// <summary>
    /// the url nameField
    /// </summary>
    [SerializeField]
    private InputField nameField;

    /// <summary>
    /// the reason text for a failed install
    /// </summary>
    [SerializeField]
    private Text reasonText;

    /// <summary>
    /// the failure popup
    /// </summary>
    [SerializeField]
    private GameObject FailPopup;

    /// <summary>
    /// the success popup
    /// </summary>
    [SerializeField]
    private GameObject SuccessPopup;

    /// <summary>
    /// destroy this when chanceled
    /// </summary>
    public void Cancel()
    {
        Destroy(gameObject);
    }

    /// <summary>
    /// install the package when the user clicks confirm
    /// </summary>
    public void Confirm()
    {
        // check valid url
        if (!GitControler.CheckUrl(nameField.text))
        {
            string reason = GitControler.GetReason(nameField.text);
            reasonText.text = "Invalid URL:\n" + reason;
            FailPopup.SetActive(true);
            return;
        }

        // download and install the package
        Uri uri = GitControler.getDownloadUrl(nameField.text);
        Debug.Log($"Add Mod {uri.GetLeftPart(UriPartial.Path)}");
        GitControler.download(nameField.text);
        GitControler.SuccessPopup = SuccessPopup;
    }
}
