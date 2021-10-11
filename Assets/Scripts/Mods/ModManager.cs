using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace PyMods
{
    /// <summary>
    /// manages mods, can get a list of mods
    /// </summary>
    public class ModManager
    {
        // singleton
        public static ModManager instance = new ModManager();

        /// <summary>
        /// the current loaded save
        /// </summary>
        public Save currentSave;

        /// <summary>
        /// gets the list of mods
        /// </summary>
        /// <returns>the list of mods</returns>
        public List<Mod> GetModList()
        {
            List<Mod> result = new List<Mod>();
            List<string> SearchPath = new List<string> { Path.Combine(Application.persistentDataPath, "Mods") };
            SearchPath.Add(Application.dataPath + "/../Mods/");
            foreach (string path in SearchPath)
            {
                // make sure directory exists
                if (!Directory.Exists(path)) continue;
                foreach (string dir in Directory.GetDirectories(path))
                {
                    // make sure the mod has a corresponding info file
                    if (File.Exists(Path.Combine(dir, "info.json")))
                    {
                        result.Add(new Mod(dir));
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// loads the mods from a list of mods
        /// </summary>
        /// <param name="ToLoad">the list</param>
        /// <param name="stage">recursion stage</param>
        /// <param name="loaded">list of loaded for recursion</param>
        /// <returns>a list of loaded mods</returns>
        public List<Mod> LoadMods(List<string> ToLoad, int stage = 0, List<string> loaded = null)
        {
            // make sure this cant loop forever
            if (stage > 5) return new List<Mod>();

            // initialize loaded
            if (loaded == null) loaded = new List<string>();

            // get all mods
            List<Mod> mods = GetModList();

            // the result list of loaded mods
            List<Mod> result = new List<Mod>();

            // loop through the mods and load the required mods
            foreach (Mod mod in mods)
            {
                if (ToLoad.Contains(mod.name) && !loaded.Contains(mod.name))
                {
                    mod.Load();
                    loaded.Add(mod.name);
                    result.Add(mod);
                    result.AddRange(LoadMods(mod.requires, stage + 1, loaded));
                }
            }

            return result;
        }
    }
}
