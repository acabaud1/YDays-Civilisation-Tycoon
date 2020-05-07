using System;
using System.Collections.Generic;
using System.Linq;

public sealed class ResourceManager : ResourceManagerCore
{
    private static ResourceManager _instance = null;
    private static bool isInstanciate = false;
    private static readonly object Padlock = new object();
    public List<ResourceManagerCore> ResourceManagerCores { get; set; }

    /// <summary>
    /// Récupère l'instance du ressource manager.
    /// </summary>
    /// <returns>ResourceManager.</returns>
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

    /// <summary>
    /// Initialise une nouvelle instance de la class <see cref="ResourceManager"/>
    /// </summary>
    private ResourceManager()
    {
        ResourceManagerCores = new List<ResourceManagerCore>();

        Resources = new List<ResourcesGame>();
        Resources.Add(new Iron(0, maximum: 100));
        Resources.Add(new Wood(0, maximum: 100));
        Resources.Add(new Stone(0, maximum: 100));
        Init(Resources);
    }

    /// <summary>
    /// Récupère la totalité des quantités.
    /// </summary>
    /// <param name="type">Type de la ressource.</param>
    /// <returns>Quantite en stock.</returns>
    public int GetAllQuantity(Type type)
    {
        // Rafraichissement de l'UI pour contenir les maximums et le contenu actuel des ressources

        // ex : Iron 60 / 100
        int sum = 0;
        foreach (var resourceManagerCore in ResourceManagerCores)
        {
            var test = resourceManagerCore.Get(type).Quantity;
            sum = sum + resourceManagerCore.Get(type).Quantity;
        }

        var quantity = ResourceManagerCores.Sum(rmc => rmc.Get(type).Quantity);
        return Get(type).Quantity + quantity;
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

        remain = AddIfPossible(this, type, remain);

        if (remain != 0)
        {
            foreach (var rmc in ResourceManagerCores)
            {
                remain = AddIfPossible(rmc, type, remain);
                if (remain == 0)
                {
                    break;
                }
            }
        }
    }

    private int AddIfPossible(ResourceManagerCore resourceManagerCore, Type type, int quantity)
    {
        if (resourceManagerCore.Get(type).Quantity + quantity > resourceManagerCore.Get(type).Maximum && quantity > 0)
        {
            int remain = Math.Abs(resourceManagerCore.Get(type).Maximum - resourceManagerCore.Get(type).Quantity -
                                  quantity);
            resourceManagerCore.Add(type, quantity - remain);
            return remain;
        }
        else if (resourceManagerCore.Get(type).Quantity + quantity < 0)
        {
            int remain = resourceManagerCore.Get(type).Quantity + quantity;
            resourceManagerCore.Add(type, -resourceManagerCore.Get(type).Quantity);
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
}