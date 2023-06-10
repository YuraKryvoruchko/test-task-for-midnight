using System;

namespace FPS
{
    public class BulletValue
    {
        #region Fields

        private int _value;

        #endregion

        #region Constructor

        public BulletValue(int value)
        {
            _value = value;
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
