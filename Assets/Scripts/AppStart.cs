using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using UnityEditor;
using UnityEngine;
public class AppStart : MonoBehaviour
{
    // private static string adbPath = EditorPrefs.GetString("AndroidSdkRoot") + "/platform-tools/adb";
    private static string adbPath;
    void Start()
    {

        //APKInstaller.InstallAPK();

        //APKInstaller
        //System.Diagnostics.Process.Start("cmd.exe", "adb");

        System.IO.DirectoryInfo parent = System.IO.Directory.GetParent(Application.dataPath);
        string projectPath = parent.ToString();
        UnityEngine.Debug.Log("projectPath:" + projectPath);
        //CmdCtr();
        //ProcessCommand("install -r", projectPath+@"\123.apk");
        Thread thread = new Thread(new ThreadStart(CmdCtr));
        thread.Start();
        //Install("C:/Users/qq547/Desktop/123.apk");

    }


	private void CmdCtr()
	{
        //其中的cmdstr放的是你需要调用的命令
        //ProcessCommand("cmd.exe", "adb install -r "+ "C:/Users/qq547/Desktop/123.apk");
        ProcessCommand("cmd.exe", "adb shell  procrank");
        //ProcessCommand("cmd.exe", "adb shell pm list packages -3");
    }




	public static void Log(string message)
	{
        UnityEngine.Debug.Log(message);
	}
	public static void ProcessCommand(string command, string argument)
	{
		ProcessStartInfo info = new ProcessStartInfo(command);
		//启动应用程序时要使用的一组命令行参数。
		//但是对于cmd来说好像是无效的，可能是因为UseShellExecute的值设置为false了
		//但是对于svn的程序TortoiseProc.exe是可以使用的一个参数
		//info.Arguments = argument;
		//是否弹窗
		info.CreateNoWindow = true;
		//获取或设置指示不能启动进程时是否向用户显示错误对话框的值。
		info.ErrorDialog = true;
		//获取或设置指示是否使用操作系统 shell 启动进程的值。
		info.UseShellExecute = false;
		//info.Arguments = argument;

		UnityEngine.Debug.Log("info:"+ info.UseShellExecute);
		if (info.UseShellExecute)
		{
			info.RedirectStandardOutput = false;
			info.RedirectStandardError = false;
			info.RedirectStandardInput = false;
		}
        else
        {
            info.RedirectStandardOutput = true; //获取或设置指示是否将应用程序的错误输出写入 StandardError 流中的值。
            info.RedirectStandardError = true; //获取或设置指示是否将应用程序的错误输出写入 StandardError 流中的值。
            info.RedirectStandardInput = true;//获取或设置指示应用程序的输入是否从 StandardInput 流中读取的值。
            info.StandardOutputEncoding = System.Text.Encoding.GetEncoding("gb2312"); 
            info.StandardErrorEncoding = System.Text.Encoding.GetEncoding("gb2312"); 
		
        }
        UnityEngine.Debug.Log("123:" );
        //启动(或重用)此 Process 组件的 StartInfo 属性指定的进程资源，并将其与该组件关联。
        System.Diagnostics.Process process = System.Diagnostics.Process.Start(info);
        UnityEngine.Debug.Log("1234:");
        //StandardInput：获取用于写入应用程序输入的流。
        //将字符数组写入文本流，后跟行终止符。
        process.StandardInput.WriteLine(argument);
		//获取或设置一个值，该值指示 StreamWriter 在每次调用 Write(Char) 之后是否都将其缓冲区刷新到基础流。
		process.StandardInput.AutoFlush = true;
        process.StandardInput.WriteLine("exit");
        UnityEngine.Debug.Log("12345:");
        process.WaitForExit();

        string resultN = "Result: " + process.StandardOutput.ReadLine();
        string resultE = process.StandardError.ReadToEnd();
       
        UnityEngine.Debug.Log("123456:" + process.ExitCode);
        if (process.ExitCode != 0)
            UnityEngine.Debug.LogError("result:" + resultE);
        else
            UnityEngine.Debug.Log("result:" + resultN);


        UnityEngine.Debug.Log("StandardOutput:"+process.StandardOutput.ReadLine());
        UnityEngine.Debug.Log("StandardError:" + process.StandardError.ReadToEnd());

        if (!info.UseShellExecute)
        {
            UnityEngine.Debug.Log(process.StandardOutput.ReadLine());
            UnityEngine.Debug.Log(process.StandardError.ReadToEnd());
        }
        //关闭
        process.Close();
	}


    public static void Install(string apkPath, bool run = false)
    {
        // Newly exposed API, only available if Android Module is installed.
        // Points to the SDK in Unity Editor folders. Unity usually uses these SDKs.
        // EditorPrefs.GetString("AndroidSkdRoot") usually points to the same SDKs in newer versions of Unity,
        // in older versions of Unity it usually points to Android studio SDK in Users folder,
        // but you can set your custom SDK in Preferences and EditorPrefs will point there (might be empty).

        AppStart.Log("apkPath2:" + apkPath);
        ProcessStartInfo process = new ProcessStartInfo(apkPath, "install \"" + apkPath + "\"")
        {
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true,
            UseShellExecute = false
        };
        var installProcess = Process.Start(process);
       // EditorUtility.DisplayProgressBar("Installing APK", "Installing...", 0.5f);
        installProcess.WaitForExit();

        string result = "Result: " + installProcess.StandardOutput.ReadLine();
        string details = installProcess.StandardOutput.ReadToEnd();
        if (!string.IsNullOrEmpty(details)) result += ": " + details;
        result += installProcess.StandardError.ReadToEnd();

        if (installProcess.ExitCode != 0)
            UnityEngine.Debug.LogError(result);
        else
            UnityEngine.Debug.Log(result);
        //EditorUtility.ClearProgressBar();
      
        //Use RunApk(...) instead of Run(), if package name in player settings does not match apk package name
        //You need to have Command line tools installed, see documentation
        //if (run) RunApk(apkPath);
    }
}
