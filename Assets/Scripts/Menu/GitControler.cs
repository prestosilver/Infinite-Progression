using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

// example git url:
// https://github.com/bob16795/Ip-creative-mode-test-mod/archive/refs/heads/main.zip

/// <summary>
/// will download and verify github repository mods
/// </summary>
public static class GitControler
{
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
        return new Uri(new Uri("http://" + url), "archive/refs/heads/main.zip");
    }
}
