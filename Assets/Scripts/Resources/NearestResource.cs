using Map;

namespace Assets.Scripts.Resources
{
    /// <summary>
    /// Class permettant de récupérer la ressource la plus proche.
    /// </summary>
    public class NearestResource
    {
        public float Distance { get; set; }

        public TileModel Tile { get; set; }
    }
}
