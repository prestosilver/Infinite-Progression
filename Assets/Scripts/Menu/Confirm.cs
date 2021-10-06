using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Confirm : MonoBehaviour
{
    // Start is called before the first frame update
    public void Cancel()
    {
        Destroy(gameObject);
    }
}
