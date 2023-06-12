using System;
using UnityEngine;

namespace FPS
{
    public abstract class Weapon : MonoBehaviour
    {
        #region Fields

        [Header("Weapon Data")]
        [SerializeField] private WeaponData _weaponData;
        [Header("VFX")]
        [SerializeField] private ParticleSystem _shootParticle;
        [Header("Animation Settings")]
        [SerializeField] private Animator _animator;
        [SerializeField] private string _isShootParameter = "IsShoot";
        [SerializeField] private string _isReloadParameter = "IsReload";

        private IShootRayCalculator _shootRayCalculator;

        private int _currentBulletCount;
        private float _currentSpread;

        private bool _isShooting = false;
        private bool _isReload = false;
        private bool _isAim = false;
        private bool _isBlock = false;
        private bool _isShake = false;

        #endregion

        #region Actions

        public abstract event Action<int> OnShoot;
        public abstract event Action<int, int> OnReload;

        #endregion

        #region Properties

        public int CurrentBulletCount { get => _currentBulletCount; protected set => _currentBulletCount = value; }
        public float CurrentSpread { get => _currentSpread; }
        public WeaponData WeaponData { get => _weaponData; }
        public Animator Animator { get => _animator; }
        public bool IsBlock { get => _isBlock; }
        public bool IsShake { get => _isShake; }

        protected bool IsShooting { get => _isShooting; set => _isShooting = value; }
        protected bool IsReload { get => _isReload; set => _isReload = value; }
        protected string IsShootParameter { get => _isShootParameter; }
        protected string IsReloadParameter { get => _isReloadParameter; }
        protected ParticleSystem ShootParticle { get => _shootParticle; }

        #endregion

        #region Unity Methods

        private void Awake()
        {
            _weaponData = _weaponData.GetCopy();
            CurrentBulletCount = _weaponData.MaxBulletCount;
        }

        #endregion

        #region Public Methods

        public void Init(IShootRayCalculator shootRayCalculator)
        {
            _shootRayCalculator = shootRayCalculator;
            _currentSpread = WeaponData.SpreadInIdle;
        }
        public void Aim()
        {
            _isAim = true;
            _currentSpread = WeaponData.SpreadInAim;
            if (_isShake == true)
                _currentSpread = (WeaponData.SpreadInAim + WeaponData.SpreadInMove) / 2;
        }
        public void ExitAiming()
        {
            _isAim = false;
            _currentSpread = WeaponData.SpreadInIdle;
            if (_isShake == true)
                _currentSpread = WeaponData.SpreadInMove;
        }
        public void Shake()
        {
            _isShake = true;
            _currentSpread = WeaponData.SpreadInMove;
            if (_isAim == true)
                _currentSpread = (WeaponData.SpreadInAim + WeaponData.SpreadInMove) / 2;
        }
        public void StopShake()
        {
            _isShake = false;
            _currentSpread = WeaponData.SpreadInIdle;
            if (_isAim == true)
                _currentSpread = WeaponData.SpreadInAim;
        }
        public void BlockWeapon()
        {
            _isBlock = true;
        }
        public void UnblockWeapon()
        {
            _isBlock = false;
        }

        public abstract void StartShoot();
        public abstract void StopShoot();
        public abstract void Reload(BulletValue bulletValue);

        #endregion

        #region Private Methods

        protected void DefaultRealod(BulletValue bulletValue)
        {
            int addedValue = WeaponData.MaxBulletCount - CurrentBulletCount;
            if (addedValue > bulletValue.GetValue())
                addedValue = bulletValue.GetValue();

            bulletValue.RemoveValue(addedValue);
            CurrentBulletCount += addedValue;
        }
        protected Ray CalculateAndGetShootRay(float spread)
        {
            return _shootRayCalculator.CalculateAndGetShootRay(spread);
        }

        #endregion
    }
}