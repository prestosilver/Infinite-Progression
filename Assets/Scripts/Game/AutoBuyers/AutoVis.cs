using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AutoVis : MonoBehaviour
{
    public Button upgradeBtn, downgradeBtn;
    public Slider progressBar;
    public Text title;
    public AutoController cont;
    // Start is called before the first frame update
    public void SetText(string t)
    {
        title.text = t;
    }

    public void SetPValue(float v)
    {
        progressBar.value = v;
    }

    public void SetPMaxValue(float v)
    {
        progressBar.maxValue = v;
    }

    public void Upgrade()
    {
        cont.Upgrade();
    }

    public void Downgrade()
    {
        cont.Downgrade();
    }
}
