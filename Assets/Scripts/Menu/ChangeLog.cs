using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeLog : MonoBehaviour
{
    public TextAsset log;
    // Start is called before the first frame update
    public void Start()
    {
        gameObject.GetComponent<Text>().text = log.text;
        transform.localPosition -= Vector3.up * 1000000;
    }
}
