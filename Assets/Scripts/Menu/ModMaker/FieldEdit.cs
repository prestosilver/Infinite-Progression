using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class FieldEdit : MonoBehaviour
{
    [SerializeField]
    private Text _text;
    [SerializeField]
    private InputField _textField;

    private Setter updateAction;

    delegate void Setter(string value);

    // Start is called before the first frame update
    public void Setup(string name, string value)
    {
        _text.text = name;
        _textField.text = value;
        updateAction = (value) =>
        {
            Selection.instance.selectedData.Set(name, value);
        };
    }

    public void Update()
    {
        updateAction(_textField.text);
    }
}
