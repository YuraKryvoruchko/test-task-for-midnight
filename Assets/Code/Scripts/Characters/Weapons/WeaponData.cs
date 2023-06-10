using UnityEngine;

namespace FPS
{
    [CreateAssetMenu(fileName = "WeponData", menuName = "ScriptableObjects/WeaponData", order = 1)]
    public class WeaponData : ScriptableObject
    {
        #region Fields

        public int Damage = 20;
        public int MaxBulletCount = 30;

        public float Rate = 0.2f;

        public WeaponModel WeaponModel;
        public BulletModel BulletModel;

        #endregion

        #region Public Methods

        public WeaponData GetCopy()
        {
            WeaponData weaponData = CreateInstance<WeaponData>();
            weaponData.Damage = Damage;
            weaponData.MaxBulletCount = MaxBulletCount;
            weaponData.Rate = Rate;
            weaponData.WeaponModel = WeaponModel;
            weaponData.BulletModel = BulletModel;

            return weaponData;
        }

        #endregion
    }
}