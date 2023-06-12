using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FPS
{
    public class InventoryDisplay : MonoBehaviour
    {
        #region Fields

        [Header("UI")]
        [SerializeField] private List<Image> _cells;
        [SerializeField] private Sprite _defaultSprite;
        [Header("Inventory")]
        [SerializeField] private Inventory _inventory;

        #endregion

        #region Unity Methods

        private void OnEnable()
        {
            _inventory.OnAdd += HandleAdd;
            _inventory.OnRemove += HandleRemove;
        }
        private void OnDisable()
        {
            _inventory.OnAdd -= HandleAdd;
            _inventory.OnRemove -= HandleRemove;
        }

        #endregion

        #region Private Methods

        private void HandleAdd(int index, Weapon weapon)
        {
            _cells[index].sprite = weapon.WeaponData.Sprite;
        }
        private void HandleRemove(int index, Weapon weapon)
        {
            _cells[index].sprite = _defaultSprite;
        }

        #endregion
    }
}
