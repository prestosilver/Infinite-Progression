using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AutoController : MonoBehaviour
{
    public int level;
    public int type = 0;
    public AutoVis vis;
    public List<Button> btns;
    public bool active = false;

    private float seconds = 6;
    private float progress;

    // Update is called once per frame
    public void Update()
    {
        seconds = 10 * Mathf.Pow(2, -level);
        if (active)
            progress += Time.deltaTime;
        if (vis != null) {
            vis.SetPValue(progress);
            vis.SetPMaxValue(seconds);
            string title = "Unlock";
            if (active) {
                title =  "Level ";
                title += level;
                title += " - ";
                title += progress;
                title += " / ";
                title += seconds;
            }
            vis.SetText(title);
            vis.cont = this;
            int pres = GameObject.FindGameObjectsWithTag("AutoController")[0].GetComponent<AutobuyersController>().presLevel;
            vis.upgradeBtn.interactable = pres >= level + 1;
            vis.downgradeBtn.interactable = active;
        }
        if (progress > seconds) {
            progress = 0;
            Buy();
        }
    }

    public void BtnsUpdate() {
        btns = Lol.FindComponentsInChildrenWithTag<Button>(gameObject, "Button");
    }

    void Buy()
    {
        try {
            Button btn = btns[Random.Range(0, btns.Count - 1)];
            if (btn.interactable)
               btn.onClick.Invoke();
        }
        catch {
            BtnsUpdate();
            Buy();
        }
    }

    public void Upgrade() {
        GameObject.FindGameObjectsWithTag("AutoController")[0].GetComponent<AutobuyersController>().presLevel -= 1;
        if (active == false){
            active = true;
            GameObject.FindGameObjectsWithTag("AutoController")[0].GetComponent<AutobuyersController>().UpdateText();
            return;
        }
        level += 1;
        GameObject.FindGameObjectsWithTag("AutoController")[0].GetComponent<AutobuyersController>().presLevel -= (level);
        GameObject.FindGameObjectsWithTag("AutoController")[0].GetComponent<AutobuyersController>().UpdateText();
    }

    public void Downgrade() {
        GameObject.FindGameObjectsWithTag("AutoController")[0].GetComponent<AutobuyersController>().presLevel += 1;
        if (level == 0){
            active = false;
        GameObject.FindGameObjectsWithTag("AutoController")[0].GetComponent<AutobuyersController>().UpdateText();
            return;
        }
        level -= 1;
        GameObject.FindGameObjectsWithTag("AutoController")[0].GetComponent<AutobuyersController>().presLevel += (level + 1);
        GameObject.FindGameObjectsWithTag("AutoController")[0].GetComponent<AutobuyersController>().UpdateText();
    }
}

public static class Lol {
    public static List<T> FindComponentsInChildrenWithTag<T>(this GameObject parent, string tag, List<T> list = null )where T:Component{
        if (list == null)
            list = new List<T>();
        Transform t = parent.transform;
        foreach(Transform tr in t) {
            if(tr.tag == tag)
            {
                list.Add(tr.GetComponent<T>());
            }
            else
            {
                List<T> l = new List<T>();
                FindComponentsInChildrenWithTag(tr.gameObject, tag, l);
                foreach (T thing in l) {
                   list.Add(thing);
                }
            }
        }
        return list;
    }
}
