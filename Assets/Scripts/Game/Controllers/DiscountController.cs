using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiscountController : GenericController
{
    public GameObject buys, discounts;
    public Text Name, dc;
    public Button upgrade_button;

    public int level;
    private float dcvalue;

    public override void LoadSave(byte[] objSave)
    {
        level = Saves.FindInt(objSave, 4);
    }

    public override void SetupVars(int id, List<GameObject> sliders){
        int rid = 0;
        do {
            buys = sliders[(int)(SeededRand.Perlin(100 * id + 1 + rid) * (id - 1))];
            rid += 1;
            if (rid > 100) {
                buys = sliders[1];
                break;
            }
        } while(buys.GetComponent<SliderController>() == null);
        do {
            discounts = sliders[(int)(SeededRand.Perlin(100 * id + 1 + rid) * (id - 1))];
            rid += 1;
            if (rid > 100) {
                discounts = sliders[2];
                break;
            }
        } while(discounts.GetComponent<MultiplyerController>() == null || buys == discounts);
        Name.text = buys.GetComponent<SliderController>().textName;
        Name.text += "/M";
        Name.text += discounts.GetComponent<MultiplyerController>().id;
    }

    public void Upgrade() {
        discounts.GetComponent<MultiplyerController>().BuyDiscount();
        buys.GetComponent<SliderController>().Buy(100000 * level);
        level += 1;
    }

    public override void UpdateDisplay() {
        dcvalue = discounts.GetComponent<MultiplyerController>().discount * 100;
        upgrade_button.interactable = ((100000 * level) < buys.GetComponent<SliderController>().value);
        dc.text = "" + Mathf.Floor(dcvalue * 1) / 100 + "%";
    }

    public override void Demo() {
        dcvalue =  (UnityEngine.Random.value * 100);
    }

    public override void Tick() {}

    public override bool BulkTick(BigNumber n) {return true;}

    public override void Prestige() {
        level = 0;
    }
}
