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

        #endregion

        #region Unity Methods

        private void OnEnable()
        {
            _player.OnWeaponChange += UpdateAllStats;
            _player.OnReload += UpdateAllStats;
            _player.OnShoot += HandleShoot;
        }
        public void OnDisable()
        {
            _player.OnWeaponChange -= UpdateAllStats;
            _player.OnReload -= UpdateAllStats;
            _player.OnShoot -= HandleShoot;
        }

        #endregion

        #region Private Methods

        private void UpdateAllStats(Weapon weapon, int bulletCountInInventory)
        {
            _currentBulletCount.text = weapon.CurrentBulletCount.ToString();
            _bulletCountInInventory.text = bulletCountInInventory.ToString();
        }
        private void HandleShoot(Weapon weapon)
        {
            _currentBulletCount.text = weapon.CurrentBulletCount.ToString();
        }

        #endregion
    }
}
