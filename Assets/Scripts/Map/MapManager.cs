using System.Linq;
using Ressource;
using UnityEngine;
using UnityEngine.AI;

namespace Map
{
    /// <summary>
    ///     Gestion de la map.
    /// </summary>
    public class MapManager
    {
        /// <summary>
        ///     Instance de la gestion de la map.
        /// </summary>
        private static MapManager instance;

        private readonly float pnThreshold = 0.65f;
        private readonly float rockZoom = 40f;
        private readonly float treeZoom = 20f;
        public GameObject ironOre;
        private readonly float ironZoom = 40f;
        public GameObject land;
        public Transform map;

        // Plus la valeur est faible plus les ressources sont rapprochés et abondantes
        private readonly float mapZoom = 10f;
        private readonly int nbOfChunksPerRow = 4;
        private float offsetX;
        private float offsetY;

        public GameObject rock;
        public GameObject sand;

        public int width = 64;
        public int height = 64;
        public TileModel[,] TileArray;
        public GameObject tree;
        public GameObject water;

        private int landDoodadsProbability = 50;
        public DoodadProbability[] LandDoodads;

        /// <summary>
        ///     Récupère l'instance du map manager.
        /// </summary>
        /// <returns></returns>
        public static MapManager GetInstance()
        {
            if (instance == null) instance = new MapManager();

            return instance;
        }

        // Couroutine servant à générer tous les éléments qui seront placés au niveau du sol.
        private void GenerateChunk(int m, int n)
        {
            for (var x = width / nbOfChunksPerRow * m; x < width / nbOfChunksPerRow * (m + 1); x++)
            for (var y = height / nbOfChunksPerRow * n; y < height / nbOfChunksPerRow * (n + 1); y++)
            {
                var pnValue = CalcPerlin(x, y, mapZoom);

                if (TileArray[x, y] == null)
                {
                    TileArray[x, y] = new TileModel();
                    TileArray[x, y].X = x;
                    TileArray[x, y].Z = y;
                }

                if (pnValue > pnThreshold)
                {
                    TileArray[x, y].TileEnum = TileEnum.Water;
                    TileArray[x, y].Tile =
                        GameObject.Instantiate(water, new Vector3(x, 0, y), Quaternion.Euler(new Vector3(90, 0, 0)),
                            map);
                }
                else if (pnValue > pnThreshold - 0.05f)
                {
                    TileArray[x, y].TileEnum = TileEnum.Sand;
                    TileArray[x, y].Tile = GameObject.Instantiate(sand, new Vector3(x, 0, y), Quaternion.identity, map);
                }
                else
                {
                    TileArray[x, y].TileEnum = TileEnum.Land;
                    TileArray[x, y].Tile = GameObject.Instantiate(land, new Vector3(x, 0, y), Quaternion.identity, map);

                    if (Random.Range(0, 100) >= landDoodadsProbability)
                    {
                        int probabilitySum = LandDoodads.Sum(s => s.Probability);
                        int selectedDoodadNumber = Random.Range(0, probabilitySum);
                        GameObject selectedDoodad = null;
                        int doodadNumber = 0;
                        foreach (var doodadProbability in LandDoodads)
                        {
                            doodadNumber = doodadNumber + doodadProbability.Probability;
                            if (doodadNumber >= selectedDoodadNumber)
                            {
                                selectedDoodad = doodadProbability.GameObject;
                                break;
                            }
                        }

                        TileArray[x, y].Doodad = GameObject.Instantiate(
                            selectedDoodad ?? LandDoodads.First().GameObject,
                            new Vector3(x, 1, y), Quaternion.identity, map);
                    }
                }
            }
        }

        public void Start()
        {
            offsetX = Random.Range(0f, 999999f);
            offsetY = Random.Range(0f, 999999f);

            // 1er passage pour génération du sol (sable, terre, eau)
            for (var m = 0; m < nbOfChunksPerRow; m++)
            for (var n = 0; n < nbOfChunksPerRow; n++)
                GenerateChunk(m % nbOfChunksPerRow, n % nbOfChunksPerRow);

            GenerateRessources(tree, 0.3f, treeZoom, RessourceEnum.Wood);
            GenerateRessources(ironOre, 0.1f, ironZoom, RessourceEnum.Iron);
            GenerateRessources(rock, 0.1f, rockZoom, RessourceEnum.Stone);
        }

        // 1+n itérations pour générer les différents éléments.
        // requiredPnValue = La valeur renvoyé par le calcul de Perlin à laquelle vous souhaitez placer vos éléments.
        private void GenerateRessources(GameObject Doodads, float requiredPnValue, float zoom,
            RessourceEnum ressourceEnum = RessourceEnum.None)
        {
            offsetX = Random.Range(0f, 999999f);
            offsetY = Random.Range(0f, 999999f);
            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    var pnValue = CalcPerlin(x, y, zoom);
                    if (pnValue < requiredPnValue)
                    {
                        if (TileArray[x, y].TileEnum == TileEnum.Land)
                        {
                            TileArray[x, y].Resource = GameObject.Instantiate(Doodads, new Vector3(x, 1, y),
                                Quaternion.Euler(0, Random.Range(0.0f, 360.0f), 0), map);
                            TileArray[x, y].Resource.AddComponent<NavMeshObstacle>().carving = true;
                            TileArray[x, y].RessourceEnum = ressourceEnum;

                            if (ressourceEnum == RessourceEnum.Iron || ressourceEnum == RessourceEnum.Stone)
                            {
                                TileArray[x, y].ResourceQuantity = 20;
                            }

                            if (ressourceEnum == RessourceEnum.Wood)
                            {
                                TileArray[x, y].ResourceQuantity = 10;
                            }

                            if (TileArray[x, y].Doodad != null)
                            {
                                GameObject.Destroy(TileArray[x, y].Doodad);
                                TileArray[x, y].Doodad = null;
                            }
                        }
                    }
                }
            }
        }

        private float CalcPerlin(int x, int y, float zoom)
        {
            var xCoord = (float) x / width * zoom + offsetX;
            var yCoord = (float) y / width * zoom + offsetY;

            return Mathf.PerlinNoise(xCoord, yCoord);
        }
    }
}