using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Microsoft.Scripting.Hosting;
using IronPython.Hosting;

namespace PyMods
{
    [Serializable]
    public class JSONModData
    {
        public string name;
        public string description;
        public string main_file;
        public string ui_file;
        public List<string> requires;
        public int chance;
    }

    public class Mod
    {
        public string name;
        public string mainFile;
        public string infoFile;
        public string uiFile;
        public List<string> requires;

        public string description;
        public ModUI UI;
        public int chance;

        public bool isEnabled;
        public bool loaded;

        public ScriptEngine engine;
        public ScriptScope scope;
        public ScriptSource source;

        public Mod(string path)
        {
            infoFile = path + "/info.json";
            JSONModData info = JsonUtility.FromJson<JSONModData>(File.ReadAllText(infoFile));
            name = info.name;
            mainFile = path + "/" + info.main_file;

            uiFile = path + "/" + info.ui_file;
            UI = JsonUtility.FromJson<ModUI>(File.ReadAllText(uiFile));

            description = info.description;
            requires = info.requires;
            chance = info.chance;

            isEnabled = false;

            engine = Python.CreateEngine();
            scope = engine.CreateScope();
            engine.CreateScriptSourceFromString("import clr\nclr.AddReference(\'IP.Lib\')\nclr.AddReference('IP.Game')").Execute();
            source = engine.CreateScriptSourceFromFile(mainFile);
            source.Execute(scope);
        }

        public void Load()
        {
            loaded = true;
            dynamic data = scope.GetVariable<Func<object>>("onLoad")();
            Debug.Log(data);
        }

        public void Unload()
        {
            loaded = false;
            dynamic data = scope.GetVariable<Func<object>>("onUnload")();
            Debug.Log(data);
        }

        public dynamic onPrestige(dynamic data)
        {
            data = scope.GetVariable<Func<object, object>>("onPrestige")(data);
            return data;
        }

        public dynamic Tick(dynamic data)
        {
            data = scope.GetVariable<Func<object, object>>("Tick")(data);
            Debug.Log(engine.Operations.GetMember(data, "result"));
            return data;
        }

        public dynamic BulkTick(dynamic data, BigNumber ticks)
        {
            data = scope.GetVariable<Func<object, object, object>>("bulkTick")(data, ticks);
            return data;
        }

        public dynamic GetVar(dynamic data, string name)
        {
            data = engine.Operations.GetMember(data, name);
            return data;
        }

        public dynamic GetFunc(dynamic data, string name)
        {
            data = scope.GetVariable<Func<object, object>>(name)(data);
            return data;
        }

        public dynamic onClick(dynamic data, string button)
        {
            data = scope.GetVariable<Func<object, object>>(button)(data);
            return data;
        }

        public dynamic createModule(int id)
        {
            return scope.GetVariable<Func<object, object>>("createModule")(id);
        }

        public dynamic LoadSave(string save, int id)
        {
            dynamic data = scope.GetVariable<Func<object, object, object>>("loadSave")(save, id);
            return data;
        }

        public dynamic saveData(dynamic data)
        {
            data = scope.GetVariable<Func<object, object>>("saveData")(data);
            return data;
        }
    }
}