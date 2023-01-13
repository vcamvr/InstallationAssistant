using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    private string filePath;
    public Text version;

    private void Awake()
    {
        string adbPath = ProjectPath.GetAdbPath();
        AdbManager.path = "cd " +"\""+ adbPath + "\"";
        Debug.Log("adbPath:" + adbPath);
/*#if !UNITY_EDITOR
        Debug.Log("path:"+EnvironmentSetting.GetSysEnvironmentByName("PATH")) ;
        EnvironmentSetting.SetPathAfter(adbPath);
#endif*/


        Debug.unityLogger.logEnabled = true;
        filePath = ProjectPath.GetFilePath();
        GetSoftVersion();
        if (!Directory.Exists(filePath))
        {
            Debug.LogError("文件不存在，开始创建文件夹");
            Directory.CreateDirectory(filePath);
        }
        Debug.Log("filePath:" + filePath);
        FileManager.processInfo = ProcessInfo.Connecting;
        InvokeRepeating("UpdateState", 0, 1);
    }
    void Start()
    {
       
        //AdbManager.Execute("adb get-serialno", MessageCallback);
        //UIManager.Instance.UpdateDeviceState(AdbManager.isLinkSuccesss);
        Debug.Log("AdbManager:"+AdbManager.isLinkSuccesss);
      
        //InvokeRepeating("UpdateProcess",2, 1);

       // FileManager.GetDFInfo();
    }

    private void UpdateState()
    {
        UpdateAdbLinkState();
        UpdateProcess();
    }

    private void UpdateAdbLinkState()
    {
        AdbManager.Execute("adb get-serialno", MessageCallback);
        UIManager.Instance.UpdateDeviceState(AdbManager.isLinkSuccesss);
    }

    public void GetSoftVersion()
    {
        version.text = Application.version;
    }
   
    private void UpdateProcess()
    {
        switch (FileManager.processInfo)
        {
            case ProcessInfo.ConnectSuccess:
                UIManager.Instance.ShowScanUI();
                Debug.Log("开始扫描");
                FileManager.processInfo = ProcessInfo.Scanning;
                FileManager.ScanApkFile();
                break;
            case ProcessInfo.Scanning:
                break;
            case ProcessInfo.ScanOver:
                FileManager.DeletAllApkFiles();
                break;
            case ProcessInfo.Deleting:
                break;
            case ProcessInfo.DeletOver:  
                FileManager.processInfo = ProcessInfo.ContentShow;
                UIManager.Instance.HideScanUI();
                UIManager.Instance.Initialized();
                break;
            case ProcessInfo.ContentShow:
                break;
            case ProcessInfo.Installing:
                break;
            case ProcessInfo.installOver:
                break;
            default:
                break;
        }
       
    }
    // Update is called once per frame
    public static void MessageCallback(string message,bool isError)
    {
        //Debug.Log("MessageCallback:" + message);
        if (message.Contains("PA"))
        {
            AdbManager.isLinkSuccesss = true;
            if(FileManager.processInfo==ProcessInfo.Connecting)
                FileManager.processInfo = ProcessInfo.ConnectSuccess;
        }
        else if(message.Contains("no devices"))
        {
            AdbManager.isLinkSuccesss = false;
            FileManager.processInfo = ProcessInfo.Connecting;
        }
    }
}
