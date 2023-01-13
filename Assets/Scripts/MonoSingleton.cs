using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    /// The instance.
    /// </summary>
    protected static T instance = null;

    /// <summary>
    /// Gets the instance, this operation will create instance and set don't destory on load if not exist.
    /// </summary>
    /// <value>The instance.</value>
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType(typeof(T)) as T;
                if (instance == null)
                {
                    instance = new GameObject("_Auto_" + typeof(T).Name).AddComponent<T>();
                }
                if (instance == null)
                {
                    Debug.LogError("Failed to create instance of " + typeof(T).FullName + ".");
                    return null;
                }

                if (instance.gameObject.transform.parent == null)
                {
                    DontDestroyOnLoad(instance.gameObject);
                }
            }
            return instance;
        }
    }

    /// <summary>
    /// Ons the application quit.
    /// </summary>
    void OnApplicationQuit()
    {
        if (instance != null)
        {
            instance = null;
        }
    }
}
