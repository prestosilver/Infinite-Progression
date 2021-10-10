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
    private static string ModPath = Path.Combine(Application.persistentDataPath, "Mods");
    private static string ListPath = Path.Combine(ModPath, "list.txt");
    private static string TempPath = Path.Combine(ModPath, "temp");
    private static string TempZip = Path.Combine(ModPath, "temp.zip");

    private static List<string> lines = new List<String>();

    public static void SetupLines()
    {
        if (!Directory.Exists(ModPath)) Directory.CreateDirectory(ModPath);
        if (!File.Exists(ListPath)) File.Create(ListPath).Close();
        lines = new List<string>(File.ReadAllLines(ListPath));
    }

    public static bool CheckUrl(string url)
    {
        SetupLines();
        if (lines.Contains(url)) return false;
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

    internal static string GetReason(string url)
    {
        SetupLines();
        lines = new List<string>(File.ReadAllLines(ListPath));
        if (lines.Contains(url)) return "You already have this mod";
        return "Its just bad!";
    }

    public static Uri getDownloadUrl(string url)
    {
        return new Uri(new Uri("http://" + url + "/"), "archive/refs/heads/main.zip");
    }

    public static void download(string url)
    {
        SetupLines();
        WebClient webClient = new WebClient();
        webClient.DownloadFileCompleted += new AsyncCompletedEventHandler((a, b) => Completed(url));
        webClient.DownloadFileAsync(getDownloadUrl(url), TempZip);
    }

    public static void downloadReq(string url)
    {
        WebClient webClient = new WebClient();
        webClient.DownloadFile(getDownloadUrl(url), TempZip);
        ZipUtil.Unzip(TempZip, TempPath);
        File.Delete(TempZip);
    }

    private static void Completed(string url)
    {
        // unzip and delete the file downloaded
        ZipUtil.Unzip(TempZip, TempPath);
        File.Delete(TempZip);
        lines.Add(url);

        // do this until no more unsolved requirements
        List<String> reqs;
        do
        {
            reqs = new List<String>();
            // install all mods downloaded
            foreach (string folder in Directory.GetDirectories(TempPath))
            {
                reqs.AddRange(ModPackage.getReqs(folder));
                ModPackage.install(folder);
            }
            // download requirements so long as theyre needed
            foreach (String req in reqs.Distinct().ToList())
            {
                if (lines.Contains(req)) continue;
                downloadReq(req);
                lines.Add(req);
            }
        } while (reqs.Count > 0);

        // update installed
        File.WriteAllLines(ListPath, lines);

        // cleanup
        Directory.Delete(TempPath);

        // show success
        SuccessPopup.SetActive(true);
    }

}
