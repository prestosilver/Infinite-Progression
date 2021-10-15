using System;
using System.Collections;
using System.Collections.Generic;
using PyMods;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    void Awake() => instance = this;

    public GameObject parent, more_button_text, confirmPrefab, conf;
    public Text presText, TpsText;
    public Slider presBar;
    public List<GameObject> prefabs;
    public List<GameObject> sliders;
    public Button next_button, pres_button;
    public int slider_ammnt;
    public int presLevel;
    public GameObject modPrefab;
    public GameObject dataPrefab;

    public static List<Mod> mods = new List<Mod>();
    public Mod forceNext;

    private bool seeded;
    private int ver = 0;
    private int sum = 0;

    public void Update()
    {
        presBar.value = (float)slider_ammnt / (float)(10 * (presLevel + 1));
        TpsText.text = ConsistantTPS.tps.ToString() + "\nTPS";
        pres_button.interactable = slider_ammnt >= 10 * (presLevel + 1);
    }

    public virtual void SaveThing() { Saves.savePath = "save.dat"; }

    public void Start()
    {
        SaveThing();
        foreach (GameObject i in prefabs)
            sum += i.GetComponent<ProbController>().chance;
        foreach (Mod m in mods)
        {
            sum += m.chance;
        }
        List<string> save = Saves.Read();
        if (save.Count > 0)
        {
            string[] misc = save[0].Split(';');
            presLevel = int.Parse(misc[0]);
            int modid = 1;
            foreach (string line in save.GetRange(1, save.Count - 1))
            {
                if (line.Split(';')[0] == "slider")
                    AddType(modid, 0);
                else
                {
                    int i = 0;
                    foreach (Mod m in mods)
                    {
                        if (m.name == line.Split(';')[0])
                        {
                            AddMod(modid, i);
                            break;
                        }
                        i++;
                    }
                    if (i == mods.Count)
                    {
                        AddMissing(modid, line.Split(';')[0]);
                    }
                }
                sliders[modid - 1].GetComponent<GenericController>().LoadSave(line.Split(';')[1]);
                modid++;
            }
            slider_ammnt = sliders.Count;
            ConsistantTPS.tps = new BigNumber(120);
            ConsistantTPS.tps.mantissa = float.Parse(misc[1]);
            ConsistantTPS.tps.exponent_little = int.Parse(misc[2]);
            ConsistantTPS.tps.exponent_big = int.Parse(misc[3]);
            BigNumber n = new BigNumber(presLevel);
            ConsistantTPS.tps = 120 * ((n * (n + 1) * (2 * n + 1) / 6) + 1);
            presText.text = "" + slider_ammnt + "/" + (10 * (presLevel + 1));
        }
        else
        {
            ConsistantTPS.tps = new BigNumber(120);
        }
    }

    public virtual void AddNext(int id)
    {
        int num = (int)(Mathf.Round(SeededRand.Perlin(100 * id) * (sum)));
        int type = 0;
        if (id <= 2)
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

    public void AddMod(int id, int type)
    {
        int tmpid = 0;
        foreach (GameObject g in sliders)
        {
            g.transform.localPosition -= new Vector3(0, (15 * (id - 1)), 0);
            if (g.GetComponent<SliderController>() != null)
            {
                g.GetComponent<SliderController>().next_button = null;
            }
            tmpid++;
        }
        GameObject slider = Instantiate(modPrefab);
        slider.GetComponent<ModController>().id = id;
        slider.GetComponent<ModController>().mod = mods[type];
        List<String> requires = mods[type].requires;
        if (mods[type].requires.Count != 0)
        {
            foreach (GameObject o in sliders)
            {
                ModController m;
                if (!(m = o.GetComponent<ModController>())) continue;
                if (requires.Contains(m.mod.name))
                {
                    requires.Remove(m.mod.name);
                    if (requires.Count == 0) break;
                }
            }
            if (requires.Count != 0)
            {
                foreach (Mod m in mods)
                    if (m.name == requires[0])
                        slider.GetComponent<ModController>().mod = m;
            }
        }
        if (forceNext != null)
        {
            slider.GetComponent<ModController>().mod = forceNext;
            forceNext = null;
        }
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
            if (g.GetComponent<SliderController>() != null)
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
        var rectTransform = parent.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, 110 + (30 * (rows)));
    }

    /// <summary>
    /// adds a DataController
    /// </summary>
    /// <param name="id">the id of the module</param>
    /// <param name="type">the type of the module</param>
    private void AddMissing(int id, string type)
    {
        int tmpid = 0;
        foreach (GameObject g in sliders)
        {
            g.transform.localPosition -= new Vector3(0, (15 * (id - 1)), 0);
            if (g.GetComponent<SliderController>() != null)
            {
                g.GetComponent<SliderController>().next_button = null;
            }
            tmpid++;
        }
        GameObject slider = Instantiate(dataPrefab);
        slider.GetComponent<DataController>().id = id;
        slider.GetComponent<DataController>().modName = type;
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
            if (g.GetComponent<SliderController>() != null)
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
            tmpid++;
        }
        if (id <= 2)
            type = 0;
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
            if (g.GetComponent<SliderController>() != null)
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

    public static GenericController GetRandOf(string kind, int defaultId, int before)
    {
        GameObject result;
        int rid = 0;
        if (kind == "slider")
        {
            do
            {
                result = instance.sliders[(int)(SeededRand.Perlin(100 * before + 100 * defaultId + 1 + rid) * (before - 1))];
                rid += 1;
                if (rid > 100)
                {
                    result = instance.sliders[defaultId];
                    break;
                }
            } while (result.GetComponent<SliderController>() == null);
        }
        else
        {
            string name = "null";
            do
            {
                result = instance.sliders[(int)(SeededRand.Perlin(100 * before + 100 * defaultId + 1 + rid) * (before - 1))];
                rid += 1;
                name = "null";
                if (result.GetComponent<ModController>())
                    name = result.GetComponent<ModController>().mod.name;
                if (rid > 100)
                {
                    result = instance.sliders[defaultId];
                    break;
                }
            } while (name != kind);
        }
        return result.GetComponent<GenericController>();
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
        Saves.Save();
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

    public static SliderController GetSlider(int id)
    {
        return instance.sliders[id].GetComponent<SliderController>();
    }

    public static dynamic GetData(int id)
    {
        return instance.sliders[id].GetComponent<ModController>().data;
    }

    public static void SetChance(string name, int chance)
    {
        foreach (Mod mod in mods)
        {
            if (mod.name != name) continue;
            if (chance == -1)
            {
                instance.forceNext = mod;
                break;
            }
            instance.sum -= mod.chance;
            instance.sum += chance;
            mod.chance = chance;
            break;
        }
    }

    public static List<String> GetModList()
    {
        List<string> result = new List<String>();

        foreach (Mod mod in mods)
        {
            result.Add(mod.name);
        }

        return result;
    }

    public static List<GenericController> GetAllOf(string name)
    {
        List<GenericController> result = new List<GenericController>();
        foreach (GameObject obj in instance.sliders)
        {
            if (obj.GetComponent<GenericController>().typeName == name) result.Add(obj.GetComponent<GenericController>());
        }
        return result;
    }

    public static int GetChance(string module)
    {
        if (module == "slider")
        {
            return instance.prefabs[0].GetComponent<ProbController>().chance;
        }

        foreach (Mod mod in mods)
        {
            if (mod.name == module) return mod.chance;
        }
        return -2;
    }

    public static void SwapType(int id, string type)
    {
        if (type == "Slider")
        {
            return;
        }
        ModController cont = instance.sliders[id].GetComponent<ModController>();
        foreach (Mod mod in mods)
        {
            if (mod.name == type)
                cont.mod = mod;
            cont.Setup(id, instance.sliders, SeededRand.Word(100 * id + 1), instance.sliders[id - 1]);
        }
    }
}
