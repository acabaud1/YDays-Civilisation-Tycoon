using Assets.Scripts.Resources;
using System;
using System.Linq;
using UnityEngine;

public class ResourceIncomeManager : MonoBehaviour
{
    public float Radius;
    public float WaitingTime;
    public float ResourceInterval;
    public int NbByResources;
    public RessourceEnum RessourceEnum = RessourceEnum.None;
    public GameObject FloatingTextPrefab;

    private GameObject BuildingManager;
    private BuildingManager buildingManagerScript;
    private GameObject ResourceManager;
    private ResourceManager resourceManagerScript;
    private int nbOres;
    
    // Start is called before the first frame update
    void Start()
    {
        try
        {
            BuildingManager = GameObject.Find("Building Manager");
            buildingManagerScript = BuildingManager.GetComponent<BuildingManager>();

            ResourceManager = GameObject.Find("Resource Manager");
            resourceManagerScript = ResourceManager.GetComponent<ResourceManager>();

            GetAllResources();
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
        foreach (GameObject element in buildingManagerScript.Doodads.Where(go => GetGameObjetWithRessourceEnum(go)))
        {
            if (Vector3.Distance(element.transform.position, this.transform.position) < Radius)
            {
                nbOres++;
            }
        }
    }

    void AddResources()
    {
        int resources = nbOres * NbByResources;
        ShowFloatingText();
        resourceManagerScript.Add(RessourceHelper.GetRessourceGameTypeFromRessourceEnum(RessourceEnum), resources);
    }

    void ShowFloatingText()
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

            GoFloatText.GetComponent<TextMesh>().text = (NbByResources * nbOres).ToString();

            Destroy(GoFloatText, 1.5f);
        }
    }
}
