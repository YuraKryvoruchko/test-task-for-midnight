using System;
using UnityEngine;

namespace FPS
{
    [Serializable]
    public class BulletValue
    {
        #region Fields

        [SerializeField] private int _value;

        [SerializeField] private BulletModel _bulletModel;

        #endregion

        #region Properties

        public BulletModel BulletModel { get => _bulletModel; private set => _bulletModel = value; }

        #endregion

        #region Constructor

        public BulletValue(int value, BulletModel bulletModel)
        {
            _value = value;
            _bulletModel = bulletModel;
        }

        #endregion

        #region Public Methods

        public int GetValue()
        {
            return _value;
        }
        public void AddValue(int addedVaule)
        {
            _value += addedVaule;
        }
        public void RemoveValue(int removedValue)
        {
            if (removedValue > _value)
                throw new Exception("Can't remove the value when removed value is greater than value");

            _value -= removedValue;
        }

        #endregion
    }
}
