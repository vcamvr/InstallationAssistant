using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemInfo : MonoBehaviour
{
    public static SystemInfo Instance;

    private bool deviceLinkState;
    private static float availROMSize;
    void Awake()
    {
        Instance = this;
    }

    public void SetAvailRom(float size)
    {
        availROMSize = size;
    }

    public float GetAvailRom()
    {
        return availROMSize;
    }
}
