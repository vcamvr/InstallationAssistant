using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstallDotAnimation : MonoBehaviour
{
    private Text content;
    private int currentNumber;
    private void OnEnable()
    {
        CancelInvoke();
        if (content==null)
        {
            content = GetComponent<Text>();
        }
        currentNumber = -1;
        InvokeRepeating("UpdateInstallDot", 0,1f);
    }

    private void UpdateInstallDot()
    {
        currentNumber = ++currentNumber % 3;
        if (currentNumber == 0)
        {
            content.text = "安装中.";
        }
        else if (currentNumber == 1)
        {
            content.text = "安装中..";
        }
        else if (currentNumber == 2)
        {
            content.text = "安装中...";
        }

    }

    private void OnDisable()
    {
        CancelInvoke();
    }

}
