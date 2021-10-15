using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PyMods;
using UnityEngine;
using UnityEngine.InputSystem;

namespace IP.Debug
{
    public class DebugConsole : MonoBehaviour
    {
        string input;
        string logs;

        bool showConsole = false;

        public static DebugCommand ADD_SLIDER;
        public static DebugCommand ADD_NEXT;
        public static DebugCommand LIST_MODS;
        public static DebugCommand<string> ADD_MOD;
        public static DebugCommand<int, BigNumber> SET_SLIDER;
        public static DebugCommand<int, string, int> SET_MOD_INT;
        public static DebugCommand<int, string, BigNumber> SET_MOD_BIG;

        public List<object> CommandList;

        public void OnToggleDebug(InputValue value)
        {
            showConsole = !showConsole;
            input = "";
        }

        public void OnReturn()
        {
            if (showConsole)
            {
                HandleInput(input);
                input = "";
            }
        }

        private void Awake()
        {
            ADD_SLIDER = new DebugCommand("addslider", "adds a slider", "addslider", () =>
            {
                GameController.instance.slider_ammnt += 1;
                GameController.instance.AddType(GameController.instance.slider_ammnt, 0);
                UnityEngine.Debug.Log("Added slider module");
            });
            ADD_NEXT = new DebugCommand("addnext", "adds the next for free", "addnext", () =>
            {
                GameController.instance.slider_ammnt += 1;
                GameController.instance.AddNext(GameController.instance.slider_ammnt);
                UnityEngine.Debug.Log("Added next module");
            });
            LIST_MODS = new DebugCommand("listmods", "adds the mod with the specified id, only matches first word in mod name", "listmods", () =>
            {
                string result = "Mods: ";

                List<string> mods = new List<string>();
                // add mods to list
                foreach (Mod m in GameController.mods)
                {
                    if (!mods.Contains(m.name))
                        mods.Add(m.name);
                }
                result += string.Join(", ", mods);
                UnityEngine.Debug.Log(result);
            });
            ADD_MOD = new DebugCommand<string>("addmod", "adds the mod with the specified id, only matches first word in mod name", "addmod name", (name) =>
            {
                int id = 0;
                foreach (Mod mod in GameController.mods)
                {
                    if (mod.name.Split(' ')[0].ToLower() == name.ToLower())
                    {
                        GameController.instance.slider_ammnt += 1;
                        GameController.instance.AddMod(GameController.instance.slider_ammnt, id);
                        UnityEngine.Debug.LogError($"Added '{name}' module");
                        return;
                    }
                    id++;
                }

                UnityEngine.Debug.LogError($"No mod named '{name}' is loaded");
            });
            SET_MOD_INT = new DebugCommand<int, string, int>("modint", "sets a int variable in data for a mod", "modint id variable value", (id, variable, value) =>
            {
                GameController.instance.sliders[id].GetComponent<ModController>().SetVar<int>(variable, value);
                UnityEngine.Debug.Log($"Set '{variable}' to {value}");
            });
            SET_MOD_BIG = new DebugCommand<int, string, BigNumber>("modbig", "sets a big number variable in data for a mod", "modbig id variable value", (id, variable, value) =>
            {
                GameController.instance.sliders[id].GetComponent<ModController>().SetVar<BigNumber>(variable, value);
                UnityEngine.Debug.Log($"Set '{variable}' to {value}");
            });
            SET_SLIDER = new DebugCommand<int, BigNumber>("setslider", "sets the value of a slider", "setslider id value", (id, value) =>
            {
                GameController.instance.sliders[id].GetComponent<SliderController>().value = value;
                UnityEngine.Debug.Log($"Set '{value}' to {value}");
            });

            CommandList = new List<object>
            {
                ADD_SLIDER,
                ADD_NEXT,
                LIST_MODS,
                ADD_MOD,
                SET_MOD_INT,
                SET_MOD_BIG,
                SET_SLIDER
            };

            Application.logMessageReceivedThreaded += (cond, st, type) =>
            {
                logs += "\n";
                switch (type)
                {
                    case LogType.Error:
                        logs += "<color=red>";
                        break;
                    case LogType.Exception:
                        logs += "<color=red>";
                        break;
                    case LogType.Warning:
                        logs += "<color=yellow>";
                        break;
                    default:
                        logs += "<color=white>";
                        break;
                }
                logs += cond;
                List<string> lines = new List<string>(logs.Split('\n'));
                if (lines.Count > 6)
                    logs = string.Join("\n", lines.Skip(lines.Count - 6));
                logs += "</color>";
            };
        }

        private void OnGUI()
        {
            if (!showConsole) return;

            float y = 100f;

            GUI.Box(new Rect(0f, 0f, Screen.width, y + 30f), "");

            GUI.Label(new Rect(2.5f, 2.5f, Screen.width - 5, y - 5), logs.TrimStart('\n'));

            GUI.backgroundColor = Color.white;
            GUI.contentColor = Color.white;

            GUI.SetNextControlName("textfield");
            input = GUI.TextField(new Rect(10f, y + 5f, Screen.width - 20f, 20f), input);
            GUI.FocusControl("textfield");
        }

        private void HandleInput(string input)
        {
            string[] properties = input.Split(' ');

            foreach (object item in CommandList)
            {
                DebugCommandBase commandBase = item as DebugCommandBase;

                if (properties[0] == commandBase.CommandId)
                {
                    if (item as DebugCommand != null)
                    {
                        (commandBase as DebugCommand).Invoke();
                        return;
                    }
                    else if (item as DebugCommand<int, BigNumber> != null)
                    {
                        (item as DebugCommand<int, BigNumber>).Invoke(int.Parse(properties[1]), BigNumber.Parse(properties[2]));
                        return;
                    }
                    else if (item as DebugCommand<int, string, int> != null)
                    {
                        (item as DebugCommand<int, string, int>).Invoke(int.Parse(properties[1]), properties[2], int.Parse(properties[3]));
                        return;
                    }
                    else if (item as DebugCommand<int, string, BigNumber> != null)
                    {
                        (item as DebugCommand<int, string, BigNumber>).Invoke(int.Parse(properties[1]), properties[2], BigNumber.Parse(properties[3]));
                        return;
                    }
                    else if (item as DebugCommand<string> != null)
                    {
                        (item as DebugCommand<string>).Invoke(properties[1]);
                        return;
                    }
                }
            }
            UnityEngine.Debug.LogWarning($"No such command {properties[0]}");
        }
    }
}
