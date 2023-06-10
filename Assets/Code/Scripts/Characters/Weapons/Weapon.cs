using UnityEngine;

namespace FPS
{
    public abstract class Weapon : MonoBehaviour
    {
        #region Fields

        [Header("Weapon Data")]
        [SerializeField] private int _damage;
        [SerializeField] private int _maxBulletCount;
        [SerializeField] private BulletModel _bulletModel;
        [SerializeField] private WeaponData _weaponData;
        [Header("VFX")]
        [SerializeField] private ParticleSystem _shootParticle;
        [Header("Animation Settings")]
        [SerializeField] private Animator _animator;
        [SerializeField] private string _shootParameter = "IsShoot";
        [SerializeField] private string _reloadParameter = "IsReload";

        private int _currentBulletCount;

        #endregion

        #region Properties

        public int CurrentBulletCount { get => _currentBulletCount; protected set => _currentBulletCount = value; }
        public WeaponData WeaponData { get => _weaponData; }

        public Animator Animator { get => _animator; }
        public Camera PlayerCamera { get; set; }

        protected ParticleSystem ShootParticle { get => _shootParticle; }

        #endregion

        #region Unity Methods

        private void Awake()
        {
            _weaponData = _weaponData.GetCopy();
        }

        #endregion

        #region Public Methods

        public abstract void Shoot();
        public abstract void Reload(BulletValue bulletValue);

        #endregion
    }
}