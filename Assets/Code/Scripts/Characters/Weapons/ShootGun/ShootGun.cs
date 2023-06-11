using System;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace FPS
{
    public class ShootGun : Weapon
    {
        #region Actions

        public override event Action<int> OnShoot;
        public override event Action<int, int> OnReload;

        #endregion

        #region Public Methods

        public async override void StartShoot()
        {
            if (IsReload == true || CurrentBulletCount <= 0 || IsBlock == true)
                return;

            IsShooting = true;
            Animator.SetBool(IsShootParameter, IsShooting);

            Shoot();
            BlockWeapon();
            await UniTask.Delay(WeaponData.RateInMS);
            UnblockWeapon();

            StopShoot();
        }
        public override void StopShoot()
        {
            IsShooting = false;
            Animator.SetBool(IsShootParameter, IsShooting);
        }

        public async override void Reload(BulletValue bulletValue)
        {
            if (bulletValue.GetValue() == 0 || CurrentBulletCount == WeaponData.MaxBulletCount || IsBlock == true)
                return;

            IsReload = true;
            Animator.SetBool(IsReloadParameter, IsReload);

            await UniTask.Delay(WeaponData.ReloadTime);

            DefaultRealod(bulletValue);
            IsReload = false;
            Animator.SetBool(IsReloadParameter, IsReload);
            OnReload?.Invoke(CurrentBulletCount, bulletValue.GetValue());
        }

        #endregion

        #region Private Methods

        private void Shoot()
        {
            ShootParticle.gameObject.SetActive(true);
            ShootParticle.Play();

            for (int i = 0; i < 5; i++)
            {
                Ray ray = new Ray(CalculateShootPosition(CurrentSpread), PlayerCamera.transform.forward);
                Debug.DrawRay(ray.origin, ray.direction * 200, Color.red, int.MaxValue);

                if (Physics.Raycast(ray, out RaycastHit raycastHit, int.MaxValue) == false)
                    continue;
                Debug.Log("Hit");
                if (raycastHit.transform.TryGetComponent(out ITakeDamaging takeDamaging) == true)
                    takeDamaging.TakeDamage(WeaponData.Damage);
            }

            CurrentBulletCount -= 1;
            OnShoot?.Invoke(CurrentBulletCount);
        }

        #endregion
    }
}
