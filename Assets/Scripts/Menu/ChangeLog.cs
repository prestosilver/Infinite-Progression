using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeLog : MonoBehaviour
{
    // singleton class
    public TextAsset log;

    /// <summary>
    /// setup the changelog
    /// </summary>
    public void Start()
    {
        gameObject.GetComponent<Text>().text = log.text;
        transform.localPosition -= Vector3.up * 1000000;
    }
}
