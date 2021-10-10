using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace PyMods
{
    [Serializable]
    public class ModInfo
    {
        public string install_name;
        public string path;
    }

    [Serializable]
    public class ModPackageData
    {
        public string version;
        public List<string> git_requires;
        public List<ModInfo> contents;
    }

    public static class ModPackage
    {
        public static void install(string path)
        {
            // get the data
            ModPackageData data = JsonUtility.FromJson<ModPackageData>(File.ReadAllText(path + "/package.json"));

            // copy the contents
            foreach (ModInfo mod in data.contents)
            {
                Directory.Move(path + "/" + mod.path, Application.persistentDataPath + "/Mods/" + mod.install_name);
            }
            Directory.Delete(path, true);
        }

        public static List<string> getReqs(string path)
        {
            return JsonUtility.FromJson<ModPackageData>(File.ReadAllText(path + "/package.json")).git_requires;
        }
    }
}