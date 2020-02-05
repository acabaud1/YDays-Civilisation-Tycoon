using System.Collections;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Algorithme de génération de terrain utilisant la méthode de génération de bruit de Perlin.
/// Ce script est rattaché au MapManager
/// </summary>
public class MapGeneration : MonoBehaviour
{
    public GameObject _buildingManager;
    private BuildingManager buildingManagerScript;

    public int nbOfChunksPerRow = 4;
    public int width = 64;
    public int height = 64;

    public Transform map;
    private GameObject[,] mapArray;
    private GameObject[,] obstacles;

    public GameObject land;
    public GameObject sand;
    public GameObject water;
    public GameObject waterCollider;
    public GameObject tree;
    public GameObject ironOre;
    public GameObject robot;
    public GameObject rock;

    // Plus la valeur est faible plus les ressources sont rapprochés et abondantes
    public float mapZoom = 10f;
    public float treeZoom = 20f;
    public float ironZoom = 40f;
    public float rockZoom = 40f;
    public float pnThreshold = 0.65f;

    private float offsetX;
    private float offsetY;
    
    // Couroutine servant à générer tous les éléments qui seront placés au niveau du sol.
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
                    buildingManagerScript.Doodads.Add(buildingWater);
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
        buildingManagerScript = _buildingManager.GetComponent<BuildingManager>();
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

        GenerateDoodads(tree, 0.3f, treeZoom, RessourceEnum.Wood);
        GenerateDoodads(ironOre, 0.1f, ironZoom, RessourceEnum.Iron);
        GenerateDoodads(rock, 0.1f, rockZoom, RessourceEnum.Stone);

        StartCoroutine(PlaceRobot());
    }

    /// <summary>
    /// Provisoire : Place un robot sur la map en position 1,1 en NavMeshAgent et règle la destination sur 60,60
    /// </summary>
    private bool isCoroutineExecuting = false;

    IEnumerator PlaceRobot()
    {
        if (isCoroutineExecuting)
        {
            yield break;
        }

        isCoroutineExecuting = true;

        yield return new WaitForSeconds(10);

        isCoroutineExecuting = false;

        robot = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        robot.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        robot.transform.position = new Vector3(1, 0.70f, 1);

        NavMeshAgent agent = robot.AddComponent<NavMeshAgent>();
        agent.destination = new Vector3(60, 0.70f, 60);
    }

    // 1+n itérations pour générer les différents éléments.
    // requiredPnValue = La valeur renvoyé par le calcul de Perlin à laquelle vous souhaitez placer vos éléments.
    private void GenerateDoodads(GameObject Doodads, float requiredPnValue, float zoom, RessourceEnum ressourceEnum = RessourceEnum.None)
    {
        offsetX = Random.Range(0f, 999999f);
        offsetY = Random.Range(0f, 999999f);
        var doodadsRenderer = Doodads.GetComponent<Renderer>();
        var landRenderer = land.GetComponent<Renderer>();
        if (doodadsRenderer && landRenderer)
        {
            float doodadHeight = doodadsRenderer.bounds.size.y / 2;
            float plateformTop = landRenderer.bounds.size.y / 2;
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
                            RessourceType ressourceTypeScript = newDoodads.AddComponent<RessourceType>() as RessourceType;
                            if(ressourceTypeScript != null)
                            {
                                ressourceTypeScript.Ressource = ressourceEnum;
                            }
                            newDoodads.AddComponent<NavMeshObstacle>().carving = true;
                            buildingManagerScript.Doodads.Add(newDoodads);
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