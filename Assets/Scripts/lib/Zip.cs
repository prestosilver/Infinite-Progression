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
#if UNITY_IPHONE
	[DllImport("__Internal")]
	private static extern void unzip (string zipFilePath, string location);

	[DllImport("__Internal")]
	private static extern void zip (string zipFilePath);

	[DllImport("__Internal")]
	private static extern void addZipFile (string addFile);

#endif

    public static void Unzip(string zipFilePath, string location)
    {
        ZipFile.ExtractToDirectory(zipFilePath, location);
    }

}