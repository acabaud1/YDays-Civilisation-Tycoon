using Assets.Scripts.Building;
using Ressource;
using UnityEngine;

namespace Map
{
    /// <summary>
    /// Définition d'une tile.
    /// </summary>
    public class TileModel
    {
        /// <summary>
        /// Type de tile.
        /// </summary>
        public TileEnum TileEnum { get; set; }
        
        /// <summary>
        /// Tile.
        /// </summary>
        public GameObject Tile { get; set; }
        
        /// <summary>
        /// Type de ressource.
        /// </summary>
        public RessourceEnum RessourceEnum { get; set; }
        
        /// <summary>
        /// Resource présente au dessus de la tile
        /// </summary>
        public GameObject Resource { get; set; }
        
        /// <summary>
        /// Bâtiment présente au dessus de la tile
        /// </summary>
        public GameObject Building { get; set; }
        
        /// <summary>
        /// Type du batiment posé.
        /// </summary>
        public BuildingEnum BuildingEnum { get; set; }
        
        /// <summary>
        /// Quantite de ressource.
        /// </summary>
        public int ResourceQuantity { get; set; }
        
        /// <summary>
        /// X.
        /// </summary>
        public int X { get; set; }
        
        /// <summary>
        /// Z.
        /// </summary>
        public int Z { get; set; }
        
        /// <summary>
        /// décoration présente au dessus de la tile
        /// </summary>
        public GameObject Doodad { get; set; }
    }
}