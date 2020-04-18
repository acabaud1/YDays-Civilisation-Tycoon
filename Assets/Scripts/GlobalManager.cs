﻿using System.Collections;
using System.Collections.Generic;
using Map;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalManager : MonoBehaviour
{
    private MapManager mapManager;
    private ResourceManager resourceManager;

    private BuildingManager buildingManager;
    private ClickableSingleton clickableSingleton;
    public LayerMask layerMask;

    public GameObject rock;
    public GameObject sand;
    public GameObject tree;
    public GameObject water;
    public GameObject land;
    public GameObject ironOre;
    public GameObject[] LandDoodads;

    public TileModel[,] TileArray;

    // Start is called before the first frame update
    void Start()
    {
        MapManagerInit();

        buildingManager = BuildingManager.GetInstance();
        clickableSingleton = ClickableSingleton.GetInstance();

        buildingManager.layerMask = layerMask;
        buildingManager.TileArray = TileArray;

        if (SceneManager.sceneCount < 2)
        {
            SceneManager.LoadScene("UiScene", LoadSceneMode.Additive);
        }
        
    }

    void Update()
    {
        buildingManager.Update();
        clickableSingleton.Update();
    }

    private void MapManagerInit()
    {
        resourceManager = ResourceManager.GetInstance();
        mapManager = MapManager.GetInstance();

        mapManager.rock = rock;
        mapManager.sand = sand;
        mapManager.tree = tree;
        mapManager.water = water;
        mapManager.land = land;
        mapManager.ironOre = ironOre;
        mapManager.LandDoodads = LandDoodads;

        TileArray = new TileModel[mapManager.width, mapManager.height];
        mapManager.TileArray = TileArray;

        mapManager.Start();
    }
}
