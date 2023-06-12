using System;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace FPS
{
    public class Pistol : Weapon
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

            Ray ray = CalculateAndGetShootRay(CurrentSpread);
            Debug.DrawRay(ray.origin, ray.direction * 200, Color.red, int.MaxValue);

            CurrentBulletCount -= 1;
            OnShoot?.Invoke(CurrentBulletCount);

            if (Physics.Raycast(ray, out RaycastHit raycastHit, int.MaxValue, WeaponData.LayerMask,
                WeaponData.QueryTriggerInteraction) == false)
                return;
            Debug.Log("Hit");
            if (raycastHit.transform.TryGetComponent(out ITakeDamaging takeDamaging) == true)
                takeDamaging.TakeDamage(WeaponData.Damage);
        }

        #endregion
    }
}
