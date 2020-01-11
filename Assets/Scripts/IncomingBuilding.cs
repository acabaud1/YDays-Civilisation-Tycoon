using UnityEngine;
using System;

public class IncomingBuilding : MonoBehaviour
{

    public string ResourceName;
    public ResourceManager ResourceManager;
    private ResourcesGame _resource;

    void Start()
    {
        try
        {
            Type ResourceType = Type.GetType(ResourceName);
            ResourceManager.Get(ResourceType);
        } 
        catch (System.NullReferenceException e)
        {
            Debug.LogError($"Le type {ResourceName} n'existe pas.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
