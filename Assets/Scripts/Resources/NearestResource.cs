using Map;

namespace Assets.Scripts.Resources
{
    /// <summary>
    /// Class permettant de récupérer la ressource la plus proche.
    /// </summary>
    public class NearestResource
    {
        public float Distance { get; set; } // Distance de la ressource

        public TileModel Tile { get; set; } // Définition ou récupération d'une tile.
    }
}
