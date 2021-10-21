using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShowOnScene : MonoBehaviour
{
    [SerializeField]
    private string name;

    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().name != name)
        {
            Destroy(gameObject);
        }
    }
}
