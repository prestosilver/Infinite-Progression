using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public bool inverse = false;
    public static bool LogNotation = false;

    public static BigNumber Add(BigNumber a, BigNumber b)
    {
        BigNumber c = a;
        if (Math.Abs(a.exponent_little - b.exponent_little) > 6)
        {
            if (a.exponent_little > b.exponent_little)
            {
                return a;
            }
            if (b.exponent_little > a.exponent_little)
            {
                return b;
            }
        }
        else
        {
            if (a.exponent_little > b.exponent_little)
            {
                c.mantissa += b.mantissa / Mathf.Pow(10f, a.exponent_little - b.exponent_little);
            }
            else if (b.exponent_little > a.exponent_little)
            {
                c = b;
                c.mantissa += a.mantissa / Mathf.Pow(10f, b.exponent_little - a.exponent_little);
            }
            else
            {
                c.mantissa += b.mantissa;
            }
        }
        //Debug.Log(a.toString() + "+" + b.toString() + "=" + c.toString());
        c.Fix();
        //Debug.Log(a.toString() + "+" + b.toString() + "=" + c.toString());
        return c;
    }


    public static BigNumber Sub(BigNumber a, BigNumber b)
    {
        if (a.exponent_little < b.exponent_little)
        {
            // Debug.Log(a.toString() + " - " + b.toString());
            throw new ArithmeticException("Negative number bad");
        }
        BigNumber c = a;
        if (Math.Abs(a.exponent_little - b.exponent_little) > 6)
        {
            return b;
        }
        else if (b.exponent_little < a.exponent_little)
        {
            c.mantissa -= b.mantissa / Mathf.Pow(10f, a.exponent_little - b.exponent_little);
            c.Fix();
            return c;
        }
        c.mantissa -= b.mantissa;
        c.Fix();
        return c;
    }

    public static BigNumber Mul(BigNumber a, BigNumber b)
    {
        if (a.inverse ^ b.inverse)
        {
            if (a.inverse)
            {
                BigNumber d = a;
                d.inverse = false;
                return Div(a, d);
            }
            if (b.inverse)
            {
                BigNumber d = b;
                d.inverse = false;
                return Div(b, d);
            }
        }
        if (a.mantissa == 0 || b.mantissa == 0)
        {
            return new BigNumber(0);
        }
        BigNumber c = a;
        c.exponent_little += b.exponent_little;
        c.mantissa *= b.mantissa;
        c.Fix();
        return c;
    }


    public static BigNumber Div(BigNumber a, BigNumber b)
    {
        // if (a.inverse ^ b.inverse){
        //     if (a.inverse) {
        //         BigNumber d = b;
        //         d.inverse = true;
        //         return Mul(a, d);
        //     }
        //     if (b.inverse) {
        //         BigNumber d = b;
        //         d.inverse = false;
        //         return Mul(b, d);
        //     }
        // }
        if (a.mantissa == 0 && a.exponent_little == 0 && a.exponent_big == 0)
        {
            return new BigNumber(0);
        }
        if (b.mantissa == 0 && b.exponent_little == 0 && b.exponent_big == 0)
        {
            Debug.Log(a);
            throw new DivideByZeroException("You Suck :|");
        }
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

    private void Fix()
    {
        // exponent_big = 0;
        if (exponent_little < 0)
        {
            exponent_little = 0;
        }
        if (mantissa < 0)
        {
            Debug.Log(mantissa);
            mantissa = 0;
            return;
        }
        if (mantissa < 1 && exponent_little != 0)
        {
            mantissa *= 10;
            exponent_little -= 1;
        }
        // if ((exponent_little < 6 && exponent_little != 0)) {
        //     while (exponent_little >= 1) {
        //         mantissa *= 10;
        //         exponent_little -= 1;
        //     }
        // }
        if (mantissa >= 10)
        {
            mantissa /= 10;
            exponent_little += 1;
        }
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

    public string lol(float A)
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
            result += lol(Mathf.Round(mantissa)) + "E(" + exponent_big + ")" + exponent_little;
        }
        else if (exponent_little > 5)
        {
            result += lol(Mathf.Round(mantissa * 10) / 10) + "E" + exponent_little;
        }
        else if (exponent_little > 2)
        {
            result += lol(Mathf.Round(mantissa * Mathf.Pow(10, exponent_little - 2)) / 10) + "K";
        }
        else
        {
            result += lol(Mathf.Round(mantissa * Mathf.Pow(10, exponent_little + 1)) / 10);
        }
        return result;
    }

    public BigNumber(int a)
    {
        exponent_little = 0;
        exponent_big = 0;
        mantissa = a;
        Fix();
        //Debug.Log("" + a + "=>" + toString());
    }

    public BigNumber(float a)
    {
        exponent_little = 0;
        exponent_big = 0;
        mantissa = a;
        Fix();
        //Debug.Log("" + a + "=>" + toString());
    }

    public BigNumber(BigNumber a)
    {
        exponent_little = a.exponent_little;
        exponent_big = a.exponent_big;
        mantissa = a.mantissa;
        Fix();
        //Debug.Log("" + a + "=>" + toString());
    }

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
