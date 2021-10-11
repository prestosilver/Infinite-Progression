using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuAnimation : MonoBehaviour
{
    /// <summary>
    /// the prefabs for the menu objects
    /// </summary>
    public List<GameObject> prefabs;

    /// <summary>
    /// the canvas to spawn the prefabs on
    /// </summary>
    public RectTransform canvas;

    /// <summary>
    /// the chance for each prefab
    /// </summary>
    public List<int> probs;

    /// <summary>
    /// the total chance
    /// </summary>
    private int sum;

    /// <summary>
    /// wether the game has been closed
    /// </summary>
    public bool quit = false;

    /// <summary>
    /// setup for spawning and stuff
    /// </summary>
    public void Start()
    {
        // setup constant tps to make flash better
        ConsistantTPS.tps = new BigNumber(30);

        // setup sum
        sum = 0;
        foreach (int i in probs)
            sum += i;

        // get the ammount of sliders
        int ammnt = (int)(canvas.rect.width * canvas.rect.height / 9500);

        // spawn all the sliders
        for (int x = 0; x < ammnt; x++)
        {
            // the slider index thing
            int num = UnityEngine.Random.Range(0, sum);

            // the type of the slider
            int type = 0;

            // calculate the slider
            foreach (int i in probs)
            {
                if ((num -= i) < 0)
                    break;
                type++;
            }

            // spawn the slider
            GameObject slider = Instantiate(prefabs[type]);

            // add fall to slider
            Falling fall = slider.AddComponent(typeof(Falling)) as Falling;

            // give fall random velocity
            fall.v = Random.Range(3, 7);
            fall.canvas = canvas;

            // set demo mode on the slider
            ((GenericController)slider.GetComponents(typeof(GenericController))[0]).demo = true;

            // move sliders to random position
            int halfwidth = 150 + (int)(canvas.rect.width / 2);
            int halfheight = 150 + (int)(canvas.rect.height / 2);
            slider.transform.parent = this.transform;
            Quaternion myRotation = Quaternion.identity;
            myRotation.eulerAngles = new Vector3(0, 0, Random.Range(0, 360));
            slider.transform.localPosition = new Vector3(Random.Range(-halfwidth, halfwidth), Random.Range(-halfheight, halfheight), 0);
            slider.transform.localScale = new Vector3(1, 1, 1);
            slider.transform.rotation = myRotation;
        }
    }

    /// <summary>
    /// transition out of the menu
    /// </summary>
    /// <returns></returns>
    public IEnumerator Transition()
    {
        // move all sliders to the top
        foreach (Falling child in GetComponentsInChildren<Falling>())
        {
            child.v = -40 + Random.Range(3, 5);
        }
        yield return Wait(0.9f);
    }

    /// <summary>
    /// wait for x ammount of time
    /// </summary>
    /// <param name="waitTime"></param>
    /// <returns></returns>
    IEnumerator Wait(float waitTime)
    {
        float counter = 0;

        while (counter < waitTime)
        {
            counter += Time.deltaTime;
            if (quit)
            {
                yield break;
            }
            yield return null;
        }
    }

}
