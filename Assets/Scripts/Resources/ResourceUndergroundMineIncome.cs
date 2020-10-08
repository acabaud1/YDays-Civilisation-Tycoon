using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// class de création de la mine de profondeur
/// </summary>
public class ResourceUndergroundMineIncome : MonoBehaviour
{
    public List<Type> Resources = new List<Type>(); // Liste des ressources étant récupéré 
    public float TimeBetween = 2;
    public int quantity = 1;
    public float WaitingTime = 1;

    // Start is called before the first frame update
    void Start()
    {
        Resources.Add(typeof(Iron));
        Resources.Add(typeof(Stone));
        InvokeRepeating("GainResources", WaitingTime, TimeBetween);
    }

    /// <summary>
    /// Récupération d'une ressource aléatoire selon la liste et la quantité fourni
    /// </summary>
    private void GainResources()
    {
        ResourceManager.GetInstance().Add(Resources[Random.Range(0, Resources.Count)], quantity);
    }
}