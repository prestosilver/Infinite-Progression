using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using UnityEngine;

// example git url:
// https://github.com/bob16795/Ip-creative-mode-test-mod/archive/refs/heads/main.zip

/// <summary>
/// will download and verify github repository mods
/// </summary>
public static class GitControler
{
    public static GameObject SuccessPopup;
    private const string PREFIX = "github.com";
    public static bool CheckUrl(string url)
    {
        Uri uri = new Uri("http://" + url);

        if (uri.Host != "github.com") return false;
        try
        {
            Uri rawJsonUri = new Uri("https://raw.githubusercontent.com/" + uri.GetLeftPart(UriPartial.Path) + "/main/info.json");
            var request = WebRequest.Create(rawJsonUri);
        }
        catch (WebException we)
        {
            HttpWebResponse errorResponse = we.Response as HttpWebResponse;
        }

        return true;
    }

    internal static string GetReason()
    {
        return "Its just bad!";
    }

    public static Uri getDownloadUrl(string url)
    {
        return new Uri(new Uri("http://" + url + "/"), "archive/refs/heads/main.zip");
    }

    public static void download(string url)
    {
        if (!Directory.Exists(Application.persistentDataPath + "/Mods")) Directory.CreateDirectory(Application.persistentDataPath + "/Mods");
        WebClient webClient = new WebClient();
        webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
        webClient.DownloadFileAsync(getDownloadUrl(url), Application.persistentDataPath + "/Mods/temp.zip");
    }

    private static void Completed(object sender, AsyncCompletedEventArgs e)
    {
        ZipUtil.Unzip(Application.persistentDataPath + "/Mods/temp.zip", Application.persistentDataPath + "/Mods");
        File.Delete(Application.persistentDataPath + "/Mods/temp.zip");
        Debug.Log($"Added Mod");
        SuccessPopup.SetActive(true);
    }
}
