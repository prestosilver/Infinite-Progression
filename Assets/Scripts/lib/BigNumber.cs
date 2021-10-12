using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// this is the primary data type for numbers in ip
/// the format is (mantissa) * (10 ^ exponenet_little)
/// </summary>
[Serializable]
public class BigNumber
{
    /// <summary>
    /// the exponent of the number
    /// </summary>
    public int exponent_little = 0, exponent_big = 0;

    /// <summary>
    /// the mantissa
    /// </summary>
    public float mantissa = 0;

    /// <summary>
    /// 1/number
    /// </summary>
    public bool inverse = false;

    /// <summary>
    /// display the numbers as Ey instead of xEy
    /// </summary>
    public static bool LogNotation = false;

    /// <summary>
    /// adds 2 big numbers
    /// </summary>
    public static BigNumber Add(BigNumber a, BigNumber b)
    {
        if (Math.Abs(a.exponent_little - b.exponent_little) > 6)
        {
            // return the bigger number if its 1000000x the other
            if (a.exponent_little > b.exponent_little) return a;
            else return b;
        }

        // otherwise add them properly
        BigNumber c = a;
        if (a.exponent_little > b.exponent_little)
        {
            c.mantissa += b.mantissa / Mathf.Pow(10f, a.exponent_little - b.exponent_little);
        }
        else if (b.exponent_little > a.exponent_little)
        {
            c = b;
            c.mantissa += a.mantissa / Mathf.Pow(10f, b.exponent_little - a.exponent_little);
        }
        else c.mantissa += b.mantissa;
        c.Fix();
        return c;
    }

    /// <summary>
    /// subtract 2 big numbers
    /// </summary>
    public static BigNumber Sub(BigNumber a, BigNumber b)
    {
        // if the result will be negative, return an error
        if (a.exponent_little < b.exponent_little)
        {
            throw new ArithmeticException("Please dont use negative numbers");
        }

        BigNumber c = a;
        if (Math.Abs(a.exponent_little - b.exponent_little) > 6)
        {
            return b;
        }
        // subtract them
        if (b.exponent_little == a.exponent_little)
        {
            c.mantissa -= b.mantissa;
            c.Fix();
            return c;
        }

        // the exponents arent equal
        c.mantissa -= b.mantissa / Mathf.Pow(10f, a.exponent_little - b.exponent_little);
        c.Fix();
        return c;
    }

    /// <summary>
    /// multiplys 2 numbers
    /// </summary>
    public static BigNumber Mul(BigNumber a, BigNumber b)
    {
        // divide if either is an inverse number
        if (a.inverse ^ b.inverse)
        {
            if (a.inverse)
            {
                BigNumber d = a;
                d.inverse = false;
                return Div(b, d);
            }
            if (b.inverse)
            {
                BigNumber d = b;
                d.inverse = false;
                return Div(a, d);
            }
        }

        // check for 0 to speed things up
        if (a.mantissa == 0 || b.mantissa == 0)
        {
            return new BigNumber(0);
        }

        // actually multiply
        BigNumber c = a;
        c.exponent_little += b.exponent_little;
        c.mantissa *= b.mantissa;
        c.Fix();
        return c;
    }

    /// <summary>
    /// divide 2 numbers
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static BigNumber Div(BigNumber a, BigNumber b)
    {
        // check for 0 to speed things up
        if (a.mantissa == 0 && a.exponent_little == 0 && a.exponent_big == 0)
        {
            return new BigNumber(0);
        }
        if (b.mantissa == 0 && b.exponent_little == 0 && b.exponent_big == 0)
        {
            Debug.Log(a);
            throw new DivideByZeroException("Lol you divided by zero");
        }

        // actually divide
        BigNumber c = a;
        if (c.exponent_little < b.exponent_little)
        {
            for (int i = a.exponent_little; i < b.exponent_little; i++)
            {
                c.mantissa /= 10;
            }
            c.exponent_little = 0;
        }
        else
        {
            c.exponent_little -= b.exponent_little;
        }
        c.mantissa /= b.mantissa;
        c.Fix();
        return c;
    }

    /// <summary>
    /// less than
    /// </summary>
    public static bool LT(BigNumber a, BigNumber b)
    {
        if (a.exponent_big < b.exponent_big)
        {
            return true;
        }
        if (a.exponent_little < b.exponent_little)
        {
            return true;
        }
        if (a.mantissa < b.mantissa && a.exponent_little == b.exponent_little)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// greater than
    /// </summary>
    public static bool GT(BigNumber a, BigNumber b)
    {
        if (a.exponent_big > b.exponent_big)
        {
            return true;
        }
        if (a.exponent_little > b.exponent_little)
        {
            return true;
        }
        if (a.mantissa > b.mantissa && a.exponent_little == b.exponent_little)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// exponents, i found this alg online so idk how it works
    /// </summary>
    public static BigNumber Pow(BigNumber x, int n)
    {
        if (n == 0) return new BigNumber(1);

        BigNumber half = Pow(new BigNumber(x), n / 2);
        if (n % 2 == 0)
            return half * half;
        else if (n > 0)
            return half * half * x;
        else
            return half * half / x;
    }

    /// <summary>
    /// fix a big numbers format
    /// </summary>
    private void Fix()
    {
        // set min exponent little
        if (exponent_little < 0) exponent_little = 0;

        // negative number bad
        if (mantissa < 0)
        {
            mantissa = 0;
            return;
        }

        // convert to make sure mantissa is between 1 and 10
        if (mantissa < 1 && exponent_little != 0)
        {
            mantissa *= 10;
            exponent_little -= 1;
        }

        // convert to make sure mantissa is between 1 and 10 
        if (mantissa >= 10)
        {
            mantissa /= 10;
            exponent_little += 1;
        }

        // make sure mantissa is between 1 and 10
        if (exponent_little >= 1)
        {
            int max = 10;
            while (mantissa >= 10)
            {
                if (max-- == 0) break;
                mantissa /= 10;
                exponent_little += 1;
            }
        }
    }

    /// <summary>
    /// make a float look better
    /// </summary>
    public string Clean(float A)
    {
        if (("" + A).Length < 2)
        {
            return "" + A;
        }
        if (("" + A)[("" + A).Length - 2] != '.')
        {
            return "" + A + ".0";
        }
        return "" + A;
    }

    /// <summary>
    /// convert a bignumber to a string
    /// </summary>
    public override string ToString()
    {
        Fix();
        if (LogNotation)
        {
            if (mantissa < 1)
                return "" + mantissa;
            return ("e" + ((double)exponent_little + Mathf.Log10(mantissa)).ToString("F2"));
        }
        string result = "";
        if (inverse)
            result = "1/";
        if (exponent_big > 0)
        {
            result += Clean(Mathf.Round(mantissa)) + "E(" + exponent_big + ")" + exponent_little;
        }
        else if (exponent_little > 5)
        {
            result += Clean(Mathf.Round(mantissa * 10) / 10) + "E" + exponent_little;
        }
        else if (exponent_little > 2)
        {
            result += Clean(Mathf.Round(mantissa * Mathf.Pow(10, exponent_little - 2)) / 10) + "K";
        }
        else
        {
            result += Clean(Mathf.Round(mantissa * Mathf.Pow(10, exponent_little + 1)) / 10);
        }
        return result;
    }

    /// <summary>
    /// constructor
    /// </summary>
    public BigNumber(int a)
    {
        exponent_little = 0;
        exponent_big = 0;
        mantissa = a;
        Fix();
    }

    /// <summary>
    /// constructor
    /// </summary>
    public BigNumber(float a)
    {
        exponent_little = 0;
        exponent_big = 0;
        mantissa = a;
        Fix();
        //Debug.Log("" + a + "=>" + toString());
    }

    /// <summary>
    /// constructor
    /// </summary>
    public BigNumber(BigNumber a)
    {
        exponent_little = a.exponent_little;
        exponent_big = a.exponent_big;
        mantissa = a.mantissa;
        Fix();
        //Debug.Log("" + a + "=>" + toString());
    }

    // make operations easier
    public static BigNumber operator +(BigNumber a, BigNumber b) => Add(new BigNumber(a), new BigNumber(b));
    public static BigNumber operator +(int a, BigNumber b) => Add(new BigNumber(a), new BigNumber(b));
    public static BigNumber operator +(BigNumber a, int b) => Add(new BigNumber(a), new BigNumber(b));
    public static BigNumber operator +(BigNumber a, float b) => Add(new BigNumber(a), new BigNumber(b));
    public static BigNumber operator +(float a, BigNumber b) => Add(new BigNumber(a), new BigNumber(b));

    public static BigNumber operator -(BigNumber a, BigNumber b) => Sub(new BigNumber(a), new BigNumber(b));
    public static BigNumber operator -(int a, BigNumber b) => Sub(new BigNumber(a), new BigNumber(b));
    public static BigNumber operator -(BigNumber a, int b) => Sub(new BigNumber(a), new BigNumber(b));
    public static BigNumber operator -(BigNumber a, float b) => Sub(new BigNumber(a), new BigNumber(b));
    public static BigNumber operator -(float a, BigNumber b) => Sub(new BigNumber(a), new BigNumber(b));

    public static BigNumber operator *(BigNumber a, BigNumber b) => Mul(new BigNumber(a), new BigNumber(b));
    public static BigNumber operator *(int a, BigNumber b) => Mul(new BigNumber(a), new BigNumber(b));
    public static BigNumber operator *(BigNumber a, int b) => Mul(new BigNumber(a), new BigNumber(b));
    public static BigNumber operator *(BigNumber a, float b) => Mul(new BigNumber(a), new BigNumber(b));
    public static BigNumber operator *(float a, BigNumber b) => Mul(new BigNumber(a), new BigNumber(b));

    public static BigNumber Pow(BigNumber a, float b) => Pow(new BigNumber(a), (int)(Mathf.Round(b)));
    public static BigNumber Pow(float a, int b) => Pow(new BigNumber(a), b);

    public static BigNumber operator /(BigNumber a, BigNumber b) => Div(new BigNumber(a), new BigNumber(b));
    public static BigNumber operator /(int a, BigNumber b) => Div(new BigNumber(a), new BigNumber(b));
    public static BigNumber operator /(BigNumber a, int b) => Div(new BigNumber(a), new BigNumber(b));
    public static BigNumber operator /(BigNumber a, float b) => Div(new BigNumber(a), new BigNumber(b));
    public static BigNumber operator /(float a, BigNumber b) => Div(new BigNumber(a), new BigNumber(b));

    public static bool operator <(BigNumber a, BigNumber b) => LT(new BigNumber(a), new BigNumber(b));
    public static bool operator <(int a, BigNumber b) => LT(new BigNumber(a), new BigNumber(b));
    public static bool operator <(BigNumber a, int b) => LT(new BigNumber(a), new BigNumber(b));
    public static bool operator <(BigNumber a, float b) => LT(new BigNumber(a), new BigNumber(b));
    public static bool operator <(float a, BigNumber b) => LT(new BigNumber(a), new BigNumber(b));
    public static bool operator <=(BigNumber a, BigNumber b) => LT(new BigNumber(a), new BigNumber(b));
    public static bool operator <=(int a, BigNumber b) => LT(new BigNumber(a), new BigNumber(b));
    public static bool operator <=(BigNumber a, int b) => LT(new BigNumber(a), new BigNumber(b));
    public static bool operator <=(BigNumber a, float b) => LT(new BigNumber(a), new BigNumber(b));
    public static bool operator <=(float a, BigNumber b) => LT(new BigNumber(a), new BigNumber(b));

    public static bool operator >(BigNumber a, BigNumber b) => GT(new BigNumber(a), new BigNumber(b));
    public static bool operator >(int a, BigNumber b) => GT(new BigNumber(a), new BigNumber(b));
    public static bool operator >(BigNumber a, int b) => GT(new BigNumber(a), new BigNumber(b));
    public static bool operator >(BigNumber a, float b) => GT(new BigNumber(a), new BigNumber(b));
    public static bool operator >(float a, BigNumber b) => GT(new BigNumber(a), new BigNumber(b));
    public static bool operator >=(BigNumber a, BigNumber b) => GT(new BigNumber(a), new BigNumber(b));
    public static bool operator >=(int a, BigNumber b) => GT(new BigNumber(a), new BigNumber(b));
    public static bool operator >=(BigNumber a, int b) => GT(new BigNumber(a), new BigNumber(b));
    public static bool operator >=(BigNumber a, float b) => GT(new BigNumber(a), new BigNumber(b));
    public static bool operator >=(float a, BigNumber b) => GT(new BigNumber(a), new BigNumber(b));
}
