using UnityEngine;
using Cysharp.Threading.Tasks;

namespace FPS
{
    public class AK74M : Weapon
    {
        #region Fields

        private bool _isShooting = false;
        private bool _isReload = false;

        #endregion

        #region Public Methods

        public override void Shoot()
        {
            if (CurrentBulletCount <= 0)
                return;

            Animator.SetBool("IsShoot", true);
            ShootParticle.gameObject.SetActive(true);
            ShootParticle.Play();

            Vector3 startPoint = PlayerCamera.ViewportToWorldPoint(GetStartShootPositon());
            Debug.Log(startPoint);
            Ray ray = new Ray(startPoint, PlayerCamera.transform.forward);
            Debug.DrawRay(ray.origin, ray.direction, Color.red, int.MaxValue);

            CurrentBulletCount -= 1;

            if (Physics.Raycast(ray, out RaycastHit raycastHit, int.MaxValue ) == false)
                return;
            Debug.Log("Hit");
            if (raycastHit.transform.TryGetComponent(out ITakeDamaging takeDamaging) == true)
                takeDamaging.TakeDamage(WeaponData.Damage);

            Animator.SetBool("IsShoot", false);
        }
        public override void Reload(BulletValue bulletValue)
        {
            if (bulletValue.GetValue() == 0)
                return;

            Animator.SetTrigger("Reload");
            int addedValue = WeaponData.MaxBulletCount - CurrentBulletCount;
            if(addedValue > bulletValue.GetValue())
                addedValue = bulletValue.GetValue();

            bulletValue.RemoveValue(addedValue);
            CurrentBulletCount += addedValue;
        }

        #endregion

        #region Private Methods

        protected Vector2 GetStartShootPositon()
        {
            int k = Screen.width / Screen.height;

            float xOffset = Random.Range(-0.4f, 0.4f);
            float yOffset = Random.Range(-0.4f, 0.4f);
            Vector2 middlePoint = new Vector2(0.5f + xOffset, 0.5f + (yOffset / k));
            Debug.Log($"middlePoint: {middlePoint}");
            return middlePoint;
        }

        #endregion
    }
}
