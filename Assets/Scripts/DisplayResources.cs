using UnityEngine;
using UnityEngine.UI;
using System;
using UniRx;
using Assets.Scripts.Resources;

public class DisplayResources : MonoBehaviour
{
    public ResourceManager ResourceManager;
    public Text TextComponent;
    public RessourceEnum RessourceEnum = RessourceEnum.None;

    void Start()
    {
        Type ResourceType = RessourceHelper.GetRessourceGameTypeFromRessourceEnum(RessourceEnum);
        ResourceManager.Get(ResourceType).Obs.AsObservable().Subscribe(resourceQuantity =>
        {
            TextComponent.text = resourceQuantity.ToString();
        });
    }

}
