using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Resources;
using UniRx;
using Ressource;
using Assets.Scripts.Resources;

public class DisplayResources : MonoBehaviour
{
    private ResourceManager _resourceManager;
    public TextMeshProUGUI TextMeshProText;
    public ResourceEnum ResourceType;
    private Type _resourceType;

    private List<ResourceManagerCore> subscribedManagers = new List<ResourceManagerCore>();

    void Start()
    {
        try
        {
            _resourceManager = ResourceManager.GetInstance();
            // Si ResourceType ne décrit pas un type valide, C# renvoi une exception.
            _resourceType = ResourceHelper.GetResourceGameTypeFromRessourceEnum(ResourceType);
            var resource = _resourceManager.Get(_resourceType);
            var observable = resource.Obs.AsObservable();
            observable.Subscribe(resourceQuantity => {
              updateTextMeshValue();
            });
            foreach (var resourceManagerCore in _resourceManager.ResourceManagerCores)
            {
                resourceManagerCore.Get(_resourceType).Obs.AsObservable().Subscribe(resourceQuantity => {
                    updateTextMeshValue();
                });

                subscribedManagers.Add(resourceManagerCore);
            }
        }
        catch (NullReferenceException e)
        {
            Debug.LogError($"Le type {ResourceType} n'existe pas... Les Resources existantes sont Iron, Stone et Wood. {e}");
        }
    }

    void Update()
    {
        foreach (var resourceManagerCore in _resourceManager.ResourceManagerCores)
        {
            if (!subscribedManagers.Contains(resourceManagerCore))
            {
                resourceManagerCore.Get(_resourceType).Obs.AsObservable().Subscribe(resourceQuantity => {
                    updateTextMeshValue();
                });
            }
        }
    }

    private void updateTextMeshValue()
    {
        TextMeshProText.SetText($"{_resourceManager.GetAllQuantity(_resourceType)}/{ _resourceManager.GetAllStock(_resourceType)}");
    }
}
