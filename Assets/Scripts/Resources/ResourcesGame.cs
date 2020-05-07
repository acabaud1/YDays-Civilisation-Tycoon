using UniRx;

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