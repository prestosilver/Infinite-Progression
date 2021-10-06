using System.Collections;
using System.Collections.Generic;
using PyMods;
using UnityEngine;
using UnityEngine.UI;

public class ModController : GenericController
{
    public Mod mod;

    private GameObject panel;

    [SerializeField]
    private GameObject panelPrefab, ButtonPrefab;

    private dynamic data;

    public GameObject ConstructUI(ModUI ui)
    {
        GameObject result = Instantiate(panelPrefab, transform);

        foreach (ModUIButton btn in ui.buttons)
        {
            GameObject button = Instantiate(ButtonPrefab, result.transform);
            button.GetComponent<RectTransform>().anchoredPosition = new Vector2(btn.x, btn.y);
            button.GetComponent<RectTransform>().sizeDelta = new Vector2(btn.w, btn.h);
        }

        return result;
    }

    public override void SetupVars(int id, List<GameObject> sliders)
    {
        panel = ConstructUI(mod.UI);
        data = mod.createModule();
    }

    public override bool BulkTick(BigNumber ticks)
    {
        data = mod.BulkTick(data, ticks);
        return true;
    }
}
