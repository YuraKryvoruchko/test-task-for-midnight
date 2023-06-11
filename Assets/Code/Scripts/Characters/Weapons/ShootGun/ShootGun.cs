using UnityEngine;
using Cysharp.Threading.Tasks;

namespace FPS
{
    public class ShootGun : Weapon
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

            for (int i = 0; i < 5; i++)
            {
                Ray ray = new Ray(CalculateShootPosition(0.8f), PlayerCamera.transform.forward);
                Debug.DrawRay(ray.origin, ray.direction * 200, Color.red, int.MaxValue);

                if (Physics.Raycast(ray, out RaycastHit raycastHit, int.MaxValue) == false)
                    continue;
                Debug.Log("Hit");
                if (raycastHit.transform.TryGetComponent(out ITakeDamaging takeDamaging) == true)
                    takeDamaging.TakeDamage(WeaponData.Damage);
            }

            CurrentBulletCount -= 1;
        }

        #endregion
    }
}
