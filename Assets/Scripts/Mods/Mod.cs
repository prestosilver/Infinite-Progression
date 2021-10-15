using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Microsoft.Scripting.Hosting;
using IronPython.Hosting;
using UnityEngine.UI;

[assembly: ForceAssemblyReference(typeof(Microsoft.Scripting.Hosting.ScriptHost))]
namespace PyMods
{
    /// <summary>
    /// a serializable struct representing json mod data
    /// this is used for `info.json`
    /// </summary>
    [Serializable]
    public struct JSONModData
    {
        public string name;
        public string description;
        public string main_file;
        public string ui_file;
        public List<string> requires;
        public int chance;
    }

    /// <summary>
    /// the mod class, runs and stores python code
    /// </summary>
    public class Mod
    {
        /// <summary>
        /// the python engine
        /// </summary>
        public static ScriptEngine engine = Python.CreateEngine();

        /// <summary>
        /// the mod name
        /// </summary>
        public string name;

        /// <summary>
        /// the python file
        /// </summary>
        public string mainFile;

        /// <summary>
        /// the info.json file
        /// </summary>
        public string infoFile;

        /// <summary>
        /// the ui.json file
        /// </summary>
        public string uiFile;

        /// <summary>
        /// the list of required mods
        /// </summary>
        public List<string> requires;

        /// <summary>
        /// the information about the mod
        /// </summary>
        public string description;

        /// <summary>
        /// the mod ui data
        /// </summary>
        public ModUI UI;

        /// <summary>
        /// the chance for the mod to show up
        /// </summary>
        public int chance;

        /// <summary>
        /// wether or not the mod is enabled
        /// </summary>
        public bool isEnabled;

        /// <summary>
        /// wether or not the mod is loaded
        /// </summary>
        public bool loaded;

        /// <summary>
        /// the python scope
        /// </summary>
        public ScriptScope scope;

        /// <summary>
        /// the python source
        /// </summary>
        public ScriptSource source;

        /// <summary>
        /// the path of the mod
        /// </summary>
        public string path;

        /// <summary>
        /// mod constructor
        /// </summary>
        /// <param name="path">the name and path of the mod</param>
        public Mod(string path)
        {
            // setup the info file
            infoFile = Path.Combine(path, "info.json");

            // read the info file
            JSONModData info = JsonUtility.FromJson<JSONModData>(File.ReadAllText(infoFile));

            // copy info from the info file
            name = info.name;
            mainFile = Path.Combine(path, info.main_file);
            uiFile = Path.Combine(path, info.ui_file);
            description = info.description;
            requires = info.requires;
            chance = info.chance;
            this.path = path;

            // read the ui file
            UI = JsonUtility.FromJson<ModUI>(File.ReadAllText(uiFile));

            // make sure the module isnt falsly loaded
            isEnabled = false;

            // setup python scope & engine
            scope = engine.CreateScope();
            engine.CreateScriptSourceFromString("import clr\nclr.AddReference(\'IP.Lib\')\nclr.AddReference('IP.Game')").Execute();
            source = engine.CreateScriptSourceFromFile(mainFile);
            source.Execute(scope);
        }

        /// <summary>
        /// loads the mod
        /// </summary>
        public void Load()
        {
            try
            {
                loaded = true;
                dynamic data = scope.GetVariable<Func<object>>("onLoad")();
            }
            catch (Exception e)
            {
                ExceptionOperations eo = engine.GetService<ExceptionOperations>();
                string error = eo.FormatException(e);
                Debug.LogError(error);
            }
        }

        /// <summary>
        /// unloads the mod
        /// </summary>
        public void Unload()
        {
            try
            {
                loaded = false;
                dynamic data = scope.GetVariable<Func<object>>("onUnload")();
            }
            catch (Exception e)
            {
                ExceptionOperations eo = engine.GetService<ExceptionOperations>();
                string error = eo.FormatException(e);
                Debug.LogError(error);
            }
        }

        /// <summary>
        /// prestiges a module
        /// </summary>
        /// <param name="data">the module data</param>
        /// <returns>the new module data</returns>
        public dynamic onPrestige(dynamic data)
        {
            try
            {
                data = scope.GetVariable<Func<object, object>>("onPrestige")(data);
            }
            catch (Exception e)
            {
                ExceptionOperations eo = engine.GetService<ExceptionOperations>();
                string error = eo.FormatException(e);
                Debug.LogError(error);
            }
            return data;
        }

        /// <summary>
        /// processes a single tick
        /// </summary>
        /// <param name="data">the module data</param>
        /// <returns>the new module data</returns>
        public dynamic Tick(dynamic data)
        {
            try
            {
                data = scope.GetVariable<Func<object, object>>("Tick")(data);
            }
            catch (Exception e)
            {
                ExceptionOperations eo = engine.GetService<ExceptionOperations>();
                string error = eo.FormatException(e);
                Debug.LogError(error);
            }
            return data;
        }

        /// <summary>
        /// processes multiple ticks
        /// </summary>
        /// <param name="data">the module data</param>
        /// <param name="ticks">the amount of ticks to process</param>
        /// <returns>the new module data</returns>
        public dynamic BulkTick(dynamic data, BigNumber ticks)
        {
            try
            {
                data = scope.GetVariable<Func<object, object, object>>("bulkTick")(data, ticks);
            }
            catch (Exception e)
            {
                ExceptionOperations eo = engine.GetService<ExceptionOperations>();
                string error = eo.FormatException(e);
                Debug.LogError(error);
            }
            return data;
        }

        /// <summary>
        /// gets a variable from the scope, for bars and stuff
        /// </summary>
        /// <param name="data">the module data</param>
        /// <param name="name">the variable name</param>
        /// <returns>the new module data</return>returns>
        public dynamic GetVar(dynamic data, string name)
        {
            try
            {
                data = engine.Operations.GetMember(data, name);
            }
            catch (Exception e)
            {
                ExceptionOperations eo = engine.GetService<ExceptionOperations>();
                string error = eo.FormatException(e);
                Debug.LogError(error);
            }
            return data;
        }

        /// <summary>
        /// gets a variable from the scope, for bars and stuff
        /// </summary>
        /// <param name="data">the module data</param>
        /// <param name="name">the variable name</param>
        /// <returns>the new module data</return>returns>
        public void SetVar<T>(dynamic data, string name, T value)
        {
            try
            {
                engine.Operations.SetMember<T>(data, name, value);
            }
            catch (Exception e)
            {
                ExceptionOperations eo = engine.GetService<ExceptionOperations>();
                string error = eo.FormatException(e);
                Debug.LogError(error);
            }
        }

        /// <summary>
        /// gets a function, runs it with data
        /// </summary>
        /// <param name="data">the module data</param>
        /// <param name="name">the name of the function</param>
        /// <returns>the new module data</returns>
        public dynamic GetFunc(dynamic data, string name)
        {
            try
            {
                data = scope.GetVariable<Func<object, object>>(name)(data);
            }
            catch (Exception e)
            {
                ExceptionOperations eo = engine.GetService<ExceptionOperations>();
                string error = eo.FormatException(e);
                Debug.LogError(error);
            }
            return data;
        }

        /// <summary>
        /// processes a button click
        /// </summary>
        /// <param name="data">the module data</param>
        /// <param name="button">the button name</param>
        /// <returns>the new module data</returns>
        public dynamic onClick(dynamic data, string button)
        {
            try
            {
                data = scope.GetVariable<Func<object, object>>(button)(data);
            }
            catch (Exception e)
            {
                ExceptionOperations eo = engine.GetService<ExceptionOperations>();
                string error = eo.FormatException(e);
                Debug.LogError(error);
            }
            return data;
        }

        /// <summary>
        /// creates a module
        /// </summary>
        /// <param name="id">the module position id</param>
        /// <returns>the module data</returns>
        public dynamic createModule(int id)
        {
            try
            {
                return scope.GetVariable<Func<object, object>>("createModule")(id);
            }
            catch (Exception e)
            {
                ExceptionOperations eo = engine.GetService<ExceptionOperations>();
                string error = eo.FormatException(e);
                Debug.LogError(error);
            }
            return null;
        }

        /// <summary>
        /// loads a save from a string to the module
        /// </summary>
        /// <param name="save">the save data</param>
        /// <param name="id">the module position id</param>
        /// <returns>the module data</returns>
        public dynamic LoadSave(string save, int id)
        {
            try
            {
                dynamic data = scope.GetVariable<Func<object, object, object>>("loadSave")(save, id);
                return data;
            }
            catch (Exception e)
            {
                ExceptionOperations eo = engine.GetService<ExceptionOperations>();
                string error = eo.FormatException(e);
                Debug.LogError(error);
            }
            return null;
        }

        /// <summary>
        /// gets the save data from the module
        /// </summary>
        /// <param name="data">the module data</param>
        /// <returns>the save data</returns>
        public dynamic saveData(dynamic data)
        {
            try
            {
                data = scope.GetVariable<Func<object, object>>("saveData")(data);
            }
            catch (Exception e)
            {
                ExceptionOperations eo = engine.GetService<ExceptionOperations>();
                string error = eo.FormatException(e);
                Debug.LogError(error);
            }
            return data;
        }

        /// <summary>
        /// uninstalls the mod
        /// </summary>
        /// <returns>the error message</returns>
        public string Remove()
        {
            foreach (Mod m in ModManager.instance.GetModList())
            {
                if (m.requires.Contains(name))
                {
                    return m.name;
                }
            }
            Directory.Delete(path, true);
            return "";
        }
    }
}

[AttributeUsage(AttributeTargets.Assembly)]
public class ForceAssemblyReference : Attribute
{
    public ForceAssemblyReference(Type forcedType)
    {
        //not sure if these two lines are required since 
        //the type is passed to constructor as parameter, 
        //thus effectively being used
        Action<Type> noop = _ => { };
        noop(forcedType);
    }
}