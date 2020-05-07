using UnityEngine;

namespace Assets.Scripts.Building.GameObjectBehavior
{
    /// <summary>
    ///     Déclaration de la position du Hub.
    /// </summary>
    public class DeclareHubPosition : MonoBehaviour
    {
        /// <summary>
        ///     Appeler lors de la création du GameObject.
        /// </summary>
        private void Start()
        {
            BuildingManager.GetInstance().Hub = gameObject;
        }
    }
}