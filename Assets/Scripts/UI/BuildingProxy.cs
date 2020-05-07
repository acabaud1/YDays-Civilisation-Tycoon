using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Building;
using UnityEngine;

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
