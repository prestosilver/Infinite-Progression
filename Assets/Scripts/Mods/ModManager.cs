using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace PyMods
{
    public class ModManager
    {
        public static ModManager instance = new ModManager();

        public Save currentSave;

        public List<Mod> GetModList()
        {
            List<Mod> result = new List<Mod>();
            List<string> SearchPath = new List<string> { Application.persistentDataPath + "/Mods/" };
#if !UNITY_ANDROID
                SearchPath.Add(Application.dataPath + "/../Mods/");
#endif
            foreach (string path in SearchPath)
            {
                if (!Directory.Exists(path)) continue;
                foreach (string dir in Directory.GetDirectories(path))
                {
                    if (File.Exists(dir + "/info.json"))
                    {
                        result.Add(new Mod(dir));
                    }
                }
            }
            return result;
        }

        public List<Mod> LoadMods(List<string> ToLoad, int stage = 0, List<string> loaded = null)
        {
            if (stage > 5) return new List<Mod>();
            if (loaded == null)
            {
                loaded = new List<string>();
            }
            List<Mod> mods = GetModList();
            List<Mod> result = new List<Mod>();

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
