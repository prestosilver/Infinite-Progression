using System;
using System.Collections;
using System.Collections.Generic;
using PyMods;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject parent, more_button_text, confirmPrefab, conf;
    public Text presText, TpsText;
    public Slider presBar;
    public List<GameObject> prefabs;
    public List<GameObject> sliders;
    public Button next_button, pres_button;
    public int slider_ammnt;
    public int presLevel;
    public GameObject modPrefab;

    public static List<Mod> mods = new List<Mod>();

    private bool seeded;
    private int ver = 0;
    private int sum = 0;

    public void Update()
    {
        presBar.value = (float)slider_ammnt / (float)(10 * (presLevel + 1));
        TpsText.text = ConsistantTPS.tps.ToString() + "\nTPS";
        pres_button.interactable = slider_ammnt >= 10 * (presLevel + 1);
    }

    public virtual void SaveThing() { Saves.savePath = "/save.dat"; }

    public void Start()
    {
        SaveThing();
        foreach (GameObject i in prefabs)
            sum += i.GetComponent<ProbController>().chance;
        foreach (Mod m in mods)
        {
            Debug.Log("" + m.name + ": " + m.chance);
            sum += m.chance;
        }
        byte[] save = Saves.Load();
        slider_ammnt = (int)Saves.FindInt(save, 0);
        presLevel = (int)Saves.FindInt(save, 4);
        SeededRand.seed = (int)Saves.FindInt(save, 8);
        seeded = Saves.FindBool(save, 12);
        ver = Saves.FindVer(save);
        int head = Saves.GetHeadSize(save);
        for (int i = 1; i < slider_ammnt + 1; i++)
        {
            if (ver <= 1)
            {
                AddNext(i);
            }
            else
            {
                AddType(i, Saves.FindInt(save, head + ((i - 1) * Saves.singleObjectSize)));
            }
        }
        int j = 0;
        foreach (GameObject slider in sliders)
        {
            slider.GetComponent<GenericController>().LoadSave(Saves.FindObject(save, head + (j * Saves.singleObjectSize)));
            if (slider.GetComponent<AutoController>() != null)
            {
                AutoController ac = slider.GetComponent<AutoController>();
                ac.level = Saves.FindInt(save, head + 36 + (j * Saves.singleObjectSize));
                ac.active = Saves.FindBool(save, head + 40 + (j * Saves.singleObjectSize));
            }
            j++;
        }
        ConsistantTPS.tps = new BigNumber(120);
        if (ver >= 3)
        {
            ConsistantTPS.tps.mantissa = Saves.FindFloat(save, 16);
            ConsistantTPS.tps.exponent_little = Saves.FindInt(save, 20);
            ConsistantTPS.tps.exponent_big = Saves.FindInt(save, 24);
        }
        BigNumber n = new BigNumber(presLevel);
        ConsistantTPS.tps = 120 * ((n * (n + 1) * (2 * n + 1) / 6) + 1);
        presText.text = "" + slider_ammnt + "/" + (10 * (presLevel + 1));
    }

    public virtual void AddNext(int id)
    {
        int num = (int)(Mathf.Round(SeededRand.Perlin(100 * id) * (sum)));
        int type = 0;
        if (id <= 3)
        {
            AddType(id, type);
            return;
        }
        foreach (GameObject i in prefabs)
        {
            if ((num -= (i.GetComponent<ProbController>().chance)) < 0)
            {
                AddType(id, type);
                return;
            }
            type++;
        }
        type = 0;
        foreach (Mod m in mods)
        {
            if ((num -= (m.chance)) < 0)
            {
                AddMod(id, type);
                return;
            }
            type++;
        }
    }

    private void AddMod(int id, int type)
    {
        bool gencont = false;
        int tmpid = 0;
        int gen;
        foreach (GameObject g in sliders)
        {
            g.transform.localPosition -= new Vector3(0, (15 * (id - 1)), 0);
            if (g.GetComponent<SliderController>() != null)
            {
                g.GetComponent<SliderController>().next_button = null;
            }
            else if (g.GetComponent<LockController>() != null)
            {
                g.GetComponent<LockController>().next_button = null;
            }
            else if (g.GetComponent<GeneratorController>() != null)
            {
                gencont = true;
                gen = tmpid;
            }
            tmpid++;
        }
        GameObject slider = Instantiate(modPrefab);
        slider.GetComponent<ModController>().mod = mods[type];
        GenericController cont = (GenericController)slider.GetComponents(typeof(GenericController))[0];
        GameObject nprev = null;
        if (sliders.Count != 0)
            nprev = sliders[sliders.Count - 1];
        cont.Setup(id, sliders, SeededRand.Word(100 * id + 1), nprev);
        slider.transform.parent = parent.transform;
        slider.transform.localScale = new Vector3(1, 1, 1);
        slider.transform.localPosition = new Vector3(0, -10 + (-30 * (id - 1)), 0);
        sliders.Add(slider);
        GameObject prev = null, last = null;
        int mult = 2;
        int amult = 10;
        int cost_mul = 1;
        int rows = 0;
        foreach (GameObject g in sliders)
        {
            g.transform.localPosition += new Vector3(0, (15 * (id)), 0);
            if (g.GetComponent<SliderController>() != null)
            {
                g.GetComponent<SliderController>().prevSlider = prev;
                g.GetComponent<SliderController>().mult_mult = mult;
                g.GetComponent<SliderController>().auto_mult = amult;
                mult += 1;
                amult += 10;
                prev = g;
                cost_mul = 0;
            }
            if (g.GetComponent<SliderController>() != null || g.GetComponent<LockController>() != null || g.GetComponent<InsaneLockController>() != null)
            {
                last = g;
            }
            rows += 1;
            cost_mul += 1;
        }
        if (last.GetComponent<SliderController>() != null)
        {
            more_button_text.GetComponent<Text>().text = "Unlock Next";
            more_button_text.GetComponent<Text>().text += " - " + new BigNumber(cost_mul * 10000).ToString() + " ";
            more_button_text.GetComponent<Text>().text += last.GetComponent<SliderController>().textName;
            last.GetComponent<SliderController>().price = Mathf.Max(cost_mul * 10000, 5000);
            last.GetComponent<SliderController>().next_button = next_button;
        }
        else if (last.GetComponent<LockController>() != null)
        {
            more_button_text.GetComponent<Text>().text = "Unlock Next";
            more_button_text.GetComponent<Text>().text += " - Lock ";
            more_button_text.GetComponent<Text>().text += last.GetComponent<LockController>().price;
            last.GetComponent<LockController>().next_button = next_button;
        }
        var rectTransform = parent.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, 110 + (30 * (rows)));
    }

    public void AddType(int id, int type)
    {
        bool gencont = false;
        int tmpid = 0;
        int gen;
        foreach (GameObject g in sliders)
        {
            g.transform.localPosition -= new Vector3(0, (15 * (id - 1)), 0);
            if (g.GetComponent<SliderController>() != null)
            {
                g.GetComponent<SliderController>().next_button = null;
            }
            else if (g.GetComponent<LockController>() != null)
            {
                g.GetComponent<LockController>().next_button = null;
            }
            else if (g.GetComponent<GeneratorController>() != null)
            {
                gencont = true;
                gen = tmpid;
            }
            tmpid++;
        }
        if (id <= 2)
            type = 0;
        if (id == 3)
            type = 1;
        if (!gencont && type == 4)
            type = 2;
        GameObject slider = Instantiate(prefabs[type]);
        GenericController cont = (GenericController)slider.GetComponents(typeof(GenericController))[0];
        GameObject nprev = null;
        if (sliders.Count != 0)
            nprev = sliders[sliders.Count - 1];
        cont.Setup(id, sliders, SeededRand.Word(100 * id + 1), nprev);
        slider.transform.parent = parent.transform;
        slider.transform.localScale = new Vector3(1, 1, 1);
        slider.transform.localPosition = new Vector3(0, -10 + (-30 * (id - 1)), 0);
        sliders.Add(slider);
        GameObject prev = null, last = null;
        int mult = 2;
        int amult = 10;
        int cost_mul = 1;
        int rows = 0;
        foreach (GameObject g in sliders)
        {
            g.transform.localPosition += new Vector3(0, (15 * (id)), 0);
            if (g.GetComponent<SliderController>() != null)
            {
                g.GetComponent<SliderController>().prevSlider = prev;
                g.GetComponent<SliderController>().mult_mult = mult;
                g.GetComponent<SliderController>().auto_mult = amult;
                mult += 1;
                amult += 10;
                prev = g;
                cost_mul = 0;
            }
            if (g.GetComponent<SliderController>() != null || g.GetComponent<LockController>() != null || g.GetComponent<InsaneLockController>() != null)
            {
                last = g;
            }
            rows += 1;
            cost_mul += 1;
        }
        if (last.GetComponent<SliderController>() != null)
        {
            more_button_text.GetComponent<Text>().text = "Unlock Next";
            more_button_text.GetComponent<Text>().text += " - " + new BigNumber(cost_mul * 10000).ToString() + " ";
            more_button_text.GetComponent<Text>().text += last.GetComponent<SliderController>().textName;
            last.GetComponent<SliderController>().price = Mathf.Max(cost_mul * 10000, 5000);
            last.GetComponent<SliderController>().next_button = next_button;
        }
        else if (last.GetComponent<LockController>() != null)
        {
            more_button_text.GetComponent<Text>().text = "Unlock Next";
            more_button_text.GetComponent<Text>().text += " - Lock ";
            more_button_text.GetComponent<Text>().text += last.GetComponent<LockController>().price;
            last.GetComponent<LockController>().next_button = next_button;
        }
        var rectTransform = parent.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, 110 + (30 * (rows)));
    }

    public void BuySlider()
    {
        slider_ammnt += 1;
        presText.text = "" + slider_ammnt + "/" + (10 * (presLevel + 1));
        Leaderboard();
        AddNext(slider_ammnt);
        Save();
    }

    public virtual void Leaderboard()
    {
        if (seeded)
        {
            PlayGamesScript.AddScoreToLeaderboard(GPGSIds.leaderboard_maximum_tiers_unlocked_set_seed, (long)slider_ammnt);
        }
        else
        {
            PlayGamesScript.AddScoreToLeaderboard(GPGSIds.leaderboard_maximum_tiers_unlocked_random_seed, (long)slider_ammnt);
        }
    }

    public void PrestigePrompt()
    {
        conf = Instantiate(confirmPrefab);
        GameObject confirm = GameObject.FindGameObjectsWithTag("ConfirmButton")[0];
        confirm.GetComponent<Button>().onClick.AddListener(Prestige);
    }

    void Prestige()
    {
        Destroy(conf);
        sliders[sliders.Count - 1].GetComponent<GenericController>().DoPrestige();
        presLevel += 1;
        // more_button_text.GetComponent<Text>().text = "Unlock";
        BigNumber n = new BigNumber(presLevel);
        ConsistantTPS.tps = 120 * ((n * (n + 1) * (2 * n + 1) / 6) + 1);
        if (ConsistantTPS.tps < 120)
        {
            ConsistantTPS.tps = new BigNumber(120);
        }
    }

    public void Save()
    {
        SaveThing();
        byte[] save = Saves.SlidersToByteArray(sliders, slider_ammnt, presLevel, SeededRand.seed, seeded);
        Saves.Save(save);
    }

    public void Home()
    {
        Save();
        SceneManager.LoadSceneAsync("MainMenu");
    }

    public void Auto()
    {
        Save();
        SceneManager.LoadScene("AutoBuyers", LoadSceneMode.Additive);
    }
}
