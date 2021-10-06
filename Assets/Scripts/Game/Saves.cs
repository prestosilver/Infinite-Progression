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
    public const int headerSize = 28;
    public const int singleObjectSize = 44;
    public const int saveVer = 3;

    public unsafe static byte[] SlidersToByteArray(List<GameObject> sliders, int ammnt, int presLevel, int seed, bool seeded)
    {

        int slider_count = ammnt;

        int total_size = headerSize + singleObjectSize * ammnt;

        byte[] result = new byte[total_size];

        byte[] lol = System.BitConverter.GetBytes(saveVer & 0x0FFF);
        lol[3] = System.BitConverter.GetBytes(seeded)[0];
        for (int j = 0; j < 4; j++)
        {
            result[j] = System.BitConverter.GetBytes(slider_count)[j];
            result[j + 4] = System.BitConverter.GetBytes(presLevel)[j];
            result[j + 8] = System.BitConverter.GetBytes(seed)[j];
            result[j + 12] = lol[j];
            result[j + 16] = System.BitConverter.GetBytes(ConsistantTPS.tps.mantissa)[j];
            result[j + 20] = System.BitConverter.GetBytes(ConsistantTPS.tps.exponent_little)[j];
            result[j + 24] = System.BitConverter.GetBytes(ConsistantTPS.tps.exponent_big)[j];
        }
        int i = 0;
        foreach (GameObject g in sliders)
        {
            SliderController sc = g.GetComponent<SliderController>();
            if (sc != null)
            {
                byte[] tmpa = System.BitConverter.GetBytes(0);
                byte[] tmpb = System.BitConverter.GetBytes(sc.mult);
                byte[] tmpc = System.BitConverter.GetBytes(sc.increase);
                byte[] tmpd = System.BitConverter.GetBytes(sc.max.mantissa);
                byte[] tmpe = System.BitConverter.GetBytes(sc.max.exponent_big);
                byte[] tmpf = System.BitConverter.GetBytes(sc.max.exponent_little);
                byte[] tmpg = System.BitConverter.GetBytes(sc.value.mantissa);
                byte[] tmph = System.BitConverter.GetBytes(sc.value.exponent_big);
                byte[] tmpi = System.BitConverter.GetBytes(sc.value.exponent_little);
                for (int j = 0; j < 4; j++)
                {
                    result[j + headerSize + 0 + (i * singleObjectSize)] = tmpa[j];
                    result[j + headerSize + 4 + (i * singleObjectSize)] = tmpb[j];
                    result[j + headerSize + 8 + (i * singleObjectSize)] = tmpc[j];
                    result[j + headerSize + 12 + (i * singleObjectSize)] = tmpd[j];
                    result[j + headerSize + 16 + (i * singleObjectSize)] = tmpe[j];
                    result[j + headerSize + 20 + (i * singleObjectSize)] = tmpf[j];
                    result[j + headerSize + 24 + (i * singleObjectSize)] = tmpg[j];
                    result[j + headerSize + 28 + (i * singleObjectSize)] = tmph[j];
                    result[j + headerSize + 32 + (i * singleObjectSize)] = tmpi[j];
                }
            }
            MultiplyerController mc = g.GetComponent<MultiplyerController>();
            if (mc != null)
            {
                byte[] tmpa = System.BitConverter.GetBytes(1);
                byte[] tmpb = System.BitConverter.GetBytes(mc.level);
                byte[] tmpc = System.BitConverter.GetBytes(mc.discount);
                byte[] tmpd = System.BitConverter.GetBytes(mc.buys.GetComponent<SliderController>().id);
                byte[] tmpe = System.BitConverter.GetBytes(mc.muls.GetComponent<SliderController>().id);
                for (int j = 0; j < 4; j++)
                {
                    result[j + headerSize + 0 + (i * singleObjectSize)] = tmpa[j];
                    result[j + headerSize + 4 + (i * singleObjectSize)] = tmpb[j];
                    result[j + headerSize + 8 + (i * singleObjectSize)] = tmpc[j];
                    result[j + headerSize + 12 + (i * singleObjectSize)] = tmpd[j];
                    result[j + headerSize + 16 + (i * singleObjectSize)] = tmpe[j];
                }
            }
            GeneratorController gc = g.GetComponent<GeneratorController>();
            if (gc != null)
            {
                byte[] tmpa = System.BitConverter.GetBytes(2);
                byte[] tmpb = System.BitConverter.GetBytes(gc.level);
                for (int j = 0; j < 4; j++)
                {
                    result[j + headerSize + 0 + (i * singleObjectSize)] = tmpa[j];
                    result[j + headerSize + 4 + (i * singleObjectSize)] = tmpb[j];
                }
            }
            DiscountController dc = g.GetComponent<DiscountController>();
            if (dc != null)
            {
                byte[] tmpa = System.BitConverter.GetBytes(3);
                byte[] tmpb = System.BitConverter.GetBytes(dc.level);
                for (int j = 0; j < 4; j++)
                {
                    result[j + headerSize + 0 + (i * singleObjectSize)] = tmpa[j];
                    result[j + headerSize + 4 + (i * singleObjectSize)] = tmpb[j];
                }
            }
            GeneratorGenController ggc = g.GetComponent<GeneratorGenController>();
            if (ggc != null)
            {
                byte[] tmpa = System.BitConverter.GetBytes(4);
                byte[] tmpb = System.BitConverter.GetBytes(ggc.level);
                for (int j = 0; j < 4; j++)
                {
                    result[j + headerSize + 0 + (i * singleObjectSize)] = tmpa[j];
                    result[j + headerSize + 4 + (i * singleObjectSize)] = tmpb[j];
                }
            }
            LockController lc = g.GetComponent<LockController>();
            if (lc != null)
            {
                byte[] tmpa = System.BitConverter.GetBytes(5);
                for (int j = 0; j < 4; j++)
                {
                    result[j + headerSize + 0 + (i * singleObjectSize)] = tmpa[j];
                }
            }
            AutoController ac = g.GetComponent<AutoController>();
            if (ac != null)
            {
                byte[] tmpa = System.BitConverter.GetBytes(ac.level);
                result[3 + headerSize + 40 + (i * singleObjectSize)] = System.BitConverter.GetBytes(ac.active)[0];
                for (int j = 0; j < 4; j++)
                {
                    result[j + headerSize + 36 + (i * singleObjectSize)] = tmpa[j];
                }
            }
            i++;
        }
        return result;
    }

    public static void Reset(int ammnt = 0, bool hasseed = false, int seed = 0)
    {
        Save(SlidersToByteArray(new List<GameObject>(), ammnt, 0, UnityEngine.Random.Range(-32767, 32767), false));
        if (hasseed)
            Save(SlidersToByteArray(new List<GameObject>(), ammnt, 0, seed, true));
    }

    public static void Save(byte[] bytes)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(fullSavePath);

        bf.Serialize(file, bytes);
        file.Close();
    }

    public static int GetHeadSize(byte[] bytes)
    {
        int ver = FindVer(bytes);
        if (ver <= 2) return 16;
        else return headerSize;
    }

    public static byte[] Load()
    {
        if (File.Exists(fullSavePath))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(fullSavePath, FileMode.Open);
            byte[] data = (byte[])bf.Deserialize(file);
            file.Close();
            return data;
        }
        else
        {
            Reset();
            if (File.Exists(fullSavePath))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(fullSavePath, FileMode.Open);
                byte[] data = (byte[])bf.Deserialize(file);
                file.Close();
                return data;
            }
            return null;
        }
    }

    public static int FindInt(byte[] bytes, int index)
    {
        return System.BitConverter.ToInt32(bytes, index);
    }

    public static bool FindBool(byte[] bytes, int index)
    {
        return 0 != bytes[index + 3];
    }

    public static uint FindUInt(byte[] bytes, int index)
    {
        return System.BitConverter.ToUInt32(bytes, index);
    }

    public static int FindVer(byte[] bytes)
    {
        return System.BitConverter.ToInt32(bytes, 12) & 0x0FFF;
    }

    public static float FindFloat(byte[] bytes, int index)
    {
        return System.BitConverter.ToSingle(bytes, index);
    }
    public static byte[] FindObject(byte[] bytes, int index)
    {
        byte[] buffer = new byte[singleObjectSize];
        for (int j = 0; j < singleObjectSize; j++)
        {
            buffer[j] = bytes[j + index];
        }
        return buffer;
    }
}
