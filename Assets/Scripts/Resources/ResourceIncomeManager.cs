using Assets.Scripts.Resources;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

public class ResourceIncomeManager : MonoBehaviour
{
    public float Radius;
    public float WaitingTime;
    public int NbByResources;
    public RessourceEnum RessourceEnum = RessourceEnum.None;
    private GameObject BuildingManager;
    private BuildingManager buildingManagerScript;
    private GameObject ResourceManager;
    private ResourceManager resourceManagerScript;
    private int nbOres;
    
    // Start is called before the first frame update
    void Start()
    {
        try
        {
            BuildingManager = GameObject.Find("Building Manager");
            buildingManagerScript = BuildingManager.GetComponent<BuildingManager>();

            ResourceManager = GameObject.Find("Resource Manager");
            resourceManagerScript = ResourceManager.GetComponent<ResourceManager>();

            getAllResources();
            InvokeRepeating(nameof(AddResources), WaitingTime, WaitingTime);
        }
        catch (Exception e)
        {
            Debug.Log(e);
            throw e;
        }
    }

    private bool getGameObjetWithRessourceEnum(GameObject go)
    {
        RessourceType ressourceType = go.GetComponent<RessourceType>();
        if(ressourceType != null && ressourceType.Ressource == RessourceEnum)
        {
            return true;
        }
        return false;
    }

    void getAllResources()
    {
        nbOres = 0;
        foreach (GameObject element in buildingManagerScript.Doodads.Where(go => getGameObjetWithRessourceEnum(go)))
        {
            if (Vector3.Distance(element.transform.position, this.transform.position) < Radius)
            {
                nbOres++;
            }
        }
    }

    void AddResources()
    {
        int resources = nbOres * NbByResources;
        resourceManagerScript.Add(RessourceHelper.GetRessourceGameTypeFromRessourceEnum(RessourceEnum), resources);
    }
}
