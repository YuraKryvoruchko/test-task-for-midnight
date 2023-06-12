using System;
using System.Collections.Generic;
using UnityEngine;
using FPS.AI;

namespace FPS.Infrastructure
{
    public class Game : MonoBehaviour
    {
        #region Fields

        [Header("Setup Game")]
        [SerializeField] private Player _playerPrefab;
        [SerializeField] private Transform _playerSpawnPoint;
        [Space]
        [SerializeField] private Swat _swatPrefab;
        [SerializeField] private Transform[] _enemySpawnPoints;

        private Player _currentPlayer;
        private List<Swat> _swats;

        private int _liveBotCount = 0;

        private bool _isEnd = false;

        #endregion

        #region Actions

        public event Action OnWin;
        public event Action OnLose;
        public event Action<int, int> OnDeadEnemy;

        #endregion

        #region Unity Methods

        private void OnApplicationFocus(bool focus)
        {
            if (focus == true)
                LockCursor();
            else
                UnlockCursor();

            if(_isEnd == true)
                UnlockCursor();
        }
        private void Awake()
        {
            LockCursor();
            _swats = new List<Swat>();
        }
        private void Start()
        {
            InstallPlayer();
            InstallSwats();
        }

        #endregion

        #region Private Methods

        private void InstallPlayer()
        {
            _currentPlayer = Instantiate(_playerPrefab, _playerSpawnPoint);
            _currentPlayer.Init();
            _currentPlayer.OnDeath += HandlePlayerDead;
        }
        private void InstallSwats()
        {
            foreach (Transform enemySpawnPoint in _enemySpawnPoints)
            {
                Swat swat = Instantiate(_swatPrefab, enemySpawnPoint);
                swat.OnDead += HandleBotDead;
                swat.Init(_currentPlayer);
                _swats.Add(swat);
            }
            _liveBotCount = _enemySpawnPoints.Length;
        }
        private void HandleBotDead()
        {
            _liveBotCount--;
            OnDeadEnemy?.Invoke(_liveBotCount, _swats.Count);
            if (_liveBotCount == 0)
                HandleWin();
        }
        private void HandlePlayerDead()
        {
            _currentPlayer.OnDeath -= HandlePlayerDead;
            HandleLose();
        }
        private void HandleWin()
        {
            _isEnd = true;
            _currentPlayer.Deactivate();
            UnlockCursor();

            int currentWinCount = SaveSystem.GetInt(SaveLoadKeys.WIN_COUNT);
            currentWinCount++;
            SaveSystem.SetInt(SaveLoadKeys.WIN_COUNT, currentWinCount++);
            OnWin?.Invoke();
        }
        private void HandleLose()
        {
            _isEnd = true;
            UnlockCursor();

            int currentLoseCount = SaveSystem.GetInt(SaveLoadKeys.LOSE_COUNT);
            currentLoseCount++;
            SaveSystem.SetInt(SaveLoadKeys.LOSE_COUNT, currentLoseCount++);
            OnLose?.Invoke();
        }
        private void LockCursor()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        private void UnlockCursor()
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
        }

        #endregion
    }
}
