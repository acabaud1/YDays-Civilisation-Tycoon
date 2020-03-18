public class Stone : ResourcesGame
{
    /// <summary>
    /// Instancie une nouvelle instance de la classe <see cref="Stone"/>
    /// </summary>
    /// <param name="quantity"></param>
    public Stone(int quantity, int minimum = 0, int maximum = int.MaxValue, bool isAccepted = true) : base("Pierre", quantity, minimum, maximum, isAccepted)
    {
    }
}