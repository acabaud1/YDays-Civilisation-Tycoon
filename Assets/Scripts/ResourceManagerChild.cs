using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ResourceManagerChild : ResourceManagerCore
{
    public int Max;
    private int actual => resources == null ? 0 : resources.Sum(resources => resources.Quantity);
    private List<Resources> resources;

    // Start is called before the first frame update
    void Start()
    {
        resources = new List<Resources>();
        resources.Add(new Iron("Fer", 0));
        resources.Add(new Wood("Bois", 0));
        resources.Add(new Stone("Pierre", 0));
        Init(resources);
    }

    protected override bool canAdd(Resources resources, int quantity)
    {
        return base.canAdd(resources, quantity) && actual + quantity < Max;
    }
}
