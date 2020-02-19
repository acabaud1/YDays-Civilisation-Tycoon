using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Resources;
using Assets.Scripts.Resources;
using Ressource;
using UniRx;

public class DisplayResources : MonoBehaviour
{
    public TextMeshProUGUI TextMeshProText;
    public RessourceEnum ResourceType;

    void Start()
    {
        // Si ResourceType ne décrit pas un type valide, C# renvoi une exception.
        try
        {
            var ressourceType = RessourceHelper.GetRessourceGameTypeFromRessourceEnum(ResourceType);

            ResourceManager.GetInstance().Get(ressourceType).Obs.AsObservable().Subscribe(resourceQuantity =>
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
