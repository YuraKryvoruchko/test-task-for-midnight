using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Cysharp.Threading.Tasks;
using FPS;

using Random = UnityEngine.Random;

namespace FPS.AI
{
    //    private void Awake()
    //    {
    //        NavAgent = GetComponent<NavMeshAgent>();
    //        NavAgent.speed = Speed;
    //        NavAgent.stoppingDistance = MinDistance;

    //        EnemyRigidbody = GetComponent<Rigidbody>();
    //    }

    //    public virtual void TakeDamage(float damage)
    //    {
    //        _health -= damage;

    //        if (_health <= 0)
    //        {
    //            OnDead?.Invoke();

    //            OnDead = null;
    //        }
    //    }

    //    protected bool CanAttack()
    //    {
    //        if (PlayerCurrent == null)
    //        {
    //            Debug.LogWarning("Player is null!");

    //            return false;
    //        }

    //        float distance = GetDistance(PlayerCurrent.transform.position);

    //        if (distance <= MinDistance && TargetDetected == true)
    //            return true;
    //        else
    //            return false;
    //    }
    //    protected float GetDistance(Vector3 point)
    //    {
    //        _distanceToPlayer = Vector3.Distance(NavAgent.transform.position, point);

    //        return _distanceToPlayer;
    //    }

    //    protected virtual void MoveToEnemy()
    //    {
    //        if (_canChangePath == false)
    //            return;

    //        StartCoroutine(RunTimerToChangePath());

    //        Vector3 point = GetPointMove();

    //        NavAgent.SetDestination(point);
    //    }
    //    protected virtual void DepartureFromEnemy()
    //    {
    //        NavAgent.Move(Vector3.back * Speed * Time.deltaTime);
    //    }
    public class Swat : MonoBehaviour
    {
        #region Fields

        [SerializeField] private Health _health;
        [SerializeField] private EnemyVision _enemyVision;
        [SerializeField] private Collider _swatCollider;
        [Header("Weapon")]
        [SerializeField] private Weapon _weapon;
        [Header("NPC Controller")]
        [SerializeField] private int _changePathDelay = 1;
        [SerializeField] private float _maxSpeed = 5f;
        [SerializeField] private float _maxRotationSpeed = 40f;
        [SerializeField] private Vector3 _offsetFromPlayer;
        [SerializeField] private NavMeshAgent _navMeshAgent;
        [Header("Animations Settings")]
        [SerializeField] private Animator _animator;
        [SerializeField] private string _speedParameter = "Speed";
        [SerializeField] private string _isShootParameter = "IsShoot";
        [SerializeField] private string _isReloadParameter = "IsReload";
        [SerializeField] private string _xAxitParameter = "XAxit";
        [SerializeField] private string _zAxitParameter = "ZAxit";
        [SerializeField] private string _deathParameter = "Dead";

        [SerializeField] private Player _player;

        private float _currentSpeed;
        private bool _playerIsDead = false;

        #endregion

        #region Actions

        public event Action OnDead;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            _health.OnDeath += Die;
            _player.OnDeath += HandlePlayerDead;
            _enemyVision.EnemyDiscovered += HandleEnemyDiscovering;
            _navMeshAgent.stoppingDistance = 10f;
        }
        private void Update()
        {
            if (_health.IsDead == true || _playerIsDead == true)
                return;

            LookEnemy();

            float distanceToPlayer = GetDistanceToPlayer();
            if (distanceToPlayer > 10f)
                MoveToPlayer();
            else if (distanceToPlayer < 5f)
                DepartureFromEnemy();

            UpdateAnimatorMoveParameters();
        }

        #endregion

        #region Public Methods

        #endregion

        #region Private Methods

        private void MoveToPlayer()
        {
            if(_navMeshAgent.pathPending == false)
                _navMeshAgent.SetDestination(GetPointMove());
        }
        private void DepartureFromEnemy()
        {
            _navMeshAgent.Move(Vector3.back * _maxSpeed * Time.deltaTime);
        }
        private Vector3 GetPointMove()
        {
            Vector3 playerTransform = new Vector3(_player.transform.position.x, 0,
                _player.transform.position.z);

            float offsetX = Random.value > 0.5f ? _offsetFromPlayer.x : -_offsetFromPlayer.x;
            float offsetY = Random.value > 0.5f ? _offsetFromPlayer.y : -_offsetFromPlayer.y;
            float offsetZ = Random.value > 0.5f ? _offsetFromPlayer.z : -_offsetFromPlayer.z;

            Vector3 offset = new Vector3(offsetX, offsetY, offsetZ);

            Vector3 point = playerTransform + offset;

            return point;
        }
        private void LookEnemy()
        {
            Vector3 direction = _player.transform.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation,
                _maxRotationSpeed * Time.deltaTime);
        }
        private void UpdateAnimatorMoveParameters()
        {
            _animator.SetFloat(_speedParameter, _navMeshAgent.speed);
            if (_navMeshAgent.path.corners.Length < 2)
                return;

            Vector3 direction = (_navMeshAgent.path.corners[1] - transform.position).normalized;
            Vector3 inverseDirection = transform.InverseTransformDirection(direction);
            _animator.SetFloat(_xAxitParameter, inverseDirection.x);
            _animator.SetFloat(_zAxitParameter, inverseDirection.z);
        }
        private float GetDistanceToPlayer()
        {
            return Vector3.Distance(transform.position, _player.transform.position);
        }
        private void Die()
        {
            _swatCollider.enabled = false;
            _navMeshAgent.enabled = false;
            _enemyVision.enabled = false;
            _health.OnDeath -= Die;
            _animator.SetTrigger(_deathParameter);
            OnDead?.Invoke();
        }
        private void HandleEnemyDiscovering()
        {
            _animator.SetBool(_isShootParameter, true);
            _weapon.StartShoot();
        }
        private void HandlePlayerDead()
        {
            _player.OnDeath -= HandlePlayerDead;
            _playerIsDead = true;
            _navMeshAgent.isStopped = true;
            _enemyVision.enabled = false;
        }

        #endregion
    }
}

