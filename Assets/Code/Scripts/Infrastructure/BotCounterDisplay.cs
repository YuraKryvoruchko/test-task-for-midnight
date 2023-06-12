using UnityEngine;
using TMPro;

namespace FPS.Infrastructure
{
    public class BotCounterDisplay : MonoBehaviour
    {
        #region Fields

        [Header("UI")]
        [SerializeField] private TMP_Text _liveBotCounter;
        [Header("GameEnd")]
        [SerializeField] private Game _game;

        #endregion

        #region Unity Methods

        private void OnEnable()
        {
            _game.OnDeadEnemy += HandleBotDead;
        }
        private void OnDisable()
        {
            _game.OnDeadEnemy -= HandleBotDead;
        }

        #endregion

        #region Private Methods

        private void HandleBotDead(int liveBotCount, int allBotCount)
        {
            _liveBotCounter.text = $"Enemy: {liveBotCount}/{allBotCount}";
        }

        #endregion
    }
}
