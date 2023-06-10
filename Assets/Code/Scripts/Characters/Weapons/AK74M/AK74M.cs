using UnityEngine;
using Cysharp.Threading.Tasks;

namespace FPS
{
    public class AK74M : Weapon
    {
        #region Fields

        [SerializeField] private float _offset = 0.04f;

        private bool _isShooting = false;
        private bool _isReload = false;

        #endregion

        #region Unity Methods

        public void OnGUI()
        {
            float k = (float)Screen.width / (float)Screen.height;
            Debug.Log(k);
            Rect rect = new Rect(Screen.width / 2 - Screen.width * _offset, 
                Screen.height / 2 - Screen.height * _offset * k,
                (Screen.width * _offset) * 2, (Screen.height * _offset) * 2 * k);
            GUI.Box(rect, "");
        }

        #endregion

        #region Public Methods

        public override void Shoot()
        {
            if (CurrentBulletCount <= 0)
                return;

            Animator.SetBool("IsShoot", true);
            ShootParticle.gameObject.SetActive(true);
            ShootParticle.Play();

            Ray ray = new Ray(CalculateShootPosition(0.03f), PlayerCamera.transform.forward);
            Debug.DrawRay(ray.origin, ray.direction * 200, Color.red, int.MaxValue);
            
            CurrentBulletCount -= 1;

            if (Physics.Raycast(ray, out RaycastHit raycastHit, int.MaxValue ) == false)
                return;
            Debug.Log("Hit");
            if (raycastHit.transform.TryGetComponent(out ITakeDamaging takeDamaging) == true)
                takeDamaging.TakeDamage(WeaponData.Damage);

            Animator.SetBool("IsShoot", false);
        }
        public override void StartShoot()
        {
            _isShooting = true;
        }
        public override void StopShoot()
        {
            _isShooting = false;
        }
        public override void Reload(BulletValue bulletValue)
        {
            DefaultRealod(bulletValue);
        }

        #endregion
    }
}
