﻿using UnityEngine;
using Cysharp.Threading.Tasks;

namespace FPS
{
    public class AK74M : Weapon
    {
        #region Public Methods

        private void OnGUI()
        {
            float coefficient = (float)Screen.width / (float)Screen.height;
            Rect rect = new Rect(Screen.width / 2 - Screen.width * 0.03f,
                Screen.height / 2 - Screen.height * 0.03f * coefficient,
                (Screen.width * 0.03f) * 2, (Screen.height * 0.03f) * 2 * coefficient);

            GUI.Box(rect, "");
        }
        public async override void StartShoot()
        {
            IsShooting = true;
            Animator.SetBool("IsShoot", IsShooting);
            while (IsShooting == true)
            {
                if (IsReload == true || CurrentBulletCount <= 0)
                    break;

                Shoot();
                await UniTask.Delay(WeaponData.RateInMS);
            }

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

        public void Shoot()
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
