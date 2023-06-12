using UnityEngine;
using FPS.AI;

namespace FPS.Infrastructure
{
    public class LocationInstaller : MonoBehaviour
    {
        #region Fields

        [Header("Player Settings")]
        [SerializeField] private Player _playerPrefab;
        [SerializeField] private Transform _playerSpawnPoint;
        [Space]
        [SerializeField] private Swat _swatPrefab;
        [SerializeField] private Transform[] _enemySpawnPoints;

        private int _allBotCount = 0;
        private int _deadBotCount = 0;

        #endregion

        #region Unity Methods

        private void Start()
        {
            Player player = Instantiate(_playerPrefab, _playerSpawnPoint);
            player.Init();
            player.OnDeath += HandlePlayerDead;
            foreach (Transform enemySpawnPoint in _enemySpawnPoints)
            {
                _allBotCount++;
                Swat swat = Instantiate(_swatPrefab, enemySpawnPoint);
                swat.Init(player);
                swat.OnDead += HandleBotDead;
            }
        }

        #endregion

        #region Private Methods

        private void HandleBotDead()
        {
            _deadBotCount++;
            if (_deadBotCount == _allBotCount)
                Debug.Log("Win");
        }
        private void HandlePlayerDead()
        {
            Debug.Log("Lose");
        }

        #endregion
    }
    public class EnemyMarker : MonoBehaviour
    {
        
    }
}
