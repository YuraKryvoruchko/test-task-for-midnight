using UnityEngine;

namespace FPS.Infrastructure
{
    public class GameEndDisplay : MonoBehaviour
    {
        #region Fields

        [Header("UI")]
        [SerializeField] private GameObject _winPanel;
        [SerializeField] private GameObject _losePanel;
        [Header("GameEnd")]
        [SerializeField] private Game _gameEnd;

        #endregion

        #region Unity Methods

        private void OnEnable()
        {
            _gameEnd.OnWin += HandleWin;
            _gameEnd.OnLose += HandleLose;
        }
        private void OnDisable()
        {
            _gameEnd.OnWin -= HandleWin;
            _gameEnd.OnLose -= HandleLose;
        }

        #endregion

        #region Private Methods

        private void HandleWin()
        {
            _winPanel.SetActive(true);
        }
        private void HandleLose()
        {
            _losePanel.SetActive(true);
        }

        #endregion
    }
}
