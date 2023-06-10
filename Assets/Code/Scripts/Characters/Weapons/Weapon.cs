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
        public abstract void StartShoot();
        public abstract void StopShoot();
        public abstract void Reload(BulletValue bulletValue);

        #endregion

        #region Private Methods

        protected void DefaultRealod(BulletValue bulletValue)
        {
            if (bulletValue.GetValue() == 0)
                return;

            Animator.SetTrigger("Reload");
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