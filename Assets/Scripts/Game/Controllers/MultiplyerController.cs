using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MultiplyerController : GenericController
{
    public GameObject buys, muls;
    public Text Name;
    public Button upgrade_button;
    public int level;
    public float discount = 1;

    public override void LoadSave(byte[] objSave)
    {
        level = Saves.FindInt(objSave, 4);
        discount = Saves.FindInt(objSave, 4);
    }
    public override void SetupVars(int nid, List<GameObject> sliders)
    {
        int rid = 0;
        do
        {
            buys = sliders[(int)(SeededRand.Perlin(100 * nid + 1 + rid) * (nid - 1))];
            rid += 1;
            if (rid > 1000)
            {
                buys = sliders[0];
                break;
            }
        } while (buys.GetComponent<SliderController>() == null);
        rid = 0;
        do
        {
            muls = sliders[(int)(SeededRand.Perlin(100 * nid + 1 + rid) * (nid - 1))];
            rid += 1;
            if (rid > 1000)
            {
                buys = sliders[0];
                muls = sliders[1];
                break;
            }
        } while (muls.GetComponent<SliderController>() == null || buys == muls);
        Name.text = buys.GetComponent<SliderController>().textName;
        Name.text += " * ";
        Name.text += muls.GetComponent<SliderController>().textName;
        discount = 1;
    }

    public void Upgrade()
    {
        muls.GetComponent<SliderController>().BuyMuls();
        buys.GetComponent<SliderController>().Buy((int)Mathf.Ceil(500 * level * discount));
        level += 1;
    }

    public override void UpdateDisplay()
    {
        if (buys != null)
            upgrade_button.interactable = (int)Mathf.Ceil(500 * level * discount) < buys.GetComponent<SliderController>().value;
    }

    public override bool BulkTick(BigNumber ammnt)
    {
        return true;
    }

    public void BuyDiscount()
    {
        discount -= discount * 0.1f;
    }
    public override void Prestige()
    {
        level = 0;
        discount = 1;
    }
}
