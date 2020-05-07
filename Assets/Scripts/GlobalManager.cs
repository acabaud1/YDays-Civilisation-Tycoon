using System.Collections;
using System.Collections.Generic;
using Map;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalManager : MonoBehaviour
{
    private MapManager mapManager;
    private ResourceManager resourceManager;
    private BuildingManager buildingManager;
    private PNJManager pnjManager;
    private ClickableSingleton clickableSingleton;
    public LayerMask layerMask;

    public GameObject hub;
    public GameObject rock;
    public GameObject sand;
    public GameObject tree;
    public GameObject water;
    public GameObject land;
    public GameObject ironOre;
    private DoodadProbability[] ProbilityLandDoodads;
    public GameObject[] LandDoodads;
    public GameObject[] animals;
    public GameObject[] robots;
    public GameObject[] humans;

    public TileModel[,] TileArray;

    private bool isInit = false;

    // Start is called before the first frame update
    void Start()
    {
        MapManagerInit();

        buildingManager = BuildingManager.GetInstance();
        clickableSingleton = ClickableSingleton.GetInstance();

        buildingManager.layerMask = layerMask;
        buildingManager.TileArray = TileArray;
        buildingManager.ResourceManager = resourceManager;

        pnjManager = PNJManager.GetInstance();
        pnjManager.animals = animals;
        pnjManager.robots = robots;
        pnjManager.humans = humans;

        var human = pnjManager.CreateHuman(new Vector3(1, 1, 1));
        var ennemy = pnjManager.CreateHuman(new Vector3(1, 1, 3));

        buildingManager.pnjManager = pnjManager;
        if (SceneManager.sceneCount < 2)
        {
            SceneManager.LoadScene("UiScene", LoadSceneMode.Additive);
        }
        
        isInit = true;
        InitHub();
    }

    void Update()
    {
        if (isInit)
        {
            buildingManager.Update();
            pnjManager.Update();
            clickableSingleton.Update();
        }
    }

    private void InitHub()
    {
        buildingManager.SetBuilding(hub);
    }

    private void MapManagerInit()
    {
        resourceManager = ResourceManager.GetInstance();

        resourceManager.AddAndDistribute(typeof(Iron), 50);
        resourceManager.AddAndDistribute(typeof(Wood), 50);
        resourceManager.AddAndDistribute(typeof(Stone), 50);
        
        mapManager = MapManager.GetInstance();

        List<DoodadProbability> doodads  =new List<DoodadProbability>();
        foreach (var landDoodad in LandDoodads)
        {
            doodads.Add(new DoodadProbability
            {
                GameObject = landDoodad,
                Probability = 1
            });
        }

        ProbilityLandDoodads = doodads.ToArray();

        mapManager.rock = rock;
        mapManager.sand = sand;
        mapManager.tree = tree;
        mapManager.water = water;
        mapManager.land = land;
        mapManager.ironOre = ironOre;
        mapManager.LandDoodads = ProbilityLandDoodads;

        TileArray = new TileModel[mapManager.width, mapManager.height];
        mapManager.TileArray = TileArray;

        mapManager.Start();
    }
}
