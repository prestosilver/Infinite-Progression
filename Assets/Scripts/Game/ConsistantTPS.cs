using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsistantTPS : MonoBehaviour
{
    /// <summary>
    /// the currrent tps
    /// </summary>
    public static BigNumber tps = new BigNumber(120);

    /// <summary>
    /// the controller attached to this
    /// </summary>
    private GenericController cont;

    /// <summary>
    /// a time counter
    /// </summary>
    private float time = 0;

    /// <summary>
    /// setup
    /// </summary>
    public void Start()
    {
        cont = (GenericController)GetComponents(typeof(GenericController))[0];
    }

    /// <summary>
    /// process ticks
    /// </summary>
    public void Update()
    {
        // avoid div by zero
        if (tps == new BigNumber(0)) return;

        // increase the timer
        time += Time.deltaTime;

        // optimization just to limit to a minimum fps
        if (time < 0.01) return;

        // get the number of ticks to process
        BigNumber ticks = tps * time;

        // process the ticks
        cont.DoTick(ticks);

        // reset time
        time = 0;
    }
}
