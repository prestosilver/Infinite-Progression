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
            string[] SearchPath = { Application.persistentDataPath + "/Mods", Application.dataPath + "/../Mods" };
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

        public List<Mod> LoadMods(List<string> ToLoad)
        {
            List<Mod> mods = GetModList();
            List<Mod> result = new List<Mod>();

            foreach (Mod mod in mods)
            {
                if (ToLoad.Contains(mod.name))
                {
                    result.Add(mod);
                    mod.Load();
                }
            }

            return result;
        }
    }
}
