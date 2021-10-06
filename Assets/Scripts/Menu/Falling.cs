using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Falling : GenericController
{
    public RectTransform canvas;
    public int v;
    public void Update()
    {
        transform.localPosition += Vector3.down * v;
        if (transform.localPosition.y < ((-canvas.rect.height / 2) - 150))
            transform.localPosition += Vector3.up * (canvas.rect.height + 300);
    }
}
