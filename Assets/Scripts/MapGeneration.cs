using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Algorithme de génération de terrain utilisant la méthode de génération de bruit de Perlin.
/// Ce script est rattaché au MapManager
/// </summary>
public class MapGeneration : MonoBehaviour
{
    public GameObject _buildingManager;

    public int nbOfChunksPerRow = 4;
    public int width = 64;
    public int height = 64;

    List<Animal> _animals = new List<Animal>();
    List<Robot> _robots = new List<Robot>();

    public int nbRobots = 10;
    public int nbAnimals = 10;

    public Transform map;
    private GameObject[,] mapArray;
    private GameObject[,] obstacles;

    public GameObject land;
    public GameObject sand;
    public GameObject water;
    public GameObject waterCollider;
    public GameObject tree;
    public GameObject ironOre;
    public GameObject rock;

    // Animaux
    //public GameObject bison;
    //public GameObject hippopotamus;
    public GameObject[] animals;

    // Robots
    //public GameObject raptoRobot;
    //public GameObject catRobot1;
    //public GameObject catRobot2;
    public GameObject[] robots;

    // Plus la valeur est faible plus les ressources sont rapprochés et abondantes
    public float mapZoom = 10f;
    public float treeZoom = 20f;
    public float ironZoom = 40f;
    public float rockZoom = 40f;
    public float pnThreshold = 0.65f;

    private float offsetX;
    private float offsetY;
    private bool _isCoroutineExecuting = false;

    // Couroutine servant à générer tous les éléments qui seront placés au niveau du sol
    void GenerateChunk(int m, int n)
    {
        for (int x = ((width / nbOfChunksPerRow) * m); x < ((width / nbOfChunksPerRow) * (m + 1)); x++)
        {
            for (int y = ((height / nbOfChunksPerRow) * n); y < ((height / nbOfChunksPerRow) * (n + 1)); y++)
            {
                float pnValue = CalcPerlin(x, y, mapZoom);

                if (pnValue > pnThreshold)
                {
                    mapArray[x, y] = Instantiate(water, new Vector3(x, 0 - 0.02f, y), Quaternion.identity, map);
                    var buildingWater = Instantiate(waterCollider, new Vector3(x, 1, y), Quaternion.identity, map);
                    _buildingManager.GetComponent<BuildingManager>().Doodads.Add(buildingWater);
                }
                else if (pnValue > pnThreshold - 0.05f)
                {
                    mapArray[x, y] = Instantiate(sand, new Vector3(x, 0 - 0.01f, y), Quaternion.identity, map);
                }
                else
                {
                    mapArray[x, y] = Instantiate(land, new Vector3(x, 0, y), Quaternion.identity, map);
                }
            }
        }
    }

    void Start()
    {
        mapArray = new GameObject[width, height];
        obstacles = new GameObject[width, height];

        offsetX = Random.Range(0f, 999999f);
        offsetY = Random.Range(0f, 999999f);

        // 1er passage pour génération du sol (sable, terre, eau)
        for (int m = 0; m < nbOfChunksPerRow; m++)
        {
            for (int n = 0; n < nbOfChunksPerRow; n++)
            {
                GenerateChunk(m % nbOfChunksPerRow, n % nbOfChunksPerRow);
            }
        }

        GenerateDoodads(tree, 0.3f, treeZoom);
        GenerateDoodads(ironOre, 0.1f, ironZoom);
        GenerateDoodads(rock, 0.1f, rockZoom);

        StartCoroutine(RandomPnj());
    }

    void Update()
    {
        // Animaux
        foreach (Animal myAnimal in _animals)
        {
            myAnimal.CheckDestination();
        }

        // Robots
        foreach (Robot myRobot in _robots)
        {
            myRobot.CheckDestination();
        }
    }

    private IEnumerator RandomPnj()
    {
        if (_isCoroutineExecuting)
          yield break;

        _isCoroutineExecuting = true;

        yield return new WaitForSeconds(5);

        _isCoroutineExecuting = false;

        // Animaux
        for (var i = 0; i < nbAnimals; i++)
        {
            var randomAnimal = new Animal(Instantiate(animals[Random.Range(0, animals.Length)]));
            _animals.Add(randomAnimal);
            randomAnimal.Spawn(Random.Range(0, width), Random.Range(0, height));
        }

        // Robots
        for (var i = 0; i < nbRobots; i++)
        {
            var randomRobot = new Robot(Instantiate(robots[Random.Range(0, robots.Length)]));
            _robots.Add(randomRobot);
            randomRobot.Spawn(Random.Range(0, width), Random.Range(0, height));
        }
    }

    // 1+n itérations pour générer les différents éléments.
    // requiredPnValue = La valeur renvoyé par le calcul de Perlin à laquelle vous souhaitez placer vos éléments.
    private void GenerateDoodads(GameObject Doodads, float requiredPnValue, float zoom)
    {
        offsetX = Random.Range(0f, 999999f);
        offsetY = Random.Range(0f, 999999f);

        if (Doodads.GetComponent<Renderer>() && land.GetComponent<Renderer>())
        {
            float doodadHeight = Doodads.GetComponent<Renderer>().bounds.size.y / 2;
            float plateformTop = land.GetComponent<Renderer>().bounds.size.y / 2;

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    float pnValue = CalcPerlin(x, y, zoom);

                    if (pnValue < requiredPnValue)
                    {
                        if (mapArray[x, y].name == "Land(Clone)")
                        {
                            var newDoodads = Instantiate(Doodads, new Vector3(x, doodadHeight + plateformTop, y), Quaternion.Euler(0, Random.Range(0.0f, 360.0f), 0), map);
                            newDoodads.AddComponent<NavMeshObstacle>().carving = true;
                            _buildingManager.GetComponent<BuildingManager>().Doodads.Add(newDoodads);
                        }
                    }
                }
            }
        }
    }

    private float CalcPerlin(int x, int y, float zoom)
    {

        float xCoord = (float)x / width * zoom + offsetX;
        float yCoord = (float)y / width * zoom + offsetY;

        return Mathf.PerlinNoise(xCoord, yCoord);
    }

}
