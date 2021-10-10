using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using PyMods;
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

    private static List<string> lines = new List<String>();

    public static bool CheckUrl(string url)
    {
        Uri uri = new Uri("http://" + url);

        if (uri.Host != "github.com") return false;
        try
        {
            Uri rawJsonUri = new Uri("https://raw.githubusercontent.com/" + uri.GetLeftPart(UriPartial.Path) + "/main/package.json");
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
        if (!File.Exists(Application.persistentDataPath + "/Mods/list.txt")) File.Create(Application.persistentDataPath + "/Mods/list.txt").Close();
        lines = new List<string>(File.ReadAllLines(Application.persistentDataPath + "/Mods/list.txt"));
        WebClient webClient = new WebClient();
        webClient.DownloadFileCompleted += new AsyncCompletedEventHandler((a, b) => lines.Add(url));
        webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
        webClient.DownloadFileAsync(getDownloadUrl(url), Application.persistentDataPath + "/Mods/temp.zip");
    }

    public static void downloadReq(string url)
    {
        if (!Directory.Exists(Application.persistentDataPath + "/Mods")) Directory.CreateDirectory(Application.persistentDataPath + "/Mods");
        WebClient webClient = new WebClient();
        webClient.DownloadFileCompleted += new AsyncCompletedEventHandler((a, b) => lines.Add(url));
        webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(CompletedReq);
        webClient.DownloadFile(getDownloadUrl(url), Application.persistentDataPath + "/Mods/temp.zip");
    }

    private static void Completed(object sender, AsyncCompletedEventArgs e)
    {
        // unzip and delete the file downloaded
        ZipUtil.Unzip(Application.persistentDataPath + "/Mods/temp.zip", Application.persistentDataPath + "/Mods/temp");
        File.Delete(Application.persistentDataPath + "/Mods/temp.zip");

        // do this until no more unsolved requirements
        List<String> reqs = new List<String>();
        do
        {
            // install all mods downloaded
            foreach (string folder in Directory.GetDirectories(Application.persistentDataPath + "/Mods/temp"))
            {
                reqs.AddRange(ModPackage.getReqs(folder));
                ModPackage.install(folder);
            }
            // download requirements so long as theyre needed
            foreach (String req in reqs.Distinct().ToList())
            {
                if (lines.Contains(req)) continue;
                download(req);
            }
        } while (reqs.Count > 0);

        // update installed
        File.WriteAllLines(Application.persistentDataPath + "/Mods/list.txt", lines);

        // cleanup
        Directory.Delete(Application.persistentDataPath + "/Mods/temp");

        // show success
        SuccessPopup.SetActive(true);
    }

    private static void CompletedReq(object sender, AsyncCompletedEventArgs e)
    {
        ZipUtil.Unzip(Application.persistentDataPath + "/Mods/temp.zip", Application.persistentDataPath + "/Mods/temp");
        File.Delete(Application.persistentDataPath + "/Mods/temp.zip");
    }
}
