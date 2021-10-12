using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderController : GenericController
{
    /// <summary>
    /// the name of the slider
    /// </summary>
    public string textName = "Null";

    /// <summary>
    /// the price of the next slider
    /// </summary>
    public int price = 10000;

    /// <summary>
    /// the max value the slider has shown
    /// </summary>
    /// <returns></returns>
    public BigNumber max = new BigNumber(10000);

    /// <summary>
    /// the ammount the slider increases per tick
    /// </summary>
    public float increase = 0;

    /// <summary>
    /// data for the increase button
    /// </summary>
    public int mult = 1, mult_mult = 10, mult_start = 1;

    /// <summary>
    /// data for the g button
    /// </summary>
    public int auto_mult = 4, auto_start = 500;

    /// <summary>
    /// the text that shows the ammount on the slider
    /// </summary>
    public Text ammountText;

    /// <summary>
    /// the slider that shows the ammount
    /// </summary>
    public Slider ammountSlider;

    /// <summary>
    /// the increase button
    /// </summary>
    public Button mult_button;

    /// <summary>
    /// the button that unclocks the next slider
    /// </summary>
    public Button next_button;

    /// <summary>
    /// the plus button
    /// </summary>
    public GameObject plus_button;

    /// <summary>
    /// the g button
    /// </summary>
    public GameObject auto_button;

    /// <summary>
    /// the previous module
    /// </summary>
    public GameObject prevSlider;

    /// <summary>
    /// is this the first module?
    /// </summary>
    public bool is_first = true;

    /// <summary>
    /// the value of the slider
    /// </summary>
    public BigNumber value = new BigNumber(0);

    /// <summary>
    /// loads a save
    /// </summary>
    /// <param name="save">the save data</param>
    public override void LoadSave(string save)
    {
        string[] data = save.Split(',');
        mult = int.Parse(data[0]);
        increase = float.Parse(data[1]);
        max.mantissa = float.Parse(data[2]);
        max.exponent_big = int.Parse(data[3]);
        max.exponent_little = int.Parse(data[4]);
        value.mantissa = float.Parse(data[5]);
        value.exponent_big = int.Parse(data[6]);
        value.exponent_little = int.Parse(data[7]);
    }

    /// <summary>
    /// creates the save data
    /// </summary>
    /// <returns>the save data</returns>
    public override string saveData()
    {
        string data = "";
        data += $"{mult},";
        data += $"{increase},";
        data += $"{max.mantissa},";
        data += $"{max.exponent_big},";
        data += $"{max.exponent_little},";
        data += $"{value.mantissa},";
        data += $"{value.exponent_big},";
        data += $"{value.exponent_little}";
        return data;
    }

    /// <summary>
    /// setup the modules variables
    /// </summary>
    /// <param name="id"></param>
    /// <param name="sliders"></param>
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

    /// <summary>
    /// processes a single tick
    /// </summary>
    public override void Tick()
    {
        value += increase * mult;
        if (value > max)
        {
            max = new BigNumber(value);
        }
    }

    /// <summary>
    /// processes multiple ticks
    /// </summary>
    /// <param name="ticks">the amount of ticks to process</param>
    /// <returns>true if successful</returns>
    public override bool BulkTick(BigNumber ticks)
    {
        value += ticks * increase * mult;
        if (value > max)
        {
            max = new BigNumber(value);
        }
        return true;
    }

    /// <summary>
    /// update the ui to reflect the current state of the slider
    /// </summary>
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

    /// <summary>
    /// demo mode for the main menu
    /// </summary>
    public override void Demo()
    {
        value += Random.value * 1000;
        if (value > max)
        {
            value = new BigNumber(0);
        }
    }

    /// <summary>
    /// sets the name of the slider
    /// </summary>
    /// <param name="name">the new name</param>
    public override void SetName(string name)
    {
        if (!is_first)
        {
            Destroy(plus_button);
            Destroy(auto_button);
        }
        textName = name;
    }

    /// <summary>
    /// increase the currency at the cost of the previous
    /// </summary>
    public void IncreaseOnce()
    {
        IncreaseGen();
        if (prevSlider != null)
            prevSlider.GetComponent<SliderController>().value -= 10000;
    }

    /// <summary>
    /// increase the currency weighted
    /// </summary>
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

    /// <summary>
    /// buys a multiplier
    /// </summary>
    public void BuyMuls()
    {
        mult += 1;
    }

    /// <summary>
    /// the > button
    /// </summary>
    public void UpgradeMult()
    {
        value -= mult_start + BigNumber.Pow(mult, mult_mult);
        mult += 1;
    }

    /// <summary>
    /// decreases value
    /// </summary>
    /// <param name="ammnt">the ammount you spent</param>
    public void Buy(int ammnt)
    {
        value -= ammnt;
    }

    /// <summary>
    /// decreases value
    /// </summary>
    /// <param name="ammnt">the ammount you spent</param>
    public void Buy(BigNumber ammnt)
    {
        value -= ammnt;
    }

    /// <summary>
    /// upgrade generation
    /// </summary>
    public void UpgradeAuto()
    {
        value -= auto_start + BigNumber.Pow(increase * 10, auto_mult);
        increase += 0.01f;
    }

    /// <summary>
    /// process a prestige event
    /// </summary>
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

    /// <summary>
    /// gets the value of the slider
    /// </summary>
    /// <returns>the value</returns>
    public override int Val()
    {
        return (int)Mathf.Floor(id * max.exponent_little * 2);
    }

    /// <summary>
    /// the type of module
    /// </summary>
    public override string typeName => "slider";
}
