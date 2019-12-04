using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ResourceManager : ResourceManagerCore
{

    // Start is called before the first frame update
    void Start()
    {
        resources = new List<Resources>();
        resources.Add(new Iron("Fer", 0));
        resources.Add(new Wood("Bois", 10));
        resources.Add(new Stone("Pierre", 45));
        Init(resources);
    }

    private List<Resources> resources;
}

public abstract class Resources
{
    public string Name { get; set; }
    public int Quantity { get; set; }
    public int Minimum { get; set; }
    public int Maximum { get; set; }
    public bool IsAccepted { get; set; }

    public Resources(string name, int quantity, int minimum = 0, int maximum = 30, bool isAccepted = true)
    {
        Name = name;
        Quantity = quantity;
        Minimum = minimum;
        Maximum = maximum;
        IsAccepted = isAccepted;
    }
}

public class Iron : Resources
{
    /// <summary>
    /// Instancie une nouvelle instance de la classe <see cref="Iron"/>
    /// </summary>
    /// <param name="name"></param>
    /// <param name="quantity"></param>
    public Iron(string name, int quantity, int minimum = 0, int maximum = int.MaxValue, bool isAccepted = true) : base(name, quantity, minimum, maximum, isAccepted)
    {
    }
}
public class Wood : Resources
{
    /// <summary>
    /// Instancie une nouvelle instance de la classe <see cref="Wood"/>
    /// </summary>
    /// <param name="name"></param>
    /// <param name="quantity"></param>
    public Wood(string name, int quantity, int minimum = 0, int maximum = int.MaxValue, bool isAccepted = true) : base(name, quantity, minimum, maximum, isAccepted)
    {
    }
}
public class Stone : Resources
{
    /// <summary>
    /// Instancie une nouvelle instance de la classe <see cref="Stone"/>
    /// </summary>
    /// <param name="name"></param>
    /// <param name="quantity"></param>
    public Stone(string name, int quantity, int minimum = 0, int maximum = int.MaxValue, bool isAccepted = true) : base(name, quantity, minimum, maximum, isAccepted)
    {
    }
}

public class ResourceManagerCore : MonoBehaviour
{
    private List<Resources> _resources;

    /// <summary>
    /// Instancie une nouvelle instance de la classe <see cref="ResourceManagerCore"/>
    /// </summary>
    public void Init(List<Resources> resources)
    {
        _resources = resources;
    }

    /// <summary>
    /// Récupère un objet de type Resources
    /// </summary>
    /// <param name="type">Type de la ressource voulue</param>
    /// <returns></returns>
    public Resources Get(Type type)
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

        }
    }

    protected virtual bool canAdd(Resources resources, int quantity)
    {
        if (resources.Quantity + quantity > resources.Minimum && resources.Quantity + quantity < resources.Maximum && resources.IsAccepted)
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