using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ResourceManagerChild : ResourceManagerCore
{
    public int Max;
    private int TotalResource => resources == null ? 0 : resources.Sum(resources => resources.Quantity);
    private List<ResourcesGame> resources;

    // Start is called before the first frame update
    void Start()
    {
        resources = new List<ResourcesGame>
        {
            new Iron(0),
            new Wood(0),
            new Stone(0)
        };
        Init(resources);
    }

    protected override bool canAdd(ResourcesGame resources, int quantity)
    {
        return base.canAdd(resources, quantity) && TotalResource + quantity < Max;
    }
}
