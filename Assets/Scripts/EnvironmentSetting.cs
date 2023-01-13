using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EnvironmentSetting 
{
    /// <summary>
    /// 获取系统环境变量
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static string GetSysEnvironmentByName(string name)
    {
        string result = string.Empty;
        try
        {
            result = OpenSysEnvironment().GetValue(name).ToString();//读取
        }
        catch (Exception e)
        {
            Debug.LogError("e.Message:" + e.Message+ "  e.StackTrace:" + e.StackTrace);
            return string.Empty;
        }
        return result;

    }

    /// <summary>
    /// 打开系统环境变量注册表
    /// </summary>
    /// <returns>RegistryKey</returns>
    private static RegistryKey OpenSysEnvironment()
    {
        //RegistryKey regLocalMachine = Registry.CurrentUser.OpenSubKey("HKEY_LOCAL_MACHINE", true);
        RegistryKey regLocalMachine = Registry.LocalMachine;
        RegistryKey regSYSTEM = regLocalMachine.OpenSubKey(@"SYSTEM\ControlSet001\Control\Session Manager\Environment",true);//打开HKEY_LOCAL_MACHINE下的SYSTEM 
   /*     RegistryKey regControlSet001 = regSYSTEM.OpenSubKey("ControlSet001", System.Security.AccessControl.RegistryRights.ChangePermissions);//打开ControlSet001 
        RegistryKey regControl = regControlSet001.OpenSubKey("Control", System.Security.AccessControl.RegistryRights.ChangePermissions);//打开Control 
        RegistryKey regManager = regControl.OpenSubKey("Session Manager", System.Security.AccessControl.RegistryRights.ChangePermissions);//打开Control 

        RegistryKey regEnvironment = regManager.OpenSubKey("Environment", System.Security.AccessControl.RegistryRights.ChangePermissions);*/
        return regSYSTEM;
    }


    /// <summary>
    /// 设置系统环境变量
    /// </summary>
    /// <param name="name">变量名</param>
    /// <param name="strValue">值</param>
    public static void SetSysEnvironment(string name, string strValue)
    {
        OpenSysEnvironment().SetValue(name, strValue);

    }



    /// <summary>
    /// 添加到PATH环境变量（会检测路径是否存在，存在就不重复）
    /// </summary>
    /// <param name="strPath"></param>
    public static void SetPathAfter(string strHome)
    {
        string pathlist;
        pathlist = GetSysEnvironmentByName("PATH");
        //检测是否以;结尾
        if (pathlist.Substring(pathlist.Length - 1, 1) != ";")
        {
            SetSysEnvironment("PATH", pathlist + ";");
            pathlist = GetSysEnvironmentByName("PATH");
        }
        string[] list = pathlist.Split(';');
        bool isPathExist = false;

        foreach (string item in list)
        {
            if (item == strHome)
                isPathExist = true;
        }
        if (!isPathExist)
        {
            SetSysEnvironment("PATH", pathlist + strHome + ";");
        }

    }

    public static void SetPathBefore(string strHome)
    {

        string pathlist;
        pathlist = GetSysEnvironmentByName("PATH");
        string[] list = pathlist.Split(';');
        bool isPathExist = false;

        foreach (string item in list)
        {
            if (item == strHome)
                isPathExist = true;
        }
        if (!isPathExist)
        {
            SetSysEnvironment("PATH", strHome + ";" + pathlist);
        }

    }

    public static void SetPath(string strHome)
    {

        string pathlist;
        pathlist = GetSysEnvironmentByName("PATH");
        string[] list = pathlist.Split(';');
        bool isPathExist = false;

        foreach (string item in list)
        {
            if (item == strHome)
                isPathExist = true;
        }
        if (!isPathExist)
        {
            SetSysEnvironment("PATH", pathlist + strHome + ";");

        }

    }


}
