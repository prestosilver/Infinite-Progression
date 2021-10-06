using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AutobuyersController : MonoBehaviour
{
    public GameObject autoPrefab, parent;
    public List<GameObject> sliders;
    public int slider_ammnt;
    public int presLevel;
    public Text levelText;

    public GameController cont;

    public void Start() {
        cont = GameObject.FindGameObjectsWithTag("GameController")[0].GetComponent<GameController>();
        Application.targetFrameRate = 60;
        byte[] save = Saves.Load();
        slider_ammnt = (int)Saves.FindInt(save, 0);
        presLevel = (int)Mathf.Pow((int)Saves.FindInt(save, 4), 2);
        for (int i = 1; i < slider_ammnt + 1; i++) {
            AddNext(i);
        }
        int j = 0;
        foreach (GameObject slider in cont.sliders) {
            slider.GetComponent<AutoController>().vis = sliders[j].GetComponent<AutoVis>();
            j ++;
            if (slider.GetComponent<AutoController>().active) {
                int n = slider.GetComponent<AutoController>().level + 1;
                presLevel -= (n * (n+1)) / 2;
            }
        }
        UpdateText();
    }

    public void AddNext(int id) {
        foreach (GameObject g in sliders) {
            g.transform.localPosition -= new Vector3(0, (15 * (id - 1)), 0);
        }
        GameObject slider = Instantiate(autoPrefab);
        slider.transform.parent = parent.transform;
        slider.transform.localScale = new Vector3(1, 1, 1);
        slider.transform.localPosition = new Vector3(0, -10 + (-30 * (id - 1)), 0);
        sliders.Add(slider);
        int rows = 0;
        foreach (GameObject g in sliders) {
            g.transform.localPosition += new Vector3(0, (15 * (id)), 0);
            rows ++;
        }
        var rectTransform = parent.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, 110 + (30 * (rows)));
    }

    public void Back() {
        SceneManager.UnloadSceneAsync("AutoBuyers");
    }

    public void UpdateText() {
        levelText.text = "" + presLevel;
    }
}
