using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuAnimation : MonoBehaviour
{
    public List<GameObject> prefabs;
    public RectTransform canvas;
    public List<int> probs;

    private int sum;
    // Start is called before the first frame update
    public void Start()
    {
        ConsistantTPS.tps = new BigNumber(30);
        sum = 0;
        foreach (int i in probs)
            sum += i;

        int ammnt = (int)(canvas.rect.width * canvas.rect.height / 9500);

        for (int x = 0; x < ammnt; x++) {
            int num = UnityEngine.Random.Range(0, sum);
            int type = 0;
            foreach (int i in probs) {
                if ((num -= i) < 0)
                    break;
                type ++;
            }
            GameObject slider = Instantiate(prefabs[type]);
            Falling fall = slider.AddComponent(typeof(Falling)) as Falling;
            fall.v = Random.Range(3, 7);
            fall.canvas = canvas;
            ((GenericController)slider.GetComponents(typeof(GenericController))[0]).demo = true;
            // if (slider.GetComponent<DiscountController>()) {
            //     slider.GetComponent<DiscountController>().demo = true;
            // } else if (slider.GetComponent<SliderController>()) {
            //     slider.GetComponent<SliderController>().demo = true;
            // } else if (slider.GetComponent<MultiplyerController>()) {
            //     slider.GetComponent<MultiplyerController>().demo = true;
            // } else if (slider.GetComponent<GeneratorController>()) {
            //     slider.GetComponent<GeneratorController>().demo = true;
            // } else if (slider.GetComponent<GeneratorGenController>()) {
            //     slider.GetComponent<GeneratorGenController>().demo = true;
            // }
            int halfwidth = 150 + (int)(canvas.rect.width / 2);
            int halfheight = 150 + (int)(canvas.rect.height / 2);
            slider.transform.parent = this.transform;
            Quaternion myRotation = Quaternion.identity;
            myRotation.eulerAngles = new Vector3(0, 0, Random.Range(0, 360));
            slider.transform.localPosition = new Vector3(Random.Range(-halfwidth, halfwidth), Random.Range(-halfheight, halfheight), 0);
            slider.transform.localScale = new Vector3(1, 1, 1);
            slider.transform.rotation = myRotation;
            //slider.transform.rotation[2] = 0;
        }
    }

    public IEnumerator Transition()
    {
        foreach (Falling child in GetComponentsInChildren<Falling>())
        {
            child.v = -40 + Random.Range(3, 5);
        }
        yield return Wait(0.9f);
    }

    public bool quit = false;

    IEnumerator Wait(float waitTime)
    {
        float counter = 0;

        while (counter < waitTime)
        {
            //Increment Timer until counter >= waitTime
            counter += Time.deltaTime;
//             Debug.Log("We have waited for: " + counter + " seconds");
            if (quit)
            {
                //Quit function
                yield break;
            }
            //Wait for a frame so that Unity doesn't freeze
            yield return null;
        }
    }

}
