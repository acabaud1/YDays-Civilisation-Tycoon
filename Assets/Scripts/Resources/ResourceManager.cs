using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UniRx;

public class ResourceManager : ResourceManagerCore
{
    // Start is called before the first frame update
    void Start()
    {
        Resources = new List<ResourcesGame>();
        Resources.Add(new Iron(0, maximum : 100));
        Resources.Add(new Wood(0, maximum : 100));
        Resources.Add(new Stone(0, maximum : 100));
        Init(Resources);
    }

    private List<ResourcesGame> Resources;
}

public abstract class ResourcesGame
{
    public string Name { get; set; }
    public int Quantity { get; set; }
    public int Minimum { get; set; }
    public int Maximum { get; set; }
    public bool IsAccepted { get; set; }

    public ReactiveProperty<int> Obs { get; }

    public ResourcesGame(string name, int quantity, int minimum = 0, int maximum = 30, bool isAccepted = true)
    {
        Name = name;
        Quantity = quantity;
        Minimum = minimum;
        Maximum = maximum;
        IsAccepted = isAccepted;
        Obs = new ReactiveProperty<int>(quantity);
    }
}

public class ResourceManagerCore : MonoBehaviour
{
    private List<ResourcesGame> _resources;

    /// <summary>
    /// Instancie une nouvelle instance de la classe <see cref="ResourceManagerCore"/>
    /// </summary>
    public void Init(List<ResourcesGame> Resources)
    {
        _resources = Resources;
    }

    /// <summary>
    /// Récupère un objet de type ResourcesGame
    /// </summary>
    /// <param name="type">Type de la ressource voulue</param>
    /// <returns></returns>
    public ResourcesGame Get(Type type) // Wood
    {
        return _resources.FirstOrDefault(w => w.GetType() == type);
    }

    /// <summary>
    /// Ajoute une quantité à une ressource
    /// </summary>
    /// <param name="type">Type de la ressource</param>
    /// <param name="quantity">Quantité à ajouter</param>
    public void Add(Type type, int quantity)
    {
        var resource = Get(type);

        if (resource != null && canAdd(resource, quantity))
        {
            resource.Quantity += quantity;
            resource.Obs.Value = resource.Quantity;
        }
    }

    protected virtual bool canAdd(ResourcesGame ResourcesGame, int quantity)
    {
        if (ResourcesGame.Quantity + quantity > ResourcesGame.Minimum && ResourcesGame.Quantity + quantity < ResourcesGame.Maximum && ResourcesGame.IsAccepted)
        {
            return true;
        }
        return false;
    }

    public bool CanAdd(Type type, int quantity)
    {
        var resource = Get(type);

        if (resource != null && canAdd(resource, quantity))
        {
            return true;
        }
        return false;
    }
}