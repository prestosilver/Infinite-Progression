using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PyMods;
using static InGameTextEditor.Operations.MoveCaretOperation;

public class ErrorBox : MonoBehaviour
{
    public static ErrorBox instance;
    void Awake() => instance = this;

    public InGameTextEditor.TextEditor textEditor;

    public GameObject errorPrefab;

    public Transform errorParent;
    public Transform objectParent;

    private List<GameObject> errors = new List<GameObject>();

    public void CheckError()
    {
        List<RequiredFunction> required = new List<RequiredFunction>();

        foreach (SelectedData data in objectParent.GetComponentsInChildren<SelectedData>())
        {

            if (!data.GetComponent<SelectedData>()) continue;
            switch (data.GetComponent<Selectable>().kind)
            {
                case 1:
                    required.Add(new RequiredFunction()
                    {
                        name = data.Get("onClick")
                    });
                    required.Add(new RequiredFunction()
                    {
                        name = data.Get("enable")
                    });
                    break;
                case 3:
                    if (data.Get("dynamic_text") != "")
                        required.Add(new RequiredFunction()
                        {
                            name = data.Get("dynamic_text")
                        });
                    break;
            }
        }

        List<ModError> errorList = Mod.CheckError(textEditor.Text, required);

        errors.ForEach((go) => Destroy(go));

        errors = new List<GameObject>();
        foreach (ModError error in errorList)
        {
            if (error.text != "")
            {
                GameObject go = Instantiate(errorPrefab, errorParent);
                go.transform.GetChild(0).GetComponent<Text>().text = error.text;
                if (!error.suggestFix)
                {
                    Destroy(go.transform.GetChild(1).gameObject);
                }
                else
                {
                    Button btn = go.transform.GetChild(1).GetComponent<Button>();
                    btn.onClick.AddListener(() =>
                    {
                        textEditor.SetText(error.fix(textEditor.Text), true);
                        textEditor.CaretPosition.lineIndex = textEditor.Text.Split('\n').Length - 1;
                        textEditor.MoveCaret(Direction.DOWN, false, false, true);
                        CheckError();
                    });
                }
                errors.Add(go);
                Debug.Log(error);
                StartCoroutine(UpdateLayout(go));
            }
        }
    }

    public IEnumerator UpdateLayout(GameObject go)
    {
        yield return null;
        go.GetComponent<ContentSizeFitter>().SetLayoutVertical();
        errorParent.GetComponent<ContentSizeFitter>().SetLayoutVertical();

        VerticalLayoutGroup layout = errorParent.GetComponent<VerticalLayoutGroup>();
        layout.enabled = false;
        yield return null;
        layout.enabled = true;
    }
}