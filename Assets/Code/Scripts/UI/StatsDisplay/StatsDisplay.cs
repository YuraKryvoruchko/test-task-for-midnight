using UnityEngine;
using TMPro;

namespace FPS.UI
{
    public class StatsDisplay : MonoBehaviour
    {
        #region Fields

        [SerializeField] private TMP_Text _winCountText;
        [SerializeField] private TMP_Text _loseCountText;

        private const int DEFAULT_VALUE = 0;

        #endregion

        #region Unity Methods

        private void Start()
        {
            _winCountText.text = SaveSystem.GetInt(SaveLoadKeys.WIN_COUNT, DEFAULT_VALUE).ToString();
            _loseCountText.text = SaveSystem.GetInt(SaveLoadKeys.LOSE_COUNT, DEFAULT_VALUE).ToString();
        }

        #endregion
    }
}
