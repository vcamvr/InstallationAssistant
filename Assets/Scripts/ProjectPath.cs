using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class ProjectPath 
{
    private const string ADBPATH= "platform-tools";
    private const string FILEPATH = "Apk文件存放目录";
    public static string GetProjectRootPath()
    {
        DirectoryInfo parent = System.IO.Directory.GetParent(Application.dataPath);
       return parent.ToString();
    }
    public static string GetAdbPath()
    {
        return Path.Combine(GetProjectRootPath(), ADBPATH);
    }

    public static string GetFilePath()
    {
        return Path.Combine(GetProjectRootPath(), FILEPATH);
    }


}
