using UnityEngine;

namespace FPS
{
    public class AK74M : Weapon
    {
        #region Fields

        #endregion

        #region Public Methods

        public override void Shoot()
        {
            Animator.SetBool("IsShoot", true);
            Ray ray = new Ray(PlayerCamera.transform.position, PlayerCamera.transform.forward);
            Debug.DrawRay(ray.origin, ray.direction, Color.red, int.MaxValue);
            if (Physics.Raycast(ray, out RaycastHit raycastHit, int.MaxValue ) == false)
                return;
            Debug.Log("Hit");
            if (raycastHit.transform.TryGetComponent(out ITakeDamaging takeDamaging) == true)
                takeDamaging.TakeDamage(20);
        }
        public override void Reload()
        {
            Animator.SetTrigger("Reload");  
        }

        #endregion
    }
}
