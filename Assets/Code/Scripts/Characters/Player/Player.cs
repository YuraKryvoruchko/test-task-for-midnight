using System;
using UnityEngine;
using Cinemachine;

namespace FPS
{
    public class Player : MonoBehaviour
    {
        #region Fields

        [SerializeField] private HandsAnimator _handsAnimator;
        [SerializeField] private Inventory _inventory;
        [Space]
        [SerializeField] private CinemachineVirtualCamera _virtualCamera;
        [SerializeField] private Camera _camera;

        private Weapon _currentWeapon;

        private StarterAssetsInput _input;

        #endregion

        #region Actions

        public Action OnWeaponChange;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            _input = new StarterAssetsInput();
            _input.Player.Shoot.performed += (callback) => Shoot();
            _input.Player.Shoot.canceled += (callback) => _currentWeapon.Animator.SetBool("IsShoot", false);
            _input.Player.Reload.performed += (callback) => Reload();
            _input.Player.Aim.performed += (callback) => Aim();
            _input.Player.Aim.canceled += (callback) => Unaim();
            _input.Player.Weapon1.performed += (callback) => SetWeapon(Weapons.AK);
            _input.Player.Weapon2.performed += (callback) => SetWeapon(Weapons.ShootGun);
            _input.Player.Weapon3.performed += (callback) => SetWeapon(Weapons.Pistol);
        }
        private void Start()
        {
            SetWeapon(Weapons.AK);
        }
        private void OnEnable()
        {
            _input.Enable();
        }
        private void OnDisable()
        {
            _input.Disable();
        }

        #endregion

        #region Private Methods

        private void Shoot()
        {
            _currentWeapon.Shoot();
        }
        private void Reload()
        {
            _currentWeapon.Reload();
        }
        private void Aim()
        {
            Debug.Log("Aim");
            _virtualCamera.m_Lens.FieldOfView = 20;
        }
        private void Unaim()
        {
            Debug.Log("Unaim");
            _virtualCamera.m_Lens.FieldOfView = 40;
        }
        private void SetWeapon(Weapons weapon)
        {
            _currentWeapon?.gameObject.SetActive(false);
            _currentWeapon = _inventory.GetWeapon(weapon);
            _currentWeapon.PlayerCamera = _camera;
            _handsAnimator.SetWeaponAnimator(_currentWeapon.Animator);
            _currentWeapon.gameObject.SetActive(true);
        }

        #endregion
    }
}
