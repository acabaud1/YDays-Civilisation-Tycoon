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
    public RessourceEnum ResourceType;
    private Type _resourceType;

    void Start()
    {
        try
        {
            _resourceManager = ResourceManager.GetInstance();
            // Si ResourceType ne décrit pas un type valide, C# renvoi une exception.
            _resourceType = RessourceHelper.GetRessourceGameTypeFromRessourceEnum(ResourceType);
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
            }
        }
        catch (NullReferenceException e)
        {
            Debug.LogError($"Le type {ResourceType} n'existe pas... Les Resources existantes sont Iron, Stone et Wood. {e}");
        }
    }

    private void updateTextMeshValue()
    {
        TextMeshProText.SetText($"{_resourceManager.GetAllQuantity(_resourceType)}/{_resourceManager.GetAllStock(_resourceType)}");
    }
}
