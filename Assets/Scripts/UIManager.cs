using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public Transform content;
    public ItemContet itemContet;
    public GameObject nullContent;
    public Text deviceState;
    public Button xpressInstall;
    public GameObject scanWindows;
    private string filePath;
    private InstallState installState;

    public List<PackageInfo> packageInfos = new List<PackageInfo>();
    private List<ItemContet> itemContets=new List<ItemContet>();

    void Awake()
    {
        Instance = this;
        xpressInstall.gameObject.SetActive(false);
        itemContet.gameObject.SetActive(false);
        nullContent.SetActive(false);
        scanWindows.SetActive(false);
        installState = InstallState.Null;
    }


    public void Initialized()
    {
        packageInfos.Clear(); 
        filePath = ProjectPath.GetFilePath();
        if (Directory.Exists(filePath))
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(filePath);
            FileInfo[] fileInfos = directoryInfo.GetFiles("*", SearchOption.AllDirectories);
            foreach (var item in fileInfos)
            {
                if (item.Name.EndsWith(".apk"))
                {
                    Debug.Log("item.FullName:"+ item.FullName);
                    packageInfos.Add(new PackageInfo(item.FullName, item.Name));
                }
            }
        }

        if (packageInfos.Count>0)
        {
            /*if (itemContets.Count > 1)
            {
                for (int index = 1; index < itemContets.Count; index++)
                {
                    DestroyImmediate(itemContets[index].gameObject);
                }
                itemContets.Clear();
            }
            itemContet.gameObject.SetActive(true);
            itemContet.Show(packageInfos[0].fileName);
           
            itemContets.Add(itemContet);
            for (int index = 1; index < packageInfos.Count; index++)
            {
                ItemContet item =Instantiate(itemContet, content);
                item.Show(packageInfos[index].fileName);
                item.gameObject.SetActive(true);
                itemContets.Add(item);
            }*/

            CloneItems();
            xpressInstall.gameObject.SetActive(true);
        }
        else
        {
            nullContent.SetActive(true);
            itemContet.gameObject.SetActive(false);
        }
            HideScanUI();
        //UpdateDeviceState(AdbManager.isLinkSuccesss);
        //CoreMainThread.Instance.RegisterInstallEvent(UICallback);
    }

    public void CloneItems()
    {
     /*   if (itemContets.Count>0)
        {*/
            /*for (int index = 0; index < itemContets.Count; index++)
            {
                itemContets[index].ClearState();
            }*/

            if (itemContets.Count <=packageInfos.Count)
            {
                for (int index =0; index < packageInfos.Count; index++)
                {
                    int temp = index;
                    if (temp >= itemContets.Count)
                    {
                        ItemContet item = Instantiate(itemContet, content);
                        itemContets.Add(item);
                        item.Show(packageInfos[temp].fileName);
                        
                    }
                    else
                    {
                        itemContets[temp].Show(packageInfos[temp].fileName);
                    }
                }
            }
            else
            {
                for (int index = 0; index < itemContets.Count; index++)
                {
                    int temp = index;
                    if (temp >= packageInfos.Count)
                    {
                        itemContets[temp].Hide();
                    }
                    else
                    {
                        itemContets[temp].Show(packageInfos[temp].fileName);
                    }
                  
                }
            }
        //}
    }
    public void ShowScanUI()
    {
        scanWindows.SetActive(true);
    }
    public void HideScanUI()
    {
        scanWindows.SetActive(false);
    }


    public void InstallOver(InstallState state)
    {
        installState = state;
    }

    public void UpdateDeviceState(bool state)
    {
        if (state)
        {
            //xpressInstall.gameObject.SetActive(true);
            deviceState.text = "设备已连接";
        }
        else
        {
            xpressInstall.gameObject.SetActive(false);
            deviceState.text = "设备未就绪";
        }
    }

    public void OnClick_XpressInstall()
    {
        xpressInstall.gameObject.SetActive(false);
        FileManager.InstallApkFiles(packageInfos);
        InvokeRepeating("UpdateInstallState",0,0.5f);
    }

    public void UpdateInstallState()
    {
      
        for (int index = 0; index < itemContets.Count; index++)
        {
            int temp = index;
            itemContets[temp].UpState(packageInfos[temp].installState,packageInfos[temp].installAdvice);
        }

        if (FileManager.processInfo == ProcessInfo.installOver)
        {
            for (int index = 0; index < packageInfos.Count; index++)
            {
                if (packageInfos[index].installState == InstallState.OutMemory)
                {
                    ReShowInstallButton();
                    break;
                }
            }
            CancelInvoke("UpdateInstallState");
            return;
        }
    }

    public void ReShowInstallButton()
    {
        //FileManager.MinSize = 5;
        xpressInstall.gameObject.SetActive(true);
    }
    public void OnClick_ExitApp()
    {
        Application.Quit();
    }

   
}
