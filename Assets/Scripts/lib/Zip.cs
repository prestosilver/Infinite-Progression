using UnityEngine;
using System.Collections;
using System;
using System.Runtime.InteropServices;
using System.Globalization;
using System.Text;
using System.IO;
using System.IO.Compression;

public class ZipUtil
{
    /// <summary>
    /// unzips a zip file
    /// </summary>
    /// <param name="zipFilePath">the zip file</param>
    /// <param name="location">where to unzip</param>
    public static void Unzip(string zipFilePath, string location)
    {
        try
        {
            ZipFile.ExtractToDirectory(zipFilePath, location);
        }
        catch (IOException)
        {
            return;
        }
    }

}