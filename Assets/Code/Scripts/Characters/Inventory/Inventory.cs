using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    public class Inventory : MonoBehaviour
    {
        #region Fields

        [SerializeField] private List<Weapon> _weapons;

        #endregion

        #region Public Methods

        public Weapon GetWeapon(Weapons weapons)
        {
            return _weapons[(int)weapons];
        }

        #endregion
    }
}
