using UnityEngine;

namespace Assets.Scripts.Building
{
    /// <summary>
    /// Modèle de probabilité des doodads.
    /// </summary>
    public class DoodadProbability
    {
        /// <summary>
        /// Obtient ou définit le doodad.
        /// </summary>
        public GameObject GameObject { get; set; }

        /// <summary>
        /// Obtient ou définit la probabilité.
        /// </summary>
        public int Probability { get; set; }
    }
}
