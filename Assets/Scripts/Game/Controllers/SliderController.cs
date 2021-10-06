using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderController : GenericController
{
    public string textName = "Null";
    public int price = 10000;
    public BigNumber max = new BigNumber(10000);
    public float increase = 0;
    public int mult = 1, mult_mult = 10, mult_start = 1;
    public int auto_mult = 4, auto_start = 500;
    public Text ammountText;
    public Slider ammountSlider;
    public Button mult_button, next_button;
    public GameObject plus_button, auto_button, prevSlider;
    public bool is_first = true;
    public BigNumber value = new BigNumber(0);

    public override void LoadSave(byte[] objSave)
    {
        mult = Saves.FindInt(objSave, 4);
        increase = Saves.FindFloat(objSave, 8);
        max.mantissa = Saves.FindFloat(objSave, 12);
        max.exponent_big = Saves.FindInt(objSave, 16);
        max.exponent_little = Saves.FindInt(objSave, 20);
        value.mantissa = Saves.FindFloat(objSave, 24);
        value.exponent_big = Saves.FindInt(objSave, 28);
        value.exponent_little = Saves.FindInt(objSave, 32);
    }

    public override void SetupVars(int id, List<GameObject> sliders)
    {
        if (id == 1)
        {
            increase = 1f;
            is_first = false;
        }
        mult_mult *= 10 * id / 3;
        auto_mult *= 10 * id / 3;
    }

    // Update is called once per frame
    public override void Tick()
    {
        //Debug.Log((auto_start + Mathf.Pow(increase * 10, auto_mult)));
        // if (next_button != null)
        //     next_button.interactable = value >= (price);
        // ammountText.text = textName + " - " + value.ToString() + "/" + max.ToString();
        // ammountSlider.maxValue = 1;
        // ammountSlider.value = (value / max).mantissa;
        //Debug.Log((value / max).toString());
        value += increase * mult;
        if (value > max)
        {
            max = new BigNumber(value);
        }
        // try {
        //     auto_button.GetComponent<Button>().interactable = value > (auto_start + BigNumber.Pow(increase * 10, auto_mult));
        //     if (prevSlider != null)
        //         plus_button.GetComponent<Button>().interactable = prevSlider.GetComponent<SliderController>().value > 10000;
        // } catch {}
    }

    // Update is called once per frame
    public override bool BulkTick(BigNumber ticks)
    {
        value += ticks * increase * mult;
        if (value > max)
        {
            max = new BigNumber(value);
        }
        return true;
    }

    public override void UpdateDisplay()
    {
        mult_button.interactable = value > (mult_start + Mathf.Pow(mult, mult_mult));
        if (next_button != null)
            next_button.interactable = value >= (price);
        ammountText.text = textName + " - " + value.ToString() + "/" + max.ToString();
        ammountSlider.maxValue = 1;
        ammountSlider.value = (value / max).mantissa;
        try
        {
            auto_button.GetComponent<Button>().interactable = value > (auto_start + BigNumber.Pow(increase * 10, auto_mult));
            if (prevSlider != null)
                plus_button.GetComponent<Button>().interactable = prevSlider.GetComponent<SliderController>().value > 10000;
        }
        catch { }
    }

    public override void Demo()
    {
        // ammountText.text = value.ToString() + "/" + max.ToString();
        // ammountSlider.maxValue = 1;
        // ammountSlider.value = (value / max).mantissa;
        value += Random.value * 1000;
        if (value > max)
        {
            value = new BigNumber(0);
        }
    }

    public override void SetName(string name)
    {
        if (!is_first)
        {
            Destroy(plus_button);
            Destroy(auto_button);
            gameObject.GetComponent<AutoController>().BtnsUpdate();
        }
        textName = name;
    }

    public void IncreaseOnce()
    {
        IncreaseGen();
        if (prevSlider != null)
            prevSlider.GetComponent<SliderController>().value -= 10000;
    }

    public void IncreaseGen()
    {
        ammountSlider.maxValue = 1;
        ammountSlider.value = (value / max).mantissa;
        value += mult * 10;
        if (value > max)
        {
            max = new BigNumber(value);
        }
    }

    public void UpgradeMult()
    {
        value -= mult_start + BigNumber.Pow(mult, mult_mult);
        mult += 1;
    }

    public void UpgradeMuls()
    {
        mult += 10;
    }

    public void BuyMuls()
    {
        mult += 10;
    }

    public void Buy(int ammnt)
    {
        value -= ammnt;
    }

    public void Buy(BigNumber ammnt)
    {
        value -= ammnt;
    }

    public void UpgradeAuto()
    {
        value -= auto_start + BigNumber.Pow(increase * 10, auto_mult);
        increase += 0.01f;
    }
    public override void Prestige()
    {
        mult = 0;
        value = new BigNumber(0);
        max = new BigNumber(10000);
        increase = 0;
        if (id == 1)
        {
            increase = 1f;
        }
        mult = 1;
    }
    public override int Val()
    {
        return (int)Mathf.Floor(id * max.exponent_little * 2);
    }
}
