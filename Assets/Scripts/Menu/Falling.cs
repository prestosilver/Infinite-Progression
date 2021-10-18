using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Falling : GenericController
{
    /// <summary>
    /// the canvas object
    /// </summary>
    public RectTransform canvas;

    /// <summary>
    /// the velocity
    /// </summary>
    public int v;

    /// <summary>
    /// moves the object
    /// </summary>
    public void Update()
    {
        transform.localPosition += Vector3.down * v * Time.deltaTime * 60;
        if (transform.localPosition.y < ((-canvas.rect.height / 2) - 150))
            transform.localPosition += Vector3.up * (canvas.rect.height + 300);
    }
}
