using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        public static DebugCommand<int, int> SET_SLIDER;
        public static DebugCommand<int, string, int> SET_MOD_VALUE;

        public List<object> CommandList;

        public void OnToggleDebug(InputValue value)
        {
            showConsole = !showConsole;
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
            ADD_SLIDER = new DebugCommand("addslider", "adds a slider", "add_slider", () =>
            {
                GameController.instance.slider_ammnt += 1;
                GameController.instance.AddType(GameController.instance.slider_ammnt, 0);
            });
            ADD_NEXT = new DebugCommand("addnext", "adds the next for free", "add_next", () =>
            {
                GameController.instance.slider_ammnt += 1;
                GameController.instance.AddNext(GameController.instance.slider_ammnt);
            });
            SET_MOD_VALUE = new DebugCommand<int, string, int>("modint", "sets a int variable in data for a mod", "modint id variable value", (id, variable, value) =>
            {
                GameController.instance.sliders[id].GetComponent<ModController>().SetVar<int>(variable, value);
            });
            SET_SLIDER = new DebugCommand<int, int>("setslider", "sets the value of a slider", "setslider id value", (id, value) =>
            {

            });

            CommandList = new List<object>
            {
                ADD_SLIDER,
                ADD_NEXT,
                SET_MOD_VALUE,
                SET_SLIDER
            };

            Application.logMessageReceivedThreaded += (cond, st, type) =>
            {
                if (type != LogType.Exception && type != LogType.Error) return;

                logs += "\n" + cond;
                List<string> lines = new List<string>(logs.Split('\n'));
                if (lines.Count > 6)
                    logs = string.Join("\n", lines.Skip(lines.Count - 6));
            };
        }

        private void OnGUI()
        {
            if (!showConsole) return;

            float y = 100f;

            GUI.Box(new Rect(0f, y, Screen.width, 30), "");

            GUI.contentColor = Color.red;

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
                        showConsole = false;
                        return;
                    }
                    else if (item as DebugCommand<int, string, int> != null)
                    {
                        (item as DebugCommand<int, string, int>).Invoke(int.Parse(properties[1]), properties[2], int.Parse(properties[3]));
                        showConsole = false;
                        return;
                    }
                }
            }
            showConsole = false;
            UnityEngine.Debug.LogError($"Error no such command {properties[0]}");
        }
    }
}
