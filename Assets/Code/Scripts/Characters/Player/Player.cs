using System;
using UnityEngine;
using Cinemachine;
using Obscure.SDC;

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
        [SerializeField] private Crosshair _crosshair;

        private Weapon _currentWeapon;

        private StarterAssetsInput _input;

        #endregion

        #region Actions

        public Action<Weapon> OnShoot;
        public Action<Weapon, int> OnReload;
        public Action<Weapon, int> OnWeaponChange;
        public Action OnDeath;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            _input = new StarterAssetsInput();
            _input.Player.Shoot.performed += (callback) => Shoot();
            _input.Player.Shoot.canceled += (callback) => StopShoot();
            _input.Player.Shoot.canceled += (callback) => _currentWeapon.Animator.SetBool("IsShoot", false);
            _input.Player.Reload.performed += (callback) => Reload();
            _input.Player.Aim.performed += (callback) => Aim();
            _input.Player.Aim.canceled += (callback) => Unaim();
            _input.Player.Weapon1.performed += (callback) => SetWeapon(WeaponModel.AK);
            _input.Player.Weapon2.performed += (callback) => SetWeapon(WeaponModel.ShootGun);
            _input.Player.Weapon3.performed += (callback) => SetWeapon(WeaponModel.Pistol);
        }
        private void Start()
        {
            SetWeapon(WeaponModel.AK);
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
            _currentWeapon.StartShoot();
            OnShoot?.Invoke(_currentWeapon);
        }
        private void StopShoot()
        {
            _currentWeapon.StopShoot();
        }
        private void Reload()
        {
            _currentWeapon.Reload(_inventory.GetBullets(_currentWeapon.WeaponData.BulletModel));
            OnReload?.Invoke(_currentWeapon, _inventory.GetBulletCount(_currentWeapon.WeaponData.BulletModel));
        }
        private void Aim()
        {
            _crosshair.SetSize(0.01f);
            _virtualCamera.m_Lens.FieldOfView = 20;
        }
        private void Unaim()
        {
            _crosshair.SetSize(0.03f);
            _virtualCamera.m_Lens.FieldOfView = 40;
        }
        private void SetWeapon(WeaponModel weapon)
        {
            _currentWeapon?.gameObject.SetActive(false);
            _currentWeapon = _inventory.GetWeapon(weapon);
            _currentWeapon.PlayerCamera = _camera;
            _handsAnimator.SetWeaponAnimator(_currentWeapon.Animator);
            _currentWeapon.gameObject.SetActive(true);

            OnWeaponChange?.Invoke(_currentWeapon, 
                _inventory.GetBulletCount(_currentWeapon.WeaponData.BulletModel));
        }

        #endregion
    }
}
