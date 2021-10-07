using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Choice : MonoBehaviour
{
    public List<GameObject> prefabs, btns;
    public GameObject ButtonPrefab, parent;
    public int id;
    public SandboxController sb;

    // Start is called before the first frame update
    public void Start()
    {
        // int cnt = 0;
        // foreach (GameObject p in prefabs)
        // {
        //     bool hasGen = false;
        //     foreach (GameObject s in sb.sliders)
        //         if (s.GetComponent<GeneratorController>() != null) hasGen = true;
        //     if (!(cnt == 4 && !hasGen))
        //     {
        //         GameObject btn = Instantiate(ButtonPrefab);
        //         GameObject slider = Instantiate(p);
        //         slider.transform.parent = btn.transform;
        //         slider.transform.SetSiblingIndex(0);
        //         btn.transform.parent = parent.transform;
        //         slider.GetComponent<GenericController>().demo = true;
        //         btn.transform.localPosition = new Vector3(0, -30 + (cnt * -30), 0);
        //         slider.transform.localScale = new Vector3(1, 1, 1);
        //         btn.transform.localScale = new Vector3(1, 1, 1);
        //         int nope = cnt;
        //         GameObject oml = gameObject;
        //         btn.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() => sb.AddType(id, nope));
        //         btn.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() => Destroy(oml));
        //         btns.Add(btn);
        //     }
        //     cnt++;
        // }
    }
}
