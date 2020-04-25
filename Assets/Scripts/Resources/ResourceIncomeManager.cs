using System;
using System.Linq;
using Assets.Scripts.Resources;
using Ressource;
using UnityEngine;

public class ResourceIncomeManager : MonoBehaviour
{
    public float Radius;
    public float WaitingTime;
    public float ResourceInterval;
    public int NbByResources;
    public RessourceEnum RessourceEnum = RessourceEnum.None;
    public GameObject FloatingTextPrefab;

    private ResourceManager resourceManagerScript;
    private int nbOres;

    // Start is called before the first frame update
    void Start()
    {
        try
        {
            resourceManagerScript = ResourceManager.GetInstance();

            InvokeRepeating(nameof(AddResources), WaitingTime, ResourceInterval);
        }
        catch (Exception e)
        {
            Debug.LogError($"Le building Manager ou le ressource Manager ne sont pas présent dans la scène actuelle.");
            Debug.LogError(e);
            throw e;
        }
    }

    private bool GetGameObjetWithRessourceEnum(GameObject go)
    {
        RessourceType ressourceType = go.GetComponent<RessourceType>();
        return ressourceType != null && ressourceType.Ressource == RessourceEnum;
    }

    void GetAllResources()
    {
        nbOres = 0;
        var tileArray = BuildingManager.GetInstance().TileArray;
        for (int i = 0; i < tileArray.GetLength(0); i++)
        {
            for (int j = 0; j < tileArray.GetLength(1); j++)
            {
                if (tileArray[i, j].Resource && tileArray[i, j].RessourceEnum == RessourceEnum && 
                    Vector3.Distance(tileArray[i, j].Resource.transform.position, this.transform.position) < Radius)
                {
                    nbOres++;
                }
            }
        }
    }

    void AddResources()
    {
        GetAllResources();

        int resources = nbOres * NbByResources;
        if (!resourceManagerScript.CanAddAndDistribute(RessourceHelper.GetRessourceGameTypeFromRessourceEnum(RessourceEnum), resources))
        {
            ShowFloatingText("Stocks pleins");
        }
        else
        {
            ShowFloatingText(resources.ToString());
        }
        resourceManagerScript.AddAndDistribute(RessourceHelper.GetRessourceGameTypeFromRessourceEnum(RessourceEnum), resources);
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