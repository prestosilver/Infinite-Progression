using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GeneratorController : GenericController
{
    public GameObject buys;
    public Text Name;
    public Button upgrade_button;
    public Slider ammountSlider;
    public BigNumber value = new BigNumber(0), max = new BigNumber(500);

    public int level;

    public override void LoadSave(byte[] objSave)
    {
        level = Saves.FindInt(objSave, 4);
    }

    public override void SetupVars(int id, List<GameObject> sliders)
    {
        int rid = 0;
        do
        {
            buys = sliders[(int)(SeededRand.Perlin(100 * id + 1 + rid) * (id - 1))];
            rid += 1;
            if (rid > 100)
            {
                buys = sliders[0];
                break;
            }
        } while (buys.GetComponent<SliderController>() == null);
        Name.text = buys.GetComponent<SliderController>().textName;
        Name.text += " += 0";
    }

    public void Upgrade()
    {
        buys.GetComponent<SliderController>().Buy(100000 * level);
        level += 1;
    }

    public override void Tick()
    {
        value += level;
        while (max < value)
        {
            value -= max;
            if (buys == null) continue;
            for (int x = 0; x < level; x++)
                buys.GetComponent<SliderController>().IncreaseGen();
        }
    }
    public override bool BulkTick(BigNumber n)
    {
        value += n * level;
        while (max < value)
        {
            value -= max;
            if (buys == null) continue;
            for (int x = 0; x < level; x++)
                buys.GetComponent<SliderController>().IncreaseGen();
        }
        return true;
    }

    public override void Demo()
    {
        level = 1;
        value += 2;
        if (max < value)
        {
            value = new BigNumber(0);
        }
    }

    public override void UpdateDisplay()
    {
        Name.text = "A$ += 15.6K";
        if (buys != null)
        {
            Name.text = buys.GetComponent<SliderController>().textName;
            Name.text += " += ";
            Name.text += (new BigNumber(level) * level * buys.GetComponent<SliderController>().mult).ToString();
            upgrade_button.interactable = ((new BigNumber(100000) * level) < buys.GetComponent<SliderController>().value);
        }
        if (level * ConsistantTPS.tps > 50 * 30)
        {
            ammountSlider.maxValue = 1;
            ammountSlider.value = 1;
        }
        else
        {
            ammountSlider.maxValue = 1;
            ammountSlider.value = (value / max).mantissa;
        }
    }

    public override void Prestige()
    {
        level = 0;
        value = new BigNumber(0);
    }
}
