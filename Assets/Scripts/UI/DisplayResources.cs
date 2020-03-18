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
    private ResourceManager ResourceManager;
    public TextMeshProUGUI TextMeshProText;
    public RessourceEnum ResourceType;

    void Start()
    {
        try
        {
            // Si ResourceType ne décrit pas un type valide, C# renvoi une exception.
            ResourceManager = ResourceManager.GetInstance();

            var ressourceType = RessourceHelper.GetRessourceGameTypeFromRessourceEnum(ResourceType);

            var tttt = ResourceManager.Get(ressourceType);
            var ttttt = tttt.Obs.AsObservable();

            ttttt.Subscribe(resourceQuantity =>
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
