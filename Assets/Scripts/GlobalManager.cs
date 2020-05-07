using System.Collections.Generic;
using Assets.Scripts.Building;
using Assets.Scripts.Building.Models;
using Map;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
///     Gestion global et génération des différents manager.
/// </summary>
public class GlobalManager : MonoBehaviour
{
    private BuildingManager _buildingManager;
    private ClickableSingleton _clickableSingleton;

    private bool _isInit;
    private MapManager _mapManager;
    private PNJManager _pnjManager;
    private DoodadProbability[] _probilityLandDoodads;
    private ResourceManager _resourceManager;
    public GameObject[] Animals;

    public GameObject Hub;
    public GameObject[] Humans;
    public GameObject IronOre;
    public GameObject Land;
    public GameObject[] LandDoodads;
    public LayerMask LayerMask;
    public GameObject[] Robots;
    public GameObject Rock;
    public GameObject Sand;

    public TileModel[,] TileArray;
    public GameObject Tree;
    public GameObject Water;

    /// <summary>
    ///     Fonction appelé a la création du GameObject.
    /// </summary>
    private void Start()
    {
        MapManagerInit();

        _buildingManager = BuildingManager.GetInstance();
        _clickableSingleton = ClickableSingleton.GetInstance();

        _buildingManager.LayerMask = LayerMask;
        _buildingManager.TileArray = TileArray;
        _buildingManager.ResourceManager = _resourceManager;

        _pnjManager = PNJManager.GetInstance();
        _pnjManager.animals = Animals;
        _pnjManager.robots = Robots;
        _pnjManager.humans = Humans;

        var human = _pnjManager.CreateHuman(new Vector3(1, 1, 1));
        var ennemy = _pnjManager.CreateHuman(new Vector3(1, 1, 3));

        _buildingManager.PnjManager = _pnjManager;
        if (SceneManager.sceneCount < 2) SceneManager.LoadScene("UiScene", LoadSceneMode.Additive);

        _isInit = true;
        InitHub();
    }

    /// <summary>
    ///     Fonction appelé a chaque frame du jeux.
    /// </summary>
    private void Update()
    {
        if (_isInit)
        {
            _buildingManager.Update();
            _pnjManager.Update();
            _clickableSingleton.Update();
        }
    }

    /// <summary>
    ///     Initialize la gestion des batiment pour pouvoir poser le hub.
    /// </summary>
    private void InitHub()
    {
        _buildingManager.SetBuilding(Hub);
    }

    /// <summary>
    ///     Initialisation du Map Manager.
    /// </summary>
    private void MapManagerInit()
    {
        _resourceManager = ResourceManager.GetInstance();

        _resourceManager.AddAndDistribute(typeof(Iron), 50);
        _resourceManager.AddAndDistribute(typeof(Wood), 50);
        _resourceManager.AddAndDistribute(typeof(Stone), 50);

        _mapManager = MapManager.GetInstance();

        var doodads = new List<DoodadProbability>();
        foreach (var landDoodad in LandDoodads)
            doodads.Add(new DoodadProbability
            {
                GameObject = landDoodad,
                Probability = 1
            });

        _probilityLandDoodads = doodads.ToArray();

        _mapManager.rock = Rock;
        _mapManager.sand = Sand;
        _mapManager.tree = Tree;
        _mapManager.water = Water;
        _mapManager.land = Land;
        _mapManager.ironOre = IronOre;
        _mapManager.LandDoodads = _probilityLandDoodads;

        TileArray = new TileModel[_mapManager.width, _mapManager.height];
        _mapManager.TileArray = TileArray;

        _mapManager.Start();
    }
}