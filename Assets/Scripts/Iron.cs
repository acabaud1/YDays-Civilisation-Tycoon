public class Iron : ResourcesGame
{
    /// <summary>
    /// Instancie une nouvelle instance de la classe <see cref="Iron"/>
    /// </summary>
    /// <param name="quantity"></param>
    public Iron(int quantity, int minimum = 0, int maximum = int.MaxValue, bool isAccepted = true) : base("Fer", quantity, minimum, maximum, isAccepted)
    {
    }
}