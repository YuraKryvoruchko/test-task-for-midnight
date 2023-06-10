using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    public class PlayerAnimation : MonoBehaviour
    {
        #region Fields

        [Header("Animator Settings")]
        [SerializeField] private Animator _animator;
        [SerializeField] private string _isMoveParameter = "isMove";
        [SerializeField] private string _isAimParameter = "isAim";

        #endregion

        #region Unity Methods

        private void Awake()
        {
            
        }

        #endregion
    }
}
