using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class VeerDelegateBase
{
    private string _Name = null;
    public string Name
    {
        get
        {
            if (string.IsNullOrEmpty(_Name))
                return string.Empty;
            return _Name;
        }
    }
    protected VeerDelegateBase(string name = null) { _Name = name; }
}
public class VeerAction : VeerDelegateBase
{
    public Action Action;
    public VeerAction(Action action, string name = null) : base(name) { Action = action; }

    public void Invoke()
    {
       Invoke_Internal();
    }

    protected void Invoke_Internal()
    {
        if (Action != null)
            Action.Invoke();
    }

}

public class VeerAction<P1> : VeerDelegateBase
{
    public Action<P1> Action;
    public VeerAction(Action<P1> action, string name = null) : base(name)
    {
        Action = action;
    }

    public void Invoke(P1 p1)
    {
        Invoke_Internal(p1);
    }

    protected void Invoke_Internal(P1 p1)
    {
        if (Action != null)
            Action.Invoke(p1);
    }

}


public class VeerAction<P1, P2> : VeerDelegateBase
{
    public Action<P1, P2> Action;
    public VeerAction(Action<P1, P2> action, string name = null) : base(name)
    {
        Action = action;
    }

    public void Invoke(P1 p1, P2 p2)
    {
  
            Invoke_Internal(p1, p2);
    }

    protected void Invoke_Internal(P1 p1, P2 p2)
    {
        if (Action != null)
            Action.Invoke(p1, p2);
    }
}

public class VeerAction<P1, P2, P3> : VeerDelegateBase
{
    public Action<P1, P2, P3> Action;
    public VeerAction(Action<P1, P2, P3> action, string name = null) : base(name)
    {
        Action = action;
    }

    public void Invoke(P1 p1, P2 p2, P3 p3)
    {
        Invoke_Internal(p1, p2, p3);
    }

    protected void Invoke_Internal(P1 p1, P2 p2, P3 p3)
    {
        if (Action != null)
            Action.Invoke(p1, p2, p3);
    }

}
