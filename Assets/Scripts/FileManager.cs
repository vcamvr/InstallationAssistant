using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;


public static class FileManager 
{
    private const string ADBCommand= "adb shell ls  sdcard -R";
    private const string ADBCommand_Delete = "adb shell rm ";
    private const string ADBCommand_Install = "adb install -r ";
    private const string ADBCommand_Df = "adb shell df -H";
  
    private const string OutMemoryInfo = "内存不足，清理内存后继续安装";
    private const string APKErrorInfo = "安装包出现问题，请联系总部";
    private const string AdbErrorInfo = "设备未就绪";

    public const int MinSize = 5;

    private static List<string> fileList = new List<string>();
    private static List<PackageInfo> packageInfos;
    private static string preMessage;
    private static string currentPath;
    private static int currentRemainFile;
    private static int installTime = 0;
    private static int currentInstallPosition;
  
    public static ProcessInfo processInfo = ProcessInfo.Null;

    #region 扫描
    public static void ScanApkFile()
    {
        processInfo= ProcessInfo.Scanning;
        AdbManager.Execute(ADBCommand, ScanApkFileCallback);
    }
    private static void ScanApkFileCallback(string message, bool isError)
    {
        if (isError)
        {
            if (message.Contains("no devices"))
            {
                AdbManager.isLinkSuccesss = false;
                UIManager.Instance.HideScanUI();
                processInfo = ProcessInfo.Connecting;
            }
            Debug.LogError(message);
            return;
        }
        if (message == null)
        {
            processInfo = ProcessInfo.ScanOver;
            Debug.Log("扫描结束");
            return;
        }


         Debug.Log(message);
        if (message.EndsWith(".apk"))
        {
            if (preMessage == string.Empty || preMessage == "")
            {
                Debug.LogError("preMessage is invalid");
                return;
            }
            if (preMessage.Contains(":"))
            {
                currentPath = preMessage.Replace(":", "/") + message;
            }
            else
            {
                currentPath = "sdcard/" + message;
            }

            if (currentPath.Contains(" "))
            {
                currentPath = currentPath.Replace(" ", @"\ ");
            }
            if (currentPath.Contains("(") || currentPath.Contains(")"))
            {
                currentPath = currentPath.Replace("(", @"\(");
                currentPath = currentPath.Replace(")", @"\)");
            }
            fileList.Add(currentPath);
            Debug.Log("=====APK:" + currentPath);
        }
        else if(message.Contains("sdcard/")|| message.Contains("sdcard:"))
        {
            preMessage = message;
        }
        
    }
    #endregion

    #region 删除
    public static void DeletAllApkFiles()
    {
        processInfo = ProcessInfo.Deleting;
        currentRemainFile = fileList.Count;
        if (fileList.Count<=0)
        {
            currentRemainFile = 0;
            processInfo = ProcessInfo.DeletOver;
            Debug.LogError("无删除内容");
        }
        else
        {
            for (int index = 0; index < fileList.Count; index++)
            {
                currentRemainFile--;
                AdbManager.Execute(ADBCommand_Delete + fileList[index], DeleteApkFileCallback);
            }
        }
        
    }
   
    private static void DeleteApkFileCallback(string message, bool isError)
    {
        Debug.Log("DeleteApkFileCallback:" + message);
        if (isError)
        {
            Debug.LogError(message);
            return;
        }
        if (message == null&& currentRemainFile <= 0)
        {
            processInfo = ProcessInfo.DeletOver;
            fileList.Clear();
            Debug.Log("删除结束");
            return;
        }
    }
    #endregion

    #region 安装
    public static void InstallApkFiles(List<PackageInfo> apkFiles)
    {
        processInfo = ProcessInfo.Installing;
        packageInfos = apkFiles;
        currentInstallPosition = 0;
        InstallApkFile(currentInstallPosition);
    }

    public static void InstallApkFile(int index)
    {
        Debug.Log("InstallApkFile" );
        if (packageInfos.Count <= index)
        {
            Debug.Log("安装完成!");
            //UIManager.Instance.InstallOver(InstallState.Over);
            processInfo = ProcessInfo.installOver;
            //CoreMainThread.Instance.DoInstallEvent(index, InstallState.Over);
            //CoreMainThread.Instance.UnRegisterInstallEvent();
            return;
        }

        packageInfos[index].UpdateInstallState(InstallState.Installing);
        Debug.Log("DelayInstall:AvailRom:" + SystemInfo.Instance.GetAvailRom());

        /*  Thread threadDF = new Thread(new ThreadStart(GetDFInfo));
          threadDF.Start();*/
        GetDFInfo();
        Debug.Log("DelayInstall:AvailRom2:" + SystemInfo.Instance.GetAvailRom());
        if (SystemInfo.Instance.GetAvailRom() >= MinSize)
        {
            Thread thread = new Thread(new ThreadStart(AdbCommant));
            thread.Start();
        }
        else
        {
            Debug.LogError("内部不足5G，跳过安装");
            packageInfos[currentInstallPosition].UpdateInstallState(InstallState.OutMemory, OutMemoryInfo);
            currentInstallPosition++;
            InstallApkFile(currentInstallPosition);
        }

        Debug.Log("AvailRom" + SystemInfo.Instance.GetAvailRom());
       
        //CoreMainThread.Instance.DoInstallEvent(index, InstallState.Installing);
    }


    private static void AdbCommant()
    {
        string path = ADBCommand_Install + "\""+packageInfos[currentInstallPosition].filePath+"\"";
        Debug.Log("install path:"+ path);
        AdbManager.Execute(path, InstallApkFileCallback);
    }
    private static void InstallApkFileCallback(string message, bool isError)
    {
        Debug.Log("InstallApkFileCallback:" + message);
        if (isError)
        {
            Debug.LogError(message);
            if (message.Contains("no devices"))
            {
                Debug.LogError("设备未就绪");
                packageInfos[currentInstallPosition].UpdateInstallState(InstallState.Failure, AdbErrorInfo);
                installTime = 0;
                return;
                //InstallApkFile(currentInstallPosition);
            }
            if (installTime>=3)
            {
                Debug.LogError("重试三次失败:"+ currentInstallPosition+"  path:"+ packageInfos[currentInstallPosition].filePath);
                packageInfos[currentInstallPosition].UpdateInstallState(InstallState.Failure, APKErrorInfo);
                //CoreMainThread.Instance.DoInstallEvent(currentInstallPosition, InstallState.Failure);
                currentInstallPosition++;
                installTime = 0;
                InstallApkFile(currentInstallPosition);
            }
            else
            {
                Debug.LogError("再次安装失败:"+"  path:" + packageInfos[currentInstallPosition].filePath);
                installTime++;
                InstallApkFile(currentInstallPosition);
            }
        }
        else 
        {
            if (message.Contains("Success"))
            {
                Debug.Log("安装成功：" + currentInstallPosition);
                packageInfos[currentInstallPosition].UpdateInstallState(InstallState.Success);
                //CoreMainThread.Instance.DoInstallEvent(currentInstallPosition, InstallState.Success);
                installTime = 0;
                currentInstallPosition++;
                InstallApkFile(currentInstallPosition);
            }
           
        }
    }
    #endregion

    #region 获取内存
    public static void GetDFInfo()
    {
        AdbManager.Execute(ADBCommand_Df, GetDFInfoCallback);
    }

    private static void GetDFInfoCallback(string message, bool isError)
    {
        Debug.Log("GetDFInfoCallback:" + message);
        
        if (isError)
        {
            if(message== "no devices")

            Debug.LogError(message);
            return;
        }

        Debug.Log("GetDFInfoCallback2:" + message);
        if (message.Contains("/storage/emulated"))
        {
            try
            {
                string[] infos = System.Text.RegularExpressions.Regex.Split(message, "\\s+", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                if (infos.Length == 6)
                {
                    infos[3] = infos[3].Replace("G", "");
                    infos[3] = infos[3].Replace("\r", "");
                    infos[3] = infos[3].Replace("\n", "");
                    infos[3] = infos[3].Replace(" ", "");
                    float size = float.Parse(infos[3].ToString());
                    Debug.Log("size:" + size);
                    SystemInfo.Instance.SetAvailRom(size);
                }
            }
            catch (Exception e)
            {

                Debug.LogError("GetDFInfoCallback.Message:" + e.Message);
            }

        }
    }

    #endregion
}
