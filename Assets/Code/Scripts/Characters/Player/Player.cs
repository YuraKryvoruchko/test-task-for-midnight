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

        [Header("Basic Components")]
        [SerializeField] private Health _health;
        [SerializeField] private HandsAnimator _handsAnimator;
        [SerializeField] private FirstPersonController _firstPersonController;
        [SerializeField] private Inventory _inventory;
        [Header("Crosshair Settings")]
        [SerializeField] private float _defaultFOV = 40f;
        [SerializeField] private float _aimedFOV = 20f;
        [SerializeField] private Crosshair _crosshair;
        [SerializeField] private CinemachineVirtualCamera _virtualCamera;
        [SerializeField] private Camera _handsCamera;
        [Header("Weapon Settings")]
        [SerializeField] private Camera _mainCamera;

        private Weapon _currentWeapon;

        private StarterAssetsInput _input;

        private IShootRayCalculator _shootRayCalculator;

        private const int WEAPON_1 = 1;
        private const int WEAPON_2 = 2;
        private const int WEAPON_3 = 3;

        #endregion

        #region Actions

        public Action<Weapon> OnShoot;
        public Action<Weapon, int> OnReload;
        public Action<Weapon, int> OnWeaponChange;
        public Action OnDeath;

        #endregion

        #region Unity Methods

        private void OnEnable()
        {
            if(_input != null)
                _input.Enable();
        }
        private void OnDisable()
        {
            if (_input != null)
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
            _health.OnDeath += HandlePlayerDead;
            _inventory.Init();
            _crosshair.SetColor(Color.black, 1);
            _shootRayCalculator = new ShootRayCalculatorWithCamera(_mainCamera);
            SetWeapon(WEAPON_1);
            InputInstall();
        }
        public void Deactivate()
        {
            _input.Disable();
            _firstPersonController.enabled = false;
            _health.OnDeath -= HandlePlayerDead;
        }

        #endregion

        #region Private Methods

        private void InputInstall()
        {
            _input = new StarterAssetsInput();
            _input.Player.Shoot.performed += (callback) => StartShoot();
            _input.Player.Shoot.canceled += (callback) => StopShoot();
            _input.Player.Reload.performed += (callback) => Reload();
            _input.Player.Aim.performed += (callback) => Aim();
            _input.Player.Aim.canceled += (callback) => ExitAiming();
            _input.Player.Weapon1.performed += (callback) => SetWeapon(WEAPON_1);
            _input.Player.Weapon2.performed += (callback) => SetWeapon(WEAPON_2);
            _input.Player.Weapon3.performed += (callback) => SetWeapon(WEAPON_3);
            _input.Enable();
        }
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
            _virtualCamera.m_Lens.FieldOfView = _aimedFOV;
            _handsCamera.fieldOfView = _aimedFOV;
        }
        private void ExitAiming()
        {
            _currentWeapon.ExitAiming();
            _crosshair.SetSize(_currentWeapon.CurrentSpread);
            _virtualCamera.m_Lens.FieldOfView = _defaultFOV;
            _handsCamera.fieldOfView = _defaultFOV;
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
        private void HandlePlayerDead()
        {
            Deactivate();
            OnDeath?.Invoke();
        }
        private void SetWeapon(int index)
        {
            _currentWeapon?.gameObject.SetActive(false);
            _currentWeapon = _inventory.GetWeapon(index);
            _currentWeapon.SetShootRayCalculator(_shootRayCalculator);
            _handsAnimator.SetWeaponAnimator(_currentWeapon.Animator);
            _currentWeapon.gameObject.SetActive(true);
            _crosshair.SetSize(_currentWeapon.CurrentSpread);

            OnWeaponChange?.Invoke(_currentWeapon, 
                _inventory.GetBulletCount(_currentWeapon.WeaponData.BulletModel));
        }

        #endregion
    }
}
