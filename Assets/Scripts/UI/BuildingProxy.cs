using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingProxy : MonoBehaviour
{
    private BuildingManager buildingManager;
    // Start is called before the first frame update
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
