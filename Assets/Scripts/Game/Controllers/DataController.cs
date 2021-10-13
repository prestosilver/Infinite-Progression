using System.Collections;
using System.Collections.Generic;
using PyMods;
using UnityEngine;
using UnityEngine.UI;

public class DataController : GenericController
{
    public string modName;

    /// <summary>
    /// the mod data
    /// </summary>
    public string data;

    /// <summary>
    /// the text that displays the error
    /// </summary>
    public Text nameText;

    /// <summary>
    /// setup the variables
    /// </summary>
    /// <param name="id">the module id</param>
    /// <param name="sliders">all the other modules</param>
    public override void SetupVars(int id, List<GameObject> sliders)
    {
    }

    /// <summary>
    /// process multiple ticks
    /// </summary>
    /// <param name="ticks">the amount of ticks to process</param>
    /// <returns>true if success, rn always true</returns>
    public override bool BulkTick(BigNumber ticks)
    {
        return true;
    }

    /// <summary>
    /// update the ui to reflect the data
    /// </summary>
    public override void UpdateDisplay()
    {
        nameText.text = "Mod: " + modName;
    }

    /// <summary>
    /// the type of the module
    /// </summary>
    public override string typeName => modName;

    /// <summary>
    /// loads a save
    /// </summary>
    /// <param name="save">the save data</param>
    public override void LoadSave(string save) => data = save;

    /// <summary>
    /// gets the save data for saving
    /// </summary>
    /// <returns>the save data</returns>
    public override string saveData() => data;
}
