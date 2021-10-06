using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsistantTPS : MonoBehaviour
{
    public static BigNumber tps = new BigNumber(120);
    private GenericController cont;
    private float time = 0;

    public void Start()
    {
        cont = (GenericController)GetComponents(typeof(GenericController))[0];
    }

    // Update is called once per frame
    public void Update()
    {
        if (tps == new BigNumber(0)) {return; }
		time += Time.deltaTime;
        if (time < 0.01) {
            return;
        }
        BigNumber ticks = tps * time;
        cont.DoTick(ticks);
        time = 0;
    }
}
