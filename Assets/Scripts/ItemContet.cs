using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemContet : MonoBehaviour
{
    public Text fileName;
    public GameObject[] states;
    public Text advice;
    public void Show(string _fileName)
    {
        fileName.text = _fileName;
        ClearState();
        gameObject.SetActive(true);
    }

    public void ClearState()
    {
        states[0].SetActive(false);
        states[1].SetActive(false);
        states[2].SetActive(false);
        advice.text = "";
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        ClearState();
    }
    // Update is called once per frame
    public void UpState(InstallState installState,string installAdvice)
    {
        switch (installState)
        {
            case InstallState.Null:
                states[0].SetActive(false);
                states[1].SetActive(false);
                states[2].SetActive(false);
                break;
            case InstallState.Installing:
                states[0].SetActive(true);
                states[1].SetActive(false);
                states[2].SetActive(false);
                break;
            case InstallState.Success:
                states[0].SetActive(false);
                states[1].SetActive(true);
                states[2].SetActive(false);
                break;
            case InstallState.Failure:
            case InstallState.OutMemory:
                states[0].SetActive(false);
                states[1].SetActive(false);
                states[2].SetActive(true);
                break;
            default:
                break;
        }

        advice.text = installAdvice;
    }

    public void SetAdviceInfo( string message)
    {
        advice.text = message;
    }
}
