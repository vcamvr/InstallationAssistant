using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class CmdRunner : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        System.IO.DirectoryInfo parent = System.IO.Directory.GetParent(Application.dataPath);
        string projectPath = parent.ToString();
        UnityEngine.Debug.Log("projectPath:"+ projectPath);
        OnDataReceivedHandler = MessageCallback;
        //Execute("adb devices");
        // Execute("adb shell pm list packages -3");

        //Execute("adb shell  procrank");
        Execute("adb install -r "+ @"D:\VeeRArcadeForPico_3.0.6.3_1128.apk");
    }

    public delegate void OnDataReceived(string message);
    public static OnDataReceived OnDataReceivedHandler;

    public static void Execute(string command)
    {
        command = "/c chcp 437&&" + command.Trim().TrimEnd('&') + "&exit";

        Process process = new Process();
        process.StartInfo.FileName = "cmd.exe";
        process.StartInfo.Arguments = command;
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.RedirectStandardError = true;
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.CreateNoWindow = true;

        process.OutputDataReceived += (sender, e) =>
        {
            OnDataReceivedHandler?.Invoke(e.Data);
        };

        process.ErrorDataReceived += (sender, e) =>
        {
            OnDataReceivedHandler?.Invoke(e.Data);
        };

        process.Start();

        process.BeginOutputReadLine();
        process.BeginErrorReadLine();

        process.WaitForExit();
        process.Close();
    }

    public void MessageCallback(string message)
    {
        UnityEngine.Debug.Log("MessageCallback:"+ message);
    }

    public void MessageErrorCallback(string message)
    {
        UnityEngine.Debug.Log("MessageErrorCallback:" + message);
    }

}
