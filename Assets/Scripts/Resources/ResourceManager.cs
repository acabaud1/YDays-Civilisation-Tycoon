using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UniRx;
using Ressource;
using Assets.Scripts.Resources;

public sealed class ResourceManager : ResourceManagerCore
{
    private static ResourceManager _instance = null;
    private static bool isInstanciate = false;
    private static readonly object Padlock = new object();
    public List<ResourceManagerCore> ResourceManagerCores { get; set; }


    public static ResourceManager GetInstance()
    {
        lock (Padlock)
        {
            if (!isInstanciate)
            {
                _instance = new ResourceManager();
                isInstanciate = true;
            }
            return _instance;
        }
    }

    private ResourceManager()
    {
        ResourceManagerCores = new List<ResourceManagerCore>();

        Resources = new List<ResourcesGame>();
        Resources.Add(new Iron(0, maximum: 100));
        Resources.Add(new Wood(0, maximum: 100));
        Resources.Add(new Stone(0, maximum: 100));
        Init(Resources);
    }

    public int GetAllQuantity(Type type)
    {
        // Rafraichissement de l'UI pour contenir les maximums et le contenu actuel des ressources

        // ex : Iron 60 / 100


        return Get(type).Quantity + ResourceManagerCores.Sum(rmc => rmc.Get(type).Quantity);
    }

    public int GetAllStock(Type type)
    {
        // Rafraichissement de l'UI pour contenir les maximums et le contenu actuel des ressources

        // ex : Iron 60 / 100

        return Get(type).Maximum + ResourceManagerCores.Sum(rmc => rmc.Get(type).Maximum);
    }

    public void AddAndDistribute(Type type, int quantity)
    {
        // en fonction du type et de la quantité => distribution de la ressources dans les entrepos
        // Calcul de la place :

        int remain = quantity;

        remain = addPossible(this, type, remain);

        if (remain != 0)
        {
            foreach (var rmc in ResourceManagerCores)
            {
                remain = addPossible(rmc, type, remain);
                if (remain == 0)
                {
                    break;
                }
            }
        }
    }

    private int addPossible(ResourceManagerCore resourceManagerCore, Type type, int quantity)
    {
        if (resourceManagerCore.Get(type).Quantity + quantity > resourceManagerCore.Get(type).Maximum)
        {
            int remain = Math.Abs(resourceManagerCore.Get(type).Maximum - resourceManagerCore.Get(type).Quantity -
                                  quantity);
            resourceManagerCore.Add(type, quantity - remain);
            return remain;
        }
        else
        {
            resourceManagerCore.Add(type, quantity);
            return 0;
        }
    }

    public bool CanAddAndDistribute(Type type, int quantity)
    {
        int quantityStock = Get(type).Quantity + ResourceManagerCores.Sum(rmc => rmc.Get(type).Quantity);
        int quantityStockMaximum = Get(type).Maximum + ResourceManagerCores.Sum(rmc => rmc.Get(type).Maximum);

        // Vérification s'il y a de la place et distribution dans les stocks
        if (quantityStock + quantity > quantityStockMaximum || quantityStock + quantity < 0)
        {
            return false;
        }

        return true;
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
            resource.Quantity = resource.Quantity + quantity;
            resource.Obs.Value = resource.Quantity;
        }
    }

    protected virtual bool canAdd(ResourcesGame ResourcesGame, int quantity)
    {
        if (ResourcesGame.Quantity + quantity >= ResourcesGame.Minimum &&
            ResourcesGame.Quantity + quantity <= ResourcesGame.Maximum && ResourcesGame.IsAccepted)
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