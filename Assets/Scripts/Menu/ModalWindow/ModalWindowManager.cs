using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ModalWindowManager : MonoBehaviour
{
    public ModalWindow data;

    [SerializeField]
    private Text titleText;

    [SerializeField]
    private Transform contentTransform;

    [SerializeField]
    private Transform footerTransform;

    [SerializeField]
    private GameObject ScrollPrefab;

    [SerializeField]
    private GameObject TextPrefab;

    [SerializeField]
    private GameObject ButtonPrefab;

    private Transform parentTransform = null;

    public void Setup()
    {
        titleText.text = data.title;
        parentTransform = contentTransform;

        if (data.isScroll)
        {
            parentTransform = Instantiate(ScrollPrefab, parentTransform).transform.GetChild(0).GetChild(0);
        }
        foreach (string text in data.content)
        {
            Text t = Instantiate(TextPrefab, parentTransform).GetComponent<Text>();
            t.text = text;
        }
        foreach (ModalWindowButton b in data.buttons)
        {
            GameObject button = Instantiate(ButtonPrefab, footerTransform);
            button.GetComponent<Button>().onClick.AddListener(() =>
            {
                b.onClick();
                if (b.destroys) Destroy(gameObject);
            });
            button.GetComponentInChildren<Text>().text = b.text;
        }
    }
    public IEnumerator UpdateLayout(GameObject go)
    {
        yield return null;
        go.GetComponent<ContentSizeFitter>().SetLayoutVertical();
        go.GetComponent<ContentSizeFitter>().SetLayoutHorizontal();
    }

    public void Close()
    {
        if (!data.canClose) return;
        Destroy(gameObject);
    }
}
