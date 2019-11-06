using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGeneration : MonoBehaviour
{
    public int nbOfChunksPerRow = 4;

    public int width = 64;
    public int height = 64;

    public Transform map;
    private Object[,] mapArray;

    public Object land;
    public Object sand;
    public Object water;
    public Object tree;
    public Object ironOre;

    public float zoom = 20f;
    public float pnThreshold = 0.65f;

    private float offsetX;
    private float offsetY;

    IEnumerator GenerateChunk(int m, int n)
    {
        for (int x = ((width / nbOfChunksPerRow) * m); x < ((width / nbOfChunksPerRow) * (m + 1)); x++)
        {
            for (int y = ((height / nbOfChunksPerRow) * n); y < ((height / nbOfChunksPerRow) * (n + 1)); y++)
            {
                float pnValue = CalcPerlin(x, y);

                if (pnValue > pnThreshold)
                {
                    mapArray[x, y] = Instantiate(water, new Vector3(x, 0, y), Quaternion.identity, map);
                }
                else if (pnValue > pnThreshold - 0.05f)
                {
                    mapArray[x, y] = Instantiate(sand, new Vector3(x, 0, y), Quaternion.identity, map);
                }
                else
                {
                    mapArray[x, y] = Instantiate(land, new Vector3(x, 0, y), Quaternion.identity, map);
                }
            }
        }
        yield return new WaitForSeconds(0);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        mapArray = new GameObject[width, height];

        offsetX = Random.Range(0f, 999999f);
        offsetY = Random.Range(0f, 999999f);

        for (int m = 0; m < nbOfChunksPerRow; m++)
        {
            for (int n = 0; n < nbOfChunksPerRow; n++)
            {
                StartCoroutine(GenerateChunk(m % nbOfChunksPerRow, n % nbOfChunksPerRow));
            }
        }

        offsetX = Random.Range(0f, 999999f);
        offsetY = Random.Range(0f, 999999f);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float pnValue = CalcPerlin(x, y);

                if (pnValue < 0.3f)
                {
                    if (mapArray[x, y].name == "Land(Clone)")
                    {
                        Instantiate(tree, new Vector3(x, 1.3f, y), Quaternion.identity, map);
                    }
                }
            }
        }

        offsetX = Random.Range(0f, 999999f);
        offsetY = Random.Range(0f, 999999f);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float pnValue = CalcPerlin(x, y);

                if (pnValue < 0.1f)
                {
                    if (mapArray[x, y].name == "Land(Clone)")
                    {
                        Instantiate(ironOre, new Vector3(x, 1.3f, y), Quaternion.identity, map);
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
