using System;
using UnityEngine;

using Random = UnityEngine.Random;

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
        public Camera PlayerCamera { get; set; }
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

        public abstract void StartShoot();
        public abstract void StopShoot();
        public abstract void Reload(BulletValue bulletValue);
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
        protected Vector3 CalculateShootPosition(float spread)
        {
            float coefficient = (float)Screen.width / (float)Screen.height;
            Rect rect = new Rect(Screen.width / 2 - Screen.width * spread,
                Screen.height / 2 - Screen.height * spread * coefficient,
                (Screen.width * spread) * 2, (Screen.height * spread) * 2 * coefficient);

            Vector3 starPosition = new Vector3(Random.Range(rect.x, rect.x + rect.width),
                Random.Range(rect.y, rect.y + rect.height), 0);

            Ray ray = Camera.main.ScreenPointToRay(starPosition);

            return ray.origin;
        }

        #endregion
    }
}