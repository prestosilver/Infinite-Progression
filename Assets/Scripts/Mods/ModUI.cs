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
        public int height;
    }

    [Serializable]
    public class ModUIButton
    {
        public string text;

        public int x;
        public int y;
        public int w;
        public int h;

        public string onClick;
    }

    [Serializable]
    public class ModUISlider
    {
        public string text;

        public int x;
        public int y;
        public int w;
        public int h;

        public double progress;
    }
}
