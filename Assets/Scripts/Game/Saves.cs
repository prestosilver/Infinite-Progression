using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

static class Saves
{
    public static string saveDir = Application.persistentDataPath + "/Saves/";
    public static string saveName = "NewSave";
    public static string savePath = "/save.dat";
    public static string fullSavePath => saveDir + "/" + saveName + savePath;
    public static void Save()
    {
        String data = "";
        BigNumber tps = ConsistantTPS.tps;
        data += $"{GameController.instance.presLevel};";
        data += $"{tps.mantissa};";
        data += $"{tps.exponent_big};";
        data += $"{tps.exponent_little}\n";
        foreach (GameObject controller in GameController.instance.sliders)
        {
            GenericController cont = controller.GetComponent<GenericController>();
            data += $"{cont.typeName};{cont.saveData()}\n";
        }
        File.WriteAllText(fullSavePath, data);
    }
    public static List<String> Read()
    {
        if (!File.Exists(fullSavePath)) { return new List<String>(); }
        return new List<string>(File.ReadAllLines(fullSavePath));
    }
}
