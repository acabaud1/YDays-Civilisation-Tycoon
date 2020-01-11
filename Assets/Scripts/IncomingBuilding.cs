using UnityEngine;
using System;
using System.Collections;

public class IncomingBuilding : MonoBehaviour
{

    public string ResourceName;
    public int IncomeQuantity;
    public float IntervalTime;
    public GameObject FloatingTextPrefab;

    private ResourceManager _myResourceManager;
    private GameObject _resourceManagerGO;
    private Type ResourceType;


    void Start()
    {
        try
        {
            ResourceType = Type.GetType(ResourceName);
            _resourceManagerGO = GameObject.Find("ResourceManager");
            _myResourceManager = _resourceManagerGO.GetComponent<ResourceManager>();

            InvokeRepeating("TriggerIncome", 0.0f, IntervalTime);
        } 
        catch (NullReferenceException e)
        {
            Debug.LogError($"Le type {ResourceName} n'existe pas.");
            Debug.LogError(e);
        }
    }

    void TriggerIncome()
    {
        if (_myResourceManager.CanAdd(ResourceType, IncomeQuantity))
        {
            ShowFloatingText();
            _myResourceManager.Add(ResourceType, IncomeQuantity);
        }    
    }

    void ShowFloatingText()
    {
        if (FloatingTextPrefab)
        {
            var GoFloatText = Instantiate(FloatingTextPrefab, transform.position, Quaternion.identity, transform);
            GoFloatText.GetComponent<TextMesh>().text = IncomeQuantity.ToString();
        }
    }

}
