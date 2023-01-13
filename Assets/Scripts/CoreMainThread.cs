using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreMainThread : MonoSingleton<CoreMainThread>
{
    // Start is called before the first frame update
    public VeerAction<int, InstallState> InstallVeerAction;
    private List<VeerAction<int, InstallState>> _OnceTask = new List<VeerAction<int, InstallState>>();
    private List<VeerAction<int, InstallState>> _PreAddOnceTask = new List<VeerAction<int, InstallState>>();

    public void RegisterInstallEvent(Action<int,InstallState> action)
    {
        InstallVeerAction = new VeerAction<int, InstallState>((index, installState) =>
          {
              action(index, installState);
          });
    }


    public void AddOnceTask(VeerAction<int, InstallState> task)
    {
        if (task == null)
            return;

        lock (_PreAddOnceTask)
        {
            _PreAddOnceTask.Add(task);
        }
    }

    private void Update()
    {
        lock (_OnceTask)
        {
            lock (_PreAddOnceTask)
            {
                foreach (var task in _PreAddOnceTask)
                    _OnceTask.Add(task);
                _PreAddOnceTask.Clear();
            }

        }
    }

    public void RegisterInstallEvent(VeerAction<int, InstallState> veerAction)
    {
        InstallVeerAction = veerAction;
    }

    public void DoInstallEvent(int _index,InstallState installState)
    {
        for (int index = 0; index < _OnceTask.Count; index++)
        {
            _OnceTask[index].Invoke(_index, installState);
        }
    }
    public void UnRegisterInstallEvent()
    {
        InstallVeerAction = null;
    }
}
