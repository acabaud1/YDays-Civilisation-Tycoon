using UnityEngine;

namespace Assets.Scripts.Building
{
    /// <summary>
    /// Déclaration de la position du Hub.
    /// </summary>
    public class DeclareHubPosition : MonoBehaviour
    {
        /// <summary>
        /// Appeler lors de la création du GameObject.
        /// </summary>
        void Start()
        {
            BuildingManager.GetInstance().Hub = gameObject;
        }
    }
}
