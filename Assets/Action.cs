using System;
using System.Runtime.InteropServices;  
using System.Collections.Generic;
using UnityEngine;

public class Action
{
    public string displayName;

    public List<Delegate> callbacks = new List<Delegate>();

    Action(string displayName, List<Delegate> callbacks)
    {
        this.displayName = displayName;
        this.callbacks = callbacks;
    }

    private void OpenDrawer()
    {
        var Drawer = GameObject.Find("Drawer").GetComponent<Drawer>();
        if (Drawer)
        {
            // Drawer.OnOpen(callbacks);
        }
    }   

}
