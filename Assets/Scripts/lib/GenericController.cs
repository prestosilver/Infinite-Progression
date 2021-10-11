using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericController : MonoBehaviour
{
    /// <summary>
    /// demo mode?
    /// </summary>
    public bool demo = false;

    /// <summary>
    /// the id of the module
    /// </summary>
    public int id = 0;

    /// <summary>
    /// the previous GenericController
    /// </summary>
    public GameObject prev;

    /// <summary>
    /// processes multiple ticks
    /// </summary>
    /// <param name="ticks">the number of ticks to process</param>
    public void DoTick(BigNumber ticks)
    {
        if (demo)
        {
            // demo mode, no bulk ticks
            Demo();
        }
        else
        {
            // the try bulk tick
            if (BulkTick(ticks))
            {
                UpdateDisplay();
                return;
            }

            // do single ticks otherwise
            for (int t = 0; t < ticks; t++)
            {
                Tick();
            }
        }
        // update the text & sliders
        UpdateDisplay();
    }

    /// <summary>
    /// prestige reset
    /// </summary>
    public void DoPrestige()
    {
        Prestige();
        if (prev != null)
        {
            prev.GetComponent<GenericController>().DoPrestige();
        }
    }

    /// <summary>
    /// setup the vars and stuff
    /// </summary>
    /// <param name="nid">the if of the module</param>
    /// <param name="sliders">the other sliders</param>
    /// <param name="name">the name of the module</param>
    /// <param name="nprev">the previous module</param>
    public void Setup(int nid, List<GameObject> sliders, string name, GameObject nprev)
    {
        // disable demo mode
        demo = false;

        // setup variables
        prev = nprev;
        id = nid;
        SetupVars(nid, sliders);
        SetName(name);
    }

    /// <summary>
    /// gets the value of the slider for scoring
    /// </summary>
    /// <returns>the total value</returns>
    public int GetValue()
    {
        if (prev != null)
            return Val() + prev.GetComponent<GenericController>().GetValue();
        return Val();
    }

    /// <summary>
    /// the name of the module
    /// </summary>
    public virtual string typeName => "";

    /// <summary>
    /// gets the save data
    /// </summary>
    public virtual string saveData() => "";

    /// <summary>
    /// loads a save
    /// </summary>
    /// <param name="save">the data</param>
    public virtual void LoadSave(string save) { }

    /// <summary>
    /// setup the module
    /// </summary>
    public virtual void Start() { }

    /// <summary>
    /// process multiple ticks
    /// </summary>
    /// <param name="ticks">the amount of ticks to process</param>
    /// <returns>tick success</returns>
    public virtual bool BulkTick(BigNumber ticks) { return false; }

    /// <summary>
    /// single tick process
    /// </summary>
    public virtual void Tick() { }

    /// <summary>
    /// demo mode
    /// </summary>
    public virtual void Demo() { }

    /// <summary>
    /// sets the name of the module
    /// </summary>
    /// <param name="name"></param>
    public virtual void SetName(string name) { }

    /// <summary>
    /// update the display
    /// </summary>
    public virtual void UpdateDisplay() { }

    /// <summary>
    /// setup the variables
    /// </summary>
    /// <param name="id">the slider id</param>
    /// <param name="sliders">previous sliders</param>
    public virtual void SetupVars(int id, List<GameObject> sliders) { }

    /// <summary>
    /// process prestige
    /// </summary>
    public virtual void Prestige() { }

    /// <summary>
    /// get the value of the slider
    /// </summary>
    /// <returns></returns>
    public virtual int Val() { return id; }
}
