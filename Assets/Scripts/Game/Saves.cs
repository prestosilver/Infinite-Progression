using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

static class Saves
{
    /// <summary>
    /// the path of Saves
    /// </summary>
    public static string saveDir = Path.Combine(Application.persistentDataPath, "Saves");

    /// <summary>
    /// the current save
    /// </summary>
    public static string saveName = "NewSave";

    /// <summary>
    /// save file name
    /// </summary>
    public static string savePath = "save.dat";

    /// <summary>
    /// the full save file path
    /// </summary>
    public static string fullSavePath => Path.Combine(new string[] { saveDir, saveName, savePath });

    /// <summary>
    /// saves the game
    /// </summary>
    public static void Save()
    {
        // setup a data variable
        String data = "";

        // store the tps for later
        BigNumber tps = ConsistantTPS.tps;

        // misc datas
        data += $"{GameController.instance.presLevel};";
        data += $"{tps.mantissa};";
        data += $"{tps.exponent_big};";
        data += $"{tps.exponent_little}\n";

        // save the slider data
        foreach (GameObject controller in GameController.instance.sliders)
        {
            GenericController cont = controller.GetComponent<GenericController>();
            data += $"{cont.typeName};{cont.saveData()}\n";
        }

        // write the save
        // ik this can be optimized
        // feel free to optimize and submit a pr
        File.WriteAllText(fullSavePath, data);
    }

    /// <summary>
    /// delete the save file
    /// </summary>
    internal static void Delete()
    {
        // ignore if the save is nonexistent
        if (!Directory.Exists(saveDir + "/" + saveName)) return;

        // delete the save
        Directory.Delete(saveDir + "/" + saveName, true);
    }

    /// <summary>
    /// read the save
    /// </summary>
    /// <returns>the save data</returns>
    public static List<String> Read()
    {
        // if no file return no data
        if (!File.Exists(fullSavePath)) return new List<String>();

        // return the save
        return new List<string>(File.ReadAllLines(fullSavePath));
    }
}
