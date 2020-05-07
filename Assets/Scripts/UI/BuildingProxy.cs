using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Building;
using UnityEngine;

/// <summary>
/// This class is used for create Buildings
/// You should link your trigger of build like buttons to this SetBuilding methods for use it.
/// </summary>
public class BuildingProxy : MonoBehaviour
{
    private BuildingManager buildingManager;

    void Start()
    {
        buildingManager = BuildingManager.GetInstance();
    }

    public void SetBuilding(GameObject building)
    {
        buildingManager.SetBuilding(building);
    }

    public void ToggleDeleteMod()
    {
        buildingManager.ToggleDeleteMod();
    }
}
