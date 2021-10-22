using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ModalWindowSpawner
{
    public static ModalWindowSpawner instance = new ModalWindowSpawner();
    public GameObject prefab = null;

    private ModalWindowSpawner()
    {
        prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Menus/ModalWindows/ModalWindow.prefab");
        Debug.Log(prefab.name);
    }

    public void Spawn(ModalWindow window)
    {
        Transform canvas = GameObject.FindGameObjectWithTag("Canvas").transform;
        ModalWindowManager win = GameObject.Instantiate(prefab, canvas).GetComponent<ModalWindowManager>();
        win.data = window;
        win.Setup();
    }
}
