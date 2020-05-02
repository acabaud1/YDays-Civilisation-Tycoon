using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StockManager : ResourceManagerCore
{
    private ResourceManager resourceManagerScript = ResourceManager.GetInstance();

    // Start is called before the first frame update
    void Start()
    {
        try
        {
            resourceManagerScript.ResourceManagerCores.Add(this);
        }
        catch (Exception e)
        {
            Debug.LogError($"Le building Manager ou le ressource Manager ne sont pas présent dans la scène actuelle.");
            Debug.LogError(e);
            throw e;
        }
    }

    private void OnDestroy()
    {
        resourceManagerScript.ResourceManagerCores.Remove(this);
    }
}
