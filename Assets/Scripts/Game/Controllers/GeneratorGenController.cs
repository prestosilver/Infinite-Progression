using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GeneratorGenController : GenericController
{
    public GameObject buys;
    public Text Name;
    public Button upgrade_button;
    public Slider ammountSlider;
    public int value = 0, max = 500;
    public int level = 0;
    public BigNumber ammnt;
    public SliderController upgrades;

    private bool gen = false;

    public override void LoadSave(byte[] objSave)
    {
        level = Saves.FindInt(objSave, 4);
    }
    public override void SetupVars(int id, List<GameObject> sliders){
        int prevgen = 0;
        foreach (GameObject g in sliders)
        {
            if (g.GetComponent<GeneratorController>() != null) {
                prevgen = g.GetComponent<GeneratorController>().id - 1;
            }
        }
        int rid = 0;
        do {
            buys = sliders[(int)(SeededRand.Perlin(100 * id + 1 + rid) * (id - 1))];
            rid += 1;
            if (rid > 100) {
                buys = sliders[prevgen];
                break;
            }
        } while(buys.GetComponent<GeneratorController>() == null
            && buys.GetComponent<GeneratorGenController>() == null);
        gen = buys.GetComponent<GeneratorGenController>() != null;
        if (gen)
            Name.text = buys.GetComponent<GeneratorGenController>().Name.text;
        else
            Name.text = buys.GetComponent<GeneratorController>().Name.text;
        Name.text += " += 0";
        ammnt = GetAmmnt(buys);
        upgrades = GetUpgrades(buys);
    }

    private SliderController GetUpgrades(GameObject g) {
        if (g.GetComponent<GeneratorGenController>() != null) {
            return GetUpgrades(g.GetComponent<GeneratorGenController>().buys);
        } else {
            return buys.GetComponent<GeneratorController>().buys.GetComponent<SliderController>();
        }
    }
    private BigNumber GetAmmnt(GameObject g) {
        if (g.GetComponent<GeneratorGenController>() != null) {
            return GetAmmnt(g.GetComponent<GeneratorGenController>().buys) * 100;
        } else {
            return new BigNumber(10000);
        }
    }

    public void Upgrade() {
        upgrades.Buy(ammnt * level);
        level += 1;
    }

    public override void Tick() {
        value += level;
        if (max < value) {
            value -= max;
            for (int x = 0; x<level; x++) {
                if (gen)
                    buys.GetComponent<GeneratorGenController>().value += level;
                else
                    buys.GetComponent<GeneratorController>().value += level;
            }
        }
        upgrade_button.interactable = upgrades.GetComponent<SliderController>().value > (ammnt * level);
    }
    public override void Demo() {
        value += 2;
        Name.text = "" + value + "/" + max;
        if (max < value) {
            value = 0;
        }
    }

    public override void UpdateDisplay()
    {
        Name.text = "A$ += 15.6K += 105";
        if (buys != null) {
            if (gen)
                Name.text = buys.GetComponent<GeneratorGenController>().Name.text;
            else
                Name.text = buys.GetComponent<GeneratorController>().Name.text;
            Name.text += " += ";
            Name.text += new BigNumber(level * level).ToString();
        }
        ammountSlider.maxValue = 1;
        ammountSlider.value = ((float)value / (float)max);
    }

    public override void Prestige() {
        level = 0;
    }
}
