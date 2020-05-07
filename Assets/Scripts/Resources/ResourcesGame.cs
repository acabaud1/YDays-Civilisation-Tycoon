using UniRx;

/// <summary>
/// class de définition de ressource
/// </summary>
public abstract class ResourcesGame
{
    public string Name { get; set; } // définit le nom de la ressource
    public int Quantity { get; set; } // définit la quantité de la ressource
    public int Minimum { get; set; } // définit le minimum de la ressource
    public int Maximum { get; set; } // définit le maximum de la ressource
    public bool IsAccepted { get; set; } // définit si la ressource est accepté 

    public ReactiveProperty<int> Obs { get; } // crée un observable pour mettre à jour l'affichage de la quantité sur l'affichage 

    /// <summary>
    /// Création de la ressource avec ses attributs
    /// </summary>
    /// <param name="isAccepted">Est accepté pour être ajouté</param>
    /// <param name="maximum">Maximum possiblement récupérable de base</param>
    /// <param name="minimum">Minimum possiblement récupérable de base</param>
    /// <param name="name">Nom de la ressource</param>
    /// <param name="quantity">Quantité de la ressource</param>
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