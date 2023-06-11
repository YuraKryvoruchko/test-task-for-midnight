﻿using UnityEngine;

namespace FPS
{
    [CreateAssetMenu(fileName = "WeponData", menuName = "ScriptableObjects/WeaponData", order = 1)]
    public class WeaponData : ScriptableObject
    {
        #region Fields

        public int Damage = 20;
        public int MaxBulletCount = 30;
        [Header("Spread Settings")]
        public float SpreadInIdle;
        public float SpreadInMove;
        public float SpreadInAim;
        [Space]
        public int ReloadTime = 3000;
        public int RateInMS = 200;
        [Space]
        public WeaponModel WeaponModel;
        public BulletModel BulletModel;

        #endregion

        #region Public Methods

        public WeaponData GetCopy()
        {
            WeaponData weaponData = CreateInstance<WeaponData>();
            weaponData.Damage = Damage;
            weaponData.MaxBulletCount = MaxBulletCount;
            weaponData.RateInMS = RateInMS;
            weaponData.WeaponModel = WeaponModel;
            weaponData.BulletModel = BulletModel;

            return weaponData;
        }

        #endregion
    }
}