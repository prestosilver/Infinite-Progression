using System.Collections.Generic;
using UnityEngine.Events;

public struct ModalWindowButton
{
    public UnityAction onClick;
    public string text;
    public bool destroys;
}

public struct ModalWindow
{
    public bool isScroll;
    public bool canClose;
    public string title;
    public string[] content;
    public List<ModalWindowButton> buttons;
}