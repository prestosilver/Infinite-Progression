using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GenericController : MonoBehaviour
{
    public bool demo = false;
    public int id = 0;
    public GameObject prev;

    public void DoTick(BigNumber ticks)
    {
        if (demo)
        {
            Demo();
        }
        else
        {
            if (BulkTick(ticks))
            {
                UpdateDisplay();
                return;
            }
            for (int t = 0; t < ticks; t++)
            {
                Tick();
            }
        }
        UpdateDisplay();
    }

    public void DoPrestige()
    {
        Prestige();
        if (prev != null)
        {
            prev.GetComponent<GenericController>().DoPrestige();
        }
    }

    public void Setup(int nid, List<GameObject> sliders, string name, GameObject nprev)
    {
        demo = false;
        prev = nprev;
        id = nid;
        SetupVars(nid, sliders);
        SetName(name);
    }

    public int GetValue()
    {
        if (prev != null)
            return Val() + prev.GetComponent<GenericController>().GetValue();
        return Val();
    }

    public virtual void LoadSave(byte[] objSave) { }
    public virtual void Start() { }
    public virtual bool BulkTick(BigNumber ticks) { return false; }
    public virtual void Tick() { }
    public virtual void Demo() { }
    public virtual void SetName(string name) { }
    public virtual void UpdateDisplay() { }
    public virtual void SetupVars(int id, List<GameObject> sliders) { }
    public virtual void Prestige() { }
    public virtual int Val() { return id; }
}
