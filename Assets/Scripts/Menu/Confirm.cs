using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Confirm : MonoBehaviour
{
    /// <summary>
    /// destroy this
    /// </summary>
    public void Cancel()
    {
        Destroy(gameObject);
    }
}
