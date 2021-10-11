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
    /// <summary>
    /// the success message
    /// </summary>
    public static GameObject SuccessPopup;

    /// <summary>
    /// the url prefix
    /// </summary>
    private const string PREFIX = "github.com";

    /// <summary>
    /// the mod path
    /// </summary>
    private static string ModPath = Path.Combine(Application.persistentDataPath, "Mods");

    /// <summary>
    /// the path for the mod list
    /// </summary>
    private static string ListPath = Path.Combine(ModPath, "list.txt");

    /// <summary>
    /// the temp directory for mods
    /// </summary>
    private static string TempPath = Path.Combine(ModPath, "temp");

    /// <summary>
    /// the temp path for a mod zip
    /// </summary>
    private static string TempZip = Path.Combine(ModPath, "temp.zip");

    /// <summary>
    /// the list of mod packages installed
    /// </summary>
    private static List<string> installedPackages = new List<String>();

    /// <summary>
    /// setup installedPackages from the modlist 
    /// </summary>
    public static void SetupInstalled()
    {
        // make sure the modlist exists
        if (!Directory.Exists(ModPath)) Directory.CreateDirectory(ModPath);
    }

    /// <summary>
    /// validates a url for installation
    /// </summary>
    /// <param name="url">the url to validate</param>
    /// <returns>wether the url is valid</returns>
    public static bool CheckUrl(string url)
    {
        // get modlist
        SetupInstalled();

        // dont install if the package is already installed
        if (installedPackages.Contains(url)) return false;

        // add http
        Uri uri = new Uri("http://" + url);

        // assert github url
        if (uri.Host != "github.com") return false;
        // look for  package.json
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

    /// <summary>
    /// gets the reason the url is invalid
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    internal static string GetReason(string url)
    {
        SetupInstalled();
        // preexists
        installedPackages = new List<string>(File.ReadAllLines(ListPath));
        if (installedPackages.Contains(url)) return "You already have this mod";
        return "Its just bad!";
    }

    /// <summary>
    /// gets the url for the package zip file
    /// </summary>
    /// <param name="url">the url of the repo</param>
    /// <returns>the uri</returns>
    public static Uri getDownloadUrl(string url)
    {
        return new Uri(new Uri("http://" + url + "/"), "archive/refs/heads/main.zip");
    }

    /// <summary>
    /// downloads a mod package from the repository
    /// </summary>
    /// <param name="url">the repo url</param>
    public static void download(string url)
    {
        SetupInstalled();
        // download the package
        WebClient webClient = new WebClient();
        webClient.DownloadFileCompleted += new AsyncCompletedEventHandler((a, b) => Completed(url));
        webClient.DownloadFileAsync(getDownloadUrl(url), TempZip);
    }

    /// <summary>
    /// downloads a required mod package from the oackage
    /// </summary>
    /// <param name="url">the requirment url</param>
    public static void downloadReq(string url)
    {
        // download the package
        WebClient webClient = new WebClient();
        webClient.DownloadFile(getDownloadUrl(url), TempZip);

        // extract the package
        ZipUtil.Unzip(TempZip, TempPath);
        File.Delete(TempZip);
    }

    /// <summary>
    /// ran when a base package is downloaded
    /// </summary>
    /// <param name="url">the package url</param>
    private static void Completed(string url)
    {
        // unzip and delete the file downloaded
        ZipUtil.Unzip(TempZip, TempPath);
        File.Delete(TempZip);
        installedPackages.Add(url);

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
                if (installedPackages.Contains(req)) continue;
                downloadReq(req);
                installedPackages.Add(req);
            }
        } while (reqs.Count > 0);

        // cleanup
        Directory.Delete(TempPath);

        // show success if exists
        if (SuccessPopup != null) SuccessPopup.SetActive(true);
    }

}
