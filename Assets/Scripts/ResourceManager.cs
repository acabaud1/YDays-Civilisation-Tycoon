using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    private List<Resources> _resources;

    /// <summary>
    /// Instancie une nouvelle instance de la classe <see cref="ResourceManager"/>
    /// </summary>
    public ResourceManager()
    {
        _resources = new List<Resources>();
        _resources.Add(new Iron("Fer", 0));
        _resources.Add(new Wood("Bois", 10));
        _resources.Add(new Stone("Pierre", 45));
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

        if(resource != null)
        {
            resource.Quantity += quantity;
        }      
    }
}

public abstract class Resources
{
    public string Name { get; set; }
    public int Quantity { get; set; }

    public Resources(string name, int quantity)
    {
        Name = name;
        Quantity = quantity;
    }
}

public class Iron : Resources
{
    /// <summary>
    /// Instancie une nouvelle instance de la classe <see cref="Iron"/>
    /// </summary>
    /// <param name="name"></param>
    /// <param name="quantity"></param>
    public Iron(string name, int quantity) : base(name, quantity)
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
    public Wood(string name, int quantity) : base(name, quantity)
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
    public Stone(string name, int quantity) : base(name, quantity)
    {
    }
}
