using UnityEngine;

namespace Assets.Scripts.Building
{
    /// <summary>
    /// Définit le comportement d'un batiment.
    /// </summary>
    public class BuildingBehavior : MonoBehaviour
    {
        public Material DefaultMaterial;
        public Material ErrorMaterial;

        private bool _isInError = false;
        private MeshRenderer _meshRenderer;

        /// <summary>
        /// Obtient si le batiment est en mode erreur.
        /// </summary>
        /// <returns>Si le batiment est en mode erreur.</returns>
        public bool IsInError()
        {
            return _isInError;
        }

        /// <summary>
        /// Change l'etat du batiement pour le mettre en erreur.
        /// </summary>
        public void ToggleMaterial()
        {
            if (_isInError)
            {
                _meshRenderer.material = ErrorMaterial;
            }
            else
            {
                _meshRenderer.material = DefaultMaterial;
            }

            _isInError = !_isInError;
        }

        /// <summary>
        /// Fonction executé a l'instanciation du GameObject.
        /// </summary>
        void Start()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
        }
    }
}
