using System.Linq;
using Assets.Scripts.Resources;
using Boo.Lang;
using JetBrains.Annotations;
using Map;
using Ressource;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Building
{
    /// <summary>
    /// Omportement de la pépinière.
    /// </summary>
    public class PlantNurseryBehavior : MonoBehaviour
    {
        public float WaitingTime;
        public float Interval;
        public float Radius;
        public GameObject Tree;
        private BuildingManager buildingManager;

        // Start is called before the first frame update
        void Start()
        {
            InvokeRepeating(nameof(PlantTree), WaitingTime, Interval);
            buildingManager = BuildingManager.GetInstance();
        }

        private void PlantTree()
        {
            var tile = SelectNearestEmptySpace();
            if (tile != null)
            {
                GameObject tree = Instantiate(Tree, new Vector3(tile.X, 1, tile.Z), Quaternion.identity);

                if (tile.Doodad)
                {
                    Destroy(tile.Doodad);
                    tile.Doodad = null;
                }

                tile.Resource = tree;
                tile.Resource.AddComponent<NavMeshObstacle>().carving = true;
                tile.RessourceEnum = RessourceEnum.Wood;
            }
        }

        [CanBeNull]
        private TileModel SelectNearestEmptySpace()
        {
            var tileArray = buildingManager.TileArray;
            List<NearestResource> nearestResources = new List<NearestResource>();
            for (int i = 0; i < tileArray.GetLength(0); i++)
            {
                for (int j = 0; j < tileArray.GetLength(1); j++)
                {
                    if (!tileArray[i, j].Resource && !tileArray[i, j].Building &&
                        tileArray[i, j].TileEnum == TileEnum.Land)
                    {
                        var distance = Vector3.Distance(new Vector3(i, 1, j), transform.position);
                        if (distance < Radius)
                        {
                            nearestResources.Add(new NearestResource()
                            {
                                Distance = distance,
                                Tile = tileArray[i, j]
                            });
                        }
                    }
                }
            }

            return nearestResources.FirstOrDefault(w => w.Distance == nearestResources.Min(m => m.Distance))?.Tile;
        }
    }
}