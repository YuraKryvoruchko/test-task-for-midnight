using UnityEngine;
using Cysharp.Threading.Tasks;

namespace FPS
{
    public class Pistol : Weapon
    {
        #region Public Methods

        public async override void StartShoot()
        {
            if (IsReload == true || CurrentBulletCount <= 0)
                return;

            IsShooting = true;
            Animator.SetBool("IsShoot", IsShooting);

            Shoot();
            await UniTask.Delay(WeaponData.RateInMS);

            StopShoot();
        }
        public override void StopShoot()
        {
            IsShooting = false;
            Animator.SetBool("IsShoot", IsShooting);
        }

        public async override void Reload(BulletValue bulletValue)
        {
            if (bulletValue.GetValue() == 0 || CurrentBulletCount == WeaponData.MaxBulletCount)
                return;

            IsReload = true;
            Animator.SetBool("IsReload", IsReload);

            await UniTask.Delay(WeaponData.ReloadTime);

            DefaultRealod(bulletValue);
            IsReload = false;
            Animator.SetBool("IsReload", IsReload);
        }

        #endregion

        #region Private Methods

        private void Shoot()
        {
            ShootParticle.gameObject.SetActive(true);
            ShootParticle.Play();

            Ray ray = new Ray(CalculateShootPosition(0.03f), PlayerCamera.transform.forward);
            Debug.DrawRay(ray.origin, ray.direction * 200, Color.red, int.MaxValue);

            CurrentBulletCount -= 1;

            if (Physics.Raycast(ray, out RaycastHit raycastHit, int.MaxValue) == false)
                return;
            Debug.Log("Hit");
            if (raycastHit.transform.TryGetComponent(out ITakeDamaging takeDamaging) == true)
                takeDamaging.TakeDamage(WeaponData.Damage);
        }

        #endregion
    }
}
