using System;
using System.Collections;
using System.Collections.Generic;

namespace PyMods
{
    [Serializable]
    public class ModUI
    {
        public List<ModUIButton> buttons;
        public List<ModUISlider> sliders;
        public List<ModUIText> text;
        public int height;
    }

    [Serializable]
    public class ModUIButton
    {
        public int x;
        public int y;
        public int w;
        public int h;

        public string onClick;
        public string enable;
    }

    [Serializable]
    public class ModUISlider
    {
        public int x;
        public int y;
        public int w;
        public int h;

        public string variable;
    }

    [Serializable]
    public class ModUIText
    {
        public int x;
        public int y;
        public int w;
        public int h;

        public string dynamic_text;
        public string static_text;
    }

    [Serializable]
    public class ModUIPopup
    {
        public int w;
        public int h;

        public List<ModUIButton> buttons;
        public List<ModUISlider> sliders;
        public List<ModUIText> text;
    }
}
