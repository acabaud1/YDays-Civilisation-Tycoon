using System.Collections;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Algorithme de génération de terrain utilisant la méthode de génération de bruit de Perlin.
/// Ce script est rattaché au MapManager
/// </summary>
public class MapGeneration : MonoBehaviour
{
    public int nbOfChunksPerRow = 4;
    public int width = 64;
    public int height = 64;

    public Transform map;
    private GameObject[,] mapArray;
    private GameObject[,] obstacles;

    public GameObject land;
    public GameObject sand;
    public GameObject water;
    public GameObject tree;
    public GameObject ironOre;
    public GameObject robot;

    public float zoom = 20f;
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
                float pnValue = CalcPerlin(x, y);

                if (pnValue > pnThreshold)
                {
                    mapArray[x, y] = Instantiate(water, new Vector3(x, 0 - 0.02f, y), Quaternion.identity, map);
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

        GenerateDoodads(tree, 0.3f);
        GenerateDoodads(ironOre, 0.1f);

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
        //Debug.Log(agent.isOnNavMesh);
        agent.destination = new Vector3(60, 0.70f, 60);
    }

    // 1+n itérations pour générer les différents éléments.
    // requiredPnValue = La valeur renvoyé par le calcul de Perlin à laquelle vous souhaitez placer vos éléments.
    private void GenerateDoodads(GameObject Doodads, float requiredPnValue)
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
                    float pnValue = CalcPerlin(x, y);

                    if (pnValue < requiredPnValue)
                    {
                        if (mapArray[x, y].name == "Land(Clone)")
                        {
                            obstacles[x, y] = Instantiate(Doodads, new Vector3(x, doodadHeight + plateformTop, y), Quaternion.identity, map);
                            obstacles[x, y].AddComponent<NavMeshObstacle>().carving = true;
                        }
                    }
                }
            }
        }

    }

    private float CalcPerlin(int x, int y)
    {

        float xCoord = (float)x / width * zoom + offsetX;
        float yCoord = (float)y / width * zoom + offsetY;

        return Mathf.PerlinNoise(xCoord, yCoord);
    }

}