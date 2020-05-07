using UnityEngine;

namespace Assets.Scripts.Building
{
    public class BuildingPopAnimation : MonoBehaviour
    {
        private Animation _animation;
        public string Animation = "BuildingPop1";
        private string _lastPlayedAnimation;
        private bool _isAnimationPlaying = false;

        /// <summary>
        /// Méthode appeler à l'instanciation du GameObject.
        /// </summary>
        void Start()
        {
            _animation = GetComponent<Animation>();
            PlayAnimation(Animation);
            if (_isAnimationPlaying && !_animation.isPlaying)
            {
                OnAnimationEnd();
            }
        }

        /// <summary>
        /// Joue une animation en fonction de son nom.
        /// </summary>
        /// <param name="animationName">Nom de l'animation.</param>
        public void PlayAnimation(string animationName)
        {
            _animation.Play(animationName);
            _lastPlayedAnimation = animationName;
            _isAnimationPlaying = true;
        }

        /// <summary>
        /// Callback de l'animation.
        /// </summary>
        private void OnAnimationEnd()
        {
            if (_lastPlayedAnimation == "BuildingDepop1" || _lastPlayedAnimation == "BuildingDepop2")
            {
                Destroy(gameObject);
                _isAnimationPlaying = false;
            }
        }
    }
}
