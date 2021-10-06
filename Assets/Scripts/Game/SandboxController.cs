using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SandboxController : GameController
{
    public GameObject promptPrefab;

    public override void SaveThing() { Saves.savePath = "/sandbox.dat"; }

    public override void AddNext(int id)
    {
        if (id > 3)
        {
            prompt(id);
        }
        else if (id == 3)
        {
            AddType(id, 1);
        }
        else
        {
            AddType(id, 0);
        }
    }

    public void prompt(int id)
    {
        GameObject pre = Instantiate(promptPrefab);
        pre.transform.parent = transform.parent;
        GameObject choice = GameObject.FindGameObjectsWithTag("Choice")[0];
        choice.GetComponent<Choice>().id = id;
        choice.GetComponent<Choice>().sb = this;
        choice.transform.localScale = new Vector3(1, 1, 1);
    }

    public override void Leaderboard() { }
}
