using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace PyMods
{
    /// <summary>
    /// json information about a mod in a pack
    /// </summary>
    [Serializable]
    public struct ModInfo
    {
        public string install_name;
        public string path;
    }

    /// <summary>
    /// json information about a mod package
    /// </summary>
    [Serializable]
    public struct ModPackageData
    {
        public string version;
        public List<string> git_requires;
        public List<ModInfo> contents;
    }

    /// <summary>
    /// the mod package manager
    /// </summary>
    public static class ModPackage
    {
        /// <summary>
        /// installs a mod package
        /// </summary>
        /// <param name="path">the mod package path</param>
        public static void install(string path)
        {
            // get the data
            ModPackageData data = JsonUtility.FromJson<ModPackageData>(File.ReadAllText(path + "/package.json"));

            // copy the contents
            foreach (ModInfo mod in data.contents)
            {
                Directory.Move(Path.Combine(path, mod.path), Path.Combine(new string[] { Application.persistentDataPath, "Mods", mod.install_name }));
            }
            Directory.Delete(path, true);
        }

        /// <summary>
        /// gets the required packages for the mod package
        /// </summary>
        /// <param name="path">the mod package path</param>
        /// <returns>the mod package required packages</returns>
        public static List<string> getReqs(string path)
        {
            return JsonUtility.FromJson<ModPackageData>(File.ReadAllText(path + "/package.json")).git_requires;
        }
    }
}