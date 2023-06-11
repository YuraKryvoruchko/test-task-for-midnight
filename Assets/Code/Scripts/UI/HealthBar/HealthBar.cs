using UnityEngine;
using UnityEngine.UI;

namespace FPS.UI
{
    public class HealthBar : MonoBehaviour
    {
        #region Fields

        [Header("Image")]
        [SerializeField] private Image _progress;
        [Space]
        [SerializeField] private Health _playerHealth;

        #endregion

        #region Unity Methods

        private void OnEnable()
        {
            _playerHealth.OnChange += HandleHealthChanging;
        }
        private void OnDisable()
        {
            _playerHealth.OnChange += HandleHealthChanging;
        }

        #endregion

        #region Private Methods

        private void HandleHealthChanging(int newValue, int defaultValue)
        {
            _progress.fillAmount = (float)newValue / (float)defaultValue;
        }

        #endregion
    }
}
