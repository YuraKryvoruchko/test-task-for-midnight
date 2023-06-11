using UnityEngine;
using TMPro;

namespace FPS.UI
{
    public class WeaponStatsDisplay : MonoBehaviour
    {
        #region Fields

        [Header("Stat Elements")]
        [SerializeField] private TMP_Text _currentBulletCount;
        [SerializeField] private TMP_Text _bulletCountInInventory;
        [Header("Player")]
        [SerializeField] private Player _player;

        private Weapon _currentWeapon;

        #endregion

        #region Unity Methods

        private void OnEnable()
        {
            _player.OnWeaponChange += HandleWeaponChange;
        }
        public void OnDisable()
        {
            _player.OnWeaponChange -= HandleWeaponChange;
        }

        #endregion

        #region Private Methods

        private void HandleWeaponChange(Weapon weapon, int bulletCountInInventory)
        {
            if (_currentWeapon != null)
            {
                _currentWeapon.OnShoot -= HandleShoot;
                _currentWeapon.OnReload -= HandleReload;
            }
            UpdateAllStats(weapon.CurrentBulletCount, bulletCountInInventory);
            _currentWeapon = weapon;
            _currentWeapon.OnShoot += HandleShoot;
            _currentWeapon.OnReload += HandleReload;
        }
        private void HandleShoot(int currentBulletCount)
        {
            _currentBulletCount.text = currentBulletCount.ToString();
        }
        private void HandleReload(int currentBulletCount, int bulletCountInInventory)
        {
            UpdateAllStats(currentBulletCount, bulletCountInInventory);
        }
        private void UpdateAllStats(int currentBulletCount, int bulletCountInInventory)
        {
            _currentBulletCount.text = currentBulletCount.ToString();
            _bulletCountInInventory.text = bulletCountInInventory.ToString();
        }

        #endregion
    }
}
