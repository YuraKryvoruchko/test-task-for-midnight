using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    public class Inventory : MonoBehaviour
    {
        #region Fields

        [SerializeField] private List<Weapon> _weapons;

        private Dictionary<WeaponModel, Weapon> _typeWeaponPairs = new Dictionary<WeaponModel, Weapon>();
        private Dictionary<BulletModel, BulletValue> _typeBulletPairs = new Dictionary<BulletModel, BulletValue> 
        {
            { BulletModel.AK, new BulletValue(120) },
            { BulletModel.Pistol, new BulletValue(60) },
            { BulletModel.ShootGun, new BulletValue(24) }
        };

        #endregion

        #region Unity Methods

        private void Awake()
        {
            _typeWeaponPairs = new Dictionary<WeaponModel, Weapon>();
            foreach (Weapon weapon in _weapons)
                _typeWeaponPairs.Add(weapon.WeaponData.WeaponModel, weapon);
        }

        #endregion

        #region Public Methods

        public Weapon GetWeapon(WeaponModel weaponModel)
        {
            return _typeWeaponPairs[weaponModel];
        }
        public BulletValue GetBullets(BulletModel bulletModel)
        {
            return _typeBulletPairs[bulletModel];
        }
        public int GetBulletCount(BulletModel bulletModel)
        {
            return _typeBulletPairs[bulletModel].GetValue();
        }

        #endregion
    }
}
