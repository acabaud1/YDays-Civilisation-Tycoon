using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Building;
using Assets.Scripts.Building.GameObjectBehavior;
using Assets.Scripts.Resources;
using JetBrains.Annotations;
using Map;
using Ressource;
using UnityEngine;

public class ResourceIncomeManager : MonoBehaviour
{
    public float Radius;
    public float WaitingTime;
    public float ResourceInterval;
    public int NbByResources;
    public ResourceEnum[] ResourceEnums;
    public GameObject FloatingTextPrefab;

    private ResourceManager resourceManagerScript;
    private int nbOres;
    private BuildingManager buildingManager;

    // Start is called before the first frame update
    void Start()
    {
        try
        {
            resourceManagerScript = ResourceManager.GetInstance();
            buildingManager = BuildingManager.GetInstance();

            InvokeRepeating(nameof(AddResources), WaitingTime, ResourceInterval);
        }
        catch (Exception e)
        {
            Debug.LogError($"Le building Manager ou le ressource Manager ne sont pas présent dans la scène actuelle.");
            Debug.LogError(e);
            throw e;
        }
    }

    private TileModel SelectNearestRessource()
    {
        var tileArray = buildingManager.TileArray;
        List<NearestResource> nearestResources = new List<NearestResource>();
        for (int i = 0; i < tileArray.GetLength(0); i++)
        {
            for (int j = 0; j < tileArray.GetLength(1); j++)
            {
                if (tileArray[i, j].Resource && ResourceEnums.Contains(tileArray[i, j].ResourceEnum))
                {
                    var distance = Vector3.Distance(tileArray[i, j].Resource.transform.position, transform.position);
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
    
    void AddResources()
    {
        var tile = SelectNearestRessource();
        if (tile != null && resourceManagerScript.CanAddAndDistribute(ResourceHelper.GetResourceGameTypeFromRessourceEnum(tile.ResourceEnum), NbByResources))
        {
            int quantity = 0;
            if (tile.ResourceQuantity - NbByResources > 0)
            {
                quantity = NbByResources;
            }
            else
            {
                quantity = tile.ResourceQuantity;
            }
            tile.ResourceQuantity = tile.ResourceQuantity - quantity;
            resourceManagerScript.AddAndDistribute(ResourceHelper.GetResourceGameTypeFromRessourceEnum(tile.ResourceEnum), NbByResources);

            if (tile.ResourceQuantity == 0)
            {
                tile.ResourceEnum = ResourceEnum.None;
                var animationComponent = tile.Resource.GetComponent<BuildingPopAnimation>();
                if (animationComponent)
                {
                    animationComponent.PlayAnimation("BuildingDepop1");
                }
                else
                {
                    Destroy(tile.Resource);
                }
                tile.Resource = null;
            }
        }
    }

    void ShowFloatingText(string text)
    {
        if (FloatingTextPrefab)
        {
            var GoFloatText = Instantiate(
                FloatingTextPrefab,
                transform.position,
                Quaternion.Euler(45f, 0f, 0f),
                transform);

            GoFloatText.transform.position = new Vector3(
                GoFloatText.transform.position.x,
                GoFloatText.transform.position.y + 1,
                GoFloatText.transform.position.z
            );

            GoFloatText.GetComponent<TextMesh>().text = text;

            Destroy(GoFloatText, 1.5f);
        }
    }
}