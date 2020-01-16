using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UniRx;

public class DisplayResources : MonoBehaviour
{
    public ResourceManager ResourceManager;
    public TextMeshProUGUI TextMeshProText;
    public string ResourceType;
    private Type _resourceType;

    void Start()
    {
        try
        {
            // Si ResourceType ne décrit pas un type valide, C# renvoi une exception.
            _resourceType = Type.GetType(ResourceType.ToString());
            ResourceManager.Get(_resourceType).Obs.AsObservable().Subscribe(resourceQuantity =>
              {
                TextMeshProText.SetText(resourceQuantity.ToString());
              });
        }
        catch (NullReferenceException e)
        {
            Debug.LogError($"Le type {ResourceType} n'existe pas... Les Resources existantes sont Iron, Stone et Wood. {e}");
        }
    }

}
