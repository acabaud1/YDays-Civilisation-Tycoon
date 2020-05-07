public class Iron : ResourcesGame
{
    /// <summary>
    /// Instancie une nouvelle instance de la classe <see cref="Iron"/>
    /// </summary>
    /// <param name="quantity"></param>
    /// <param name="minimum"></param>
    /// <param name="maximum"></param>
    /// <param name="isAccepted"></param>
    public Iron(int quantity, int minimum = 0, int maximum = int.MaxValue, bool isAccepted = true) : base("Fer", quantity, minimum, maximum, isAccepted)
    {
    }
}