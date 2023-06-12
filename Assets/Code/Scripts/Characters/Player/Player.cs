using System;
using UnityEngine;
using Cinemachine;
using Obscure.SDC;
using StarterAssets;

namespace FPS
{
    public class Player : MonoBehaviour
    {
        #region Fields

        [SerializeField] private HandsAnimator _handsAnimator;
        [SerializeField] private FirstPersonController _firstPersonController;
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
            _input.Player.Shoot.performed += (callback) => StartShoot();
            _input.Player.Shoot.canceled += (callback) => StopShoot();
            _input.Player.Reload.performed += (callback) => Reload();
            _input.Player.Aim.performed += (callback) => Aim();
            _input.Player.Aim.canceled += (callback) => ExitAiming();
            _input.Player.Weapon1.performed += (callback) => SetWeapon(WeaponModel.AK);
            _input.Player.Weapon2.performed += (callback) => SetWeapon(WeaponModel.ShootGun);
            _input.Player.Weapon3.performed += (callback) => SetWeapon(WeaponModel.Pistol);
        }
        private void Start()
        {
            _crosshair.SetColor(Color.grey, 1);
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
        private void Update()
        {
            if (_firstPersonController.Grounded == false || _firstPersonController.CurrentSpeed != 0)
                HandleMove();
            else
                HandleStopMove();
        }

        #endregion

        #region Public Methods

        public void Init()
        {
            _input = new StarterAssetsInput();
            _input.Player.Shoot.performed += (callback) => StartShoot();
            _input.Player.Shoot.canceled += (callback) => StopShoot();
            _input.Player.Reload.performed += (callback) => Reload();
            _input.Player.Aim.performed += (callback) => Aim();
            _input.Player.Aim.canceled += (callback) => ExitAiming();
            _input.Player.Weapon1.performed += (callback) => SetWeapon(WeaponModel.AK);
            _input.Player.Weapon2.performed += (callback) => SetWeapon(WeaponModel.ShootGun);
            _input.Player.Weapon3.performed += (callback) => SetWeapon(WeaponModel.Pistol);
            _input.Enable();

            _crosshair.SetColor(Color.grey, 1);
            SetWeapon(WeaponModel.AK);
        }

        #endregion

        #region Private Methods

        private void StartShoot()
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
            _currentWeapon.Aim();
            _crosshair.SetSize(_currentWeapon.CurrentSpread);
            _virtualCamera.m_Lens.FieldOfView = 20;
        }
        private void ExitAiming()
        {
            _currentWeapon.ExitAiming();
            _crosshair.SetSize(_currentWeapon.CurrentSpread);
            _virtualCamera.m_Lens.FieldOfView = 40;
        }
        private void HandleMove()
        {
            _currentWeapon.Shake(); 
            _crosshair.SetSize(_currentWeapon.CurrentSpread);

            if (_firstPersonController.CurrentSpeed == _firstPersonController.SprintSpeed)
                _currentWeapon.BlockWeapon();
            else
                _currentWeapon.UnblockWeapon();
        }
        private void HandleStopMove()
        {
            _currentWeapon.StopShake();
            _crosshair.SetSize(_currentWeapon.CurrentSpread);
        }
        private void SetWeapon(WeaponModel weapon)
        {
            _currentWeapon?.gameObject.SetActive(false);
            _currentWeapon = _inventory.GetWeapon(weapon);
            _currentWeapon.Init(new ShootRayCalculatorWithCamera(_camera));
            _handsAnimator.SetWeaponAnimator(_currentWeapon.Animator);
            _currentWeapon.gameObject.SetActive(true);
            _crosshair.SetSize(_currentWeapon.CurrentSpread);

            OnWeaponChange?.Invoke(_currentWeapon, 
                _inventory.GetBulletCount(_currentWeapon.WeaponData.BulletModel));
        }

        #endregion
    }
}
