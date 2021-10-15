using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public static class SeededRand
{
    // the default seed, to reimplement
    public static int seed = 14321;

    // data for random names
    private const string vowels = "aeiouy";
    private const string constonants = "bcdfghjklmnpqrstvwxyz";
    const string Symbols = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    /// <summary>
    /// set the seed
    /// </summary>
    /// <param name="fseed">the seed to set</param>
    public static void SetSeed(int fseed)
    {
        seed = fseed;
    }

    /// <summary>
    /// perlin noise based 2d random
    /// </summary>
    /// <param name="at">the id of the random number</param>
    /// <returns>a random number</returns>
    public static float Perlin(float at)
    {
        float rand = (float)NormalInv((double)Mathf.PerlinNoise(Mathf.PI / 20 * at, Mathf.PI / 3 * seed)); // pi is a Nothing up my sleeve number
        return rand;
    }

    /// <summary>
    /// gets a random word at postion num
    /// </summary>
    /// <param name="num">the word number</param>
    /// <returns>a random word</returns>
    public static string Word(int num)
    {
        // no random names
        if (PlayerPrefs.GetString("RandNames") != "True")
            return Base26((num / 100) - 1);

        int x = 0;
        string m = "";
        while (x < (Perlin(num) * 3 + 5))
        {
            if (x % 2 == 1)
            {
                m += vowels[(int)(Perlin(num + x) * 6)];
            }
            else
            {
                m += constonants[(int)(Perlin(num + x) * 20)];
            }
            x += 1;
        }
        return m;
    }

    /// <summary>
    /// non random word
    /// </summary>
    /// <param name="id">the number to convert</param>
    /// <returns>a base 26 number</returns>
    static string Base26(int id)
    {
        int iter = id;
        string result = "";
        if (iter < Symbols.Length)
            return "" + Symbols[iter % Symbols.Length];
        do
        {
            if (iter / Symbols.Length == 0)
                result = Symbols[iter % Symbols.Length - 1] + result;
            else
                result = Symbols[iter % Symbols.Length] + result;
            iter /= Symbols.Length;
        } while (iter != 0);
        return result;
    }

    /// <summary>
    /// normalize the random function
    /// </summary>
    /// <param name="q">the random value</param>
    /// <returns>normailzed value</returns>
    private static double NormalInv(double q)
    {
        // return q;
        // I forgot how this works so no comments
        if (q == .5)
            return 0;

        q = 1.0 - q;

        double p = (q > 0.0 && q < 0.5) ? q : (1.0 - q);
        double t = Math.Sqrt(Math.Log(1.0 / Math.Pow(p, 2.0)));

        double c0 = 2.515517;
        double c1 = 0.802853;
        double c2 = 0.010328;

        double d1 = 1.432788;
        double d2 = 0.189269;
        double d3 = 0.001308;

        double x = t - (c0 + c1 * t + c2 * Math.Pow(t, 2.0)) /
                       (1.0 + d1 * t + d2 * Math.Pow(t, 2.0) + d3 * Math.Pow(t, 3.0));

        if (q > .5)
            x *= -1.0;

        return Mathf.Clamp01((float)Math.Abs((x + 3) * 0.1666667f));
    }
}

