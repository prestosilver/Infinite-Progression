using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InsaneLockController : GenericController
{
    public BigNumber price = new BigNumber(100);
    public Button next_button;
    public Text lockNameText;
    public Slider progressSlider;

    private string lockName;

    public override void LoadSave(byte[] objSave) {}
    public override void Start() {}
    public override bool BulkTick(BigNumber ticks) {return false;}
    public override void Tick() {
    }
    
    public override void Demo() {
        lockName = "A";
    }
    
    public override void SetName(string nname) {
        lockName = nname;
    }

    public override void UpdateDisplay()
    {
        int value = GetValue();
        progressSlider.maxValue = 1;
        progressSlider.value = (new BigNumber(value) / price).mantissa;
        lockNameText.text = "Lock - " + lockName;
        next_button.interactable = value > price;
    }

    public override void SetupVars(int id, List<GameObject> sliders){}
}
