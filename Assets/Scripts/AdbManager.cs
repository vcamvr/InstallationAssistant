using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public static class AdbManager 
{
    public static bool isLinkSuccesss=false;
    public static string path;
    public static void Execute2(string command,Action<string,bool> action)
    {
        command = "/c chcp 437&&" + command.Trim().TrimEnd('&') + "&exit";
        UnityEngine.Debug.Log("Execute:" + command);
        Process process = new Process();
        process.StartInfo.FileName = "cmd.exe";
        process.StartInfo.Arguments = command;
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.RedirectStandardError = true;
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.CreateNoWindow = true;

        process.OutputDataReceived += (sender, e) =>
        {
            if (e.Data == string.Empty)
                return;
            action?.Invoke(e.Data,false);
        };

        process.ErrorDataReceived += (sender, e) =>
        {
            if (e.Data == null || e.Data == string.Empty)
                return;
            action?.Invoke(e.Data,true);
        };

        process.Start();

        process.BeginOutputReadLine();
        process.BeginErrorReadLine();

        process.WaitForExit();
        process.Close();
    }


    public static void Execute(string command, Action<string, bool> action)
    {
        command = "/c chcp 437&&"+ path+ "&" + command.Trim().TrimEnd('&') + "&exit";
        //UnityEngine.Debug.Log("Execute2:"+ command);
        Process process = new Process();
        process.StartInfo.FileName = "cmd.exe";
        process.StartInfo.Arguments = command;
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.RedirectStandardError = true;
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.CreateNoWindow = true;

        process.OutputDataReceived += (sender, e) =>
        {
            if (e.Data == string.Empty)
                return;
            action?.Invoke(e.Data, false);
        };

        process.ErrorDataReceived += (sender, e) =>
        {
            if (e.Data == null || e.Data == string.Empty)
                return;
            action?.Invoke(e.Data, true);
        };

        process.Start();

        process.BeginOutputReadLine();
        process.BeginErrorReadLine();

        process.WaitForExit();
        process.Close();
    }

}
