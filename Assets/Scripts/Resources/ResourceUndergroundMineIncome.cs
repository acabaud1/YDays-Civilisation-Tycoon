using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ResourceUndergroundMineIncome : MonoBehaviour
{
    public List<Type> Resources = new List<Type>();
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

    private void GainResources()
    {
        ResourceManager.GetInstance().Add(Resources[Random.Range(0, Resources.Count)], quantity);
    }
}