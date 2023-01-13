using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackageInfo 
{
    public string filePath;
    public string fileName;
    public InstallState installState;
    public string installAdvice;

    public PackageInfo(string m_path, string m_fileName)
    {
        filePath = m_path;
        fileName = m_fileName;
        installState = InstallState.Null;
        installAdvice = "";
    }
    public void UpdateInstallState(InstallState state,string advice="")
    {
        installState = state;
        installAdvice = advice;
    }

}
