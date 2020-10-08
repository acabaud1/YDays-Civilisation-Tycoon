public class Wood : ResourcesGame
{
    /// <summary>
    /// Instancie une nouvelle instance de la classe <see cref="Wood"/>
    /// </summary>
    /// <param name="quantity"></param>
    /// <param name="minimum"></param>
    /// <param name="maximum"></param>
    /// <param name="isAccepted"></param>
    public Wood(int quantity, int minimum = 0, int maximum = int.MaxValue, bool isAccepted = true) : base("Bois", quantity, minimum, maximum, isAccepted)
    {
    }
}