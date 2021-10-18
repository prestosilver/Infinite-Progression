using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideStart : MonoBehaviour
{
    public void Start()
    {
        StartCoroutine(SetActiveLater());
    }

    private IEnumerator SetActiveLater()
    {
        yield return new WaitForSeconds(0.01f);
        gameObject.SetActive(false);
    }
}
