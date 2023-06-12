using System;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    public class Inventory : MonoBehaviour
    {
        #region Fields

        [SerializeField] private int _maxWeaponCount = 3;
        [Space]
        [SerializeField] private List<Weapon> _defaultWeapons;
        [SerializeField] private List<BulletValue> _defaultBullets;

        private Dictionary<WeaponModel, int> _weaponIndexPairs =
            new Dictionary<WeaponModel, int>();
        private Dictionary<WeaponModel, Weapon> _typeWeaponPairs = 
            new Dictionary<WeaponModel, Weapon>();
        private Dictionary<BulletModel, BulletValue> _typeBulletPairs = 
            new Dictionary<BulletModel, BulletValue>();

        #endregion

        #region Actions

        public event Action<int, Weapon> OnAdd;
        public event Action<int, Weapon> OnRemove;

        #endregion

        #region Public Methods

        public void Init()
        {
            _weaponIndexPairs = new Dictionary<WeaponModel, int>();
            _typeWeaponPairs = new Dictionary<WeaponModel, Weapon>();
            _typeBulletPairs = new Dictionary<BulletModel, BulletValue>();

            if (_defaultWeapons.Count > _maxWeaponCount)
                throw new Exception("Inventory.Init: the default weapons is more than the max weapon count!");

            foreach (Weapon weapon in _defaultWeapons)
                AddWeapon(weapon);
            foreach (BulletValue bulletsValue in _defaultBullets)
                AddBullets(bulletsValue.BulletModel, bulletsValue);
        }
        public void AddWeapon(Weapon weapon)
        {
            if (_typeWeaponPairs.Count >= _maxWeaponCount)
                throw new Exception("Inventory.AddWeapon: The weapon count is equal to the max weapon count!");

            _typeWeaponPairs.Add(weapon.WeaponData.WeaponModel, weapon);
            _weaponIndexPairs.Add(weapon.WeaponData.WeaponModel, _weaponIndexPairs.Count - 1);
            OnAdd?.Invoke(_typeWeaponPairs.Count - 1, weapon);
        }
        public bool CanAddWeapon()
        {
            if (_typeWeaponPairs.Count < _maxWeaponCount)
                return true;

            return false;
        }
        public void RemoveWeapon(WeaponModel weaponModel)
        {
            OnRemove?.Invoke(_weaponIndexPairs[weaponModel], _typeWeaponPairs[weaponModel]);
            _typeWeaponPairs.Remove(weaponModel);
            _weaponIndexPairs.Remove(weaponModel);
        }
        public Weapon GetWeapon(WeaponModel weaponModel)
        {
            return _typeWeaponPairs[weaponModel];
        }
        public Weapon GetWeapon(int index)
        {
            IEnumerator<WeaponModel> keys = _typeWeaponPairs.Keys.GetEnumerator();
            for(int i = 0; i < index; i++)
                keys.MoveNext();

            return _typeWeaponPairs[keys.Current];
        }
        public void AddBullets(BulletModel bulletModel, int value)
        {
            if (_typeBulletPairs.ContainsKey(bulletModel) == true)
                _typeBulletPairs[bulletModel].AddValue(value);
            else
                _typeBulletPairs.Add(bulletModel, new BulletValue(value, bulletModel));
        }
        public void AddBullets(BulletModel bulletModel, BulletValue bulletValue)
        {
            if (_typeBulletPairs.ContainsKey(bulletModel) == true)
                _typeBulletPairs[bulletModel].AddValue(bulletValue.GetValue());
            else
                _typeBulletPairs.Add(bulletModel, bulletValue);
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
