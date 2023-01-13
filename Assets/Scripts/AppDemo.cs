using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System;
using System.Management;

public class AppDemo : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        System.IO.DriveInfo[] drives = System.IO.DriveInfo.GetDrives();
        foreach (System.IO.DriveInfo di in drives)
        {
            Debug.Log(di.Name + "================================================");
            DirectoryInfo TheFolder = new DirectoryInfo(di.Name);
            //bl(TheFolder);
            //Debug.Log(packagesPath);
            Debug.Log(di.Name + "================================================");
        }
    }

   
}
