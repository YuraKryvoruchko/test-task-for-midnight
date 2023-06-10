using UnityEngine;

namespace FPS
{
    public abstract class Weapon : MonoBehaviour
    {
        #region Fields

        [SerializeField] private Animator _animator;

        #endregion

        #region Properties

        public Animator Animator { get => _animator; }
        public Camera PlayerCamera { get; set; }

        #endregion

        #region Public Methods

        public abstract void Shoot();
        public abstract void Reload();

        #endregion
    }
}
