using UnityEngine;
using StarterAssets;

namespace FPS
{
    public class HandsAnimator : MonoBehaviour
    {
        #region Fields

        [Header("First Person Controller")]
        [SerializeField] private FirstPersonController _firstPersonController;
        [Header("Animator Settings")]
        [SerializeField] private string _speedParameter = "Speed";

        private Animator _currentAnimator;

        #endregion

        #region Unity Methods

        private void Update()
        {
            _currentAnimator.SetFloat(_speedParameter, _firstPersonController.CurrentSpeed);
        }

        #endregion

        #region Public Methods

        public void SetWeaponAnimator(Animator animator)
        {
            _currentAnimator = animator;
        }

        #endregion
    }
}