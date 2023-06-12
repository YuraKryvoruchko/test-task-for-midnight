using System;
using UnityEngine;
using UnityEngine.AI;
using Cysharp.Threading.Tasks;

namespace FPS.AI
{
    public class Swat : MonoBehaviour
    {
        #region Fields

        [SerializeField] private Health _health;
        [SerializeField] private BotVision _vision;
        [SerializeField] private Collider _swatCollider;
        [Header("Weapon")]
        [SerializeField] private Weapon _weapon;
        [SerializeField] private Transform _shootPoint;
        [Header("NPC Controller")]
        [SerializeField] private float _maxSpeed = 3.5f;
        [SerializeField] private float _maxRotationSpeed = 40f;
        [SerializeField] private float _maxDistanceToPlayer = 16f;
        [SerializeField] private float _minDistanceToPlayer = 11f;
        [SerializeField] private NavMeshAgent _navMeshAgent;
        [Header("Animations Settings")]
        [SerializeField] private Animator _animator;
        [SerializeField] private int _shootDelayInMS = 1160;
        [SerializeField] private int _reloadDelayInMS = 3300;
        [SerializeField] private string _isShootParameter = "IsShoot";
        [SerializeField] private string _isReloadParameter = "IsReload";
        [SerializeField] private string _xAxitParameter = "XAxit";
        [SerializeField] private string _zAxitParameter = "ZAxit";
        [SerializeField] private string _deathParameter = "Dead";
            
        private Player _player;

        private bool _playerInZone = false;

        private bool _canMakeNextShoot = true;
        private bool _isReload = false;

        private bool _playerIsDead = false;

        private const int NEXT_CORNER = 1;

        #endregion

        #region Actions

        public event Action OnDead;

        #endregion

        #region Unity Methods

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Player player) == true)
                _playerInZone = true;
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out Player player) == true)
                _playerInZone = false;
        }
        private void Update()
        {
            if (_health.IsDead == true || _playerIsDead == true || _playerInZone == false)
                return;

            LookEnemy();

            float distanceToPlayer = GetDistanceToPlayer();
            if (distanceToPlayer > _maxDistanceToPlayer)
                MoveToPlayer();
            else if (distanceToPlayer < _minDistanceToPlayer)
                DepartureFromPlayer();
            else
                Idle();

            UpdateAnimatorMoveParameters();
        }

        #endregion

        #region Public Methods

        public void Init(Player player)
        {
            _player = player;
            _player.OnDeath += HandlePlayerDead;
            _health.OnDeath += Die;
            _weapon.SetShootRayCalculator(new ShootRayCalculatorWithoutCamera(_shootPoint, _player.transform));
        }

        #endregion

        #region Private Methods

        private void MoveToPlayer()
        {
            _navMeshAgent.speed = _maxSpeed;
            if (_navMeshAgent.pathPending == false)
                _navMeshAgent.SetDestination(GetPointMove());

            _animator.SetBool(_isShootParameter, false);
            _weapon.Shake();
        }
        private void DepartureFromPlayer()
        {
            _navMeshAgent.speed = _maxSpeed;
            _animator.SetBool(_isShootParameter, false);
            _navMeshAgent.Move(Vector3.back * _navMeshAgent.speed * Time.deltaTime);
            _weapon.Shake();
        }
        private void Idle()
        {
            _navMeshAgent.speed = 0;
            _weapon.StopShake();

            if (_vision.PlayerIsDiscovered == true)
                ShootToPlayer();
        }
        private async void ShootToPlayer()
        {
            if (_canMakeNextShoot == false || _isReload == true)
                return;
            if (_weapon.CurrentBulletCount == 0)
            {
                Reload();
                return;
            }

            _animator.SetBool(_isShootParameter, true);
            _weapon.StartShoot();
            _weapon.StopShoot();

            _canMakeNextShoot = false;
            await UniTask.Delay(_shootDelayInMS);
            _canMakeNextShoot = true;
        }
        private async void Reload()
        {
            _isReload = true;
            _animator.SetBool(_isReloadParameter, _isReload);
            _weapon.Reload(new BulletValue(_weapon.WeaponData.MaxBulletCount, BulletModel.AK));

            await UniTask.Delay(_reloadDelayInMS);
            _isReload = false;
            _animator.SetBool(_isReloadParameter, _isReload);
        }
        private void LookEnemy()
        {
            Vector3 direction = _player.transform.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation,
                _maxRotationSpeed * Time.deltaTime);
        }
        private Vector3 GetPointMove()
        {
            Vector3 point = new Vector3(_player.transform.position.x, transform.position.y,
                _player.transform.position.z);
            return point;
        }
        private void UpdateAnimatorMoveParameters()
        {
            Vector3 inverseMoveDirection = Vector3.zero;
            if (_navMeshAgent.path.corners.Length > 1 && _navMeshAgent.speed > 0)
            {
                Vector3 direction = (_navMeshAgent.path.corners[NEXT_CORNER] - transform.position).normalized;
                inverseMoveDirection = transform.InverseTransformDirection(direction);
            }

            _animator.SetFloat(_xAxitParameter, inverseMoveDirection.x);
            _animator.SetFloat(_zAxitParameter, inverseMoveDirection.z);
        }
        private float GetDistanceToPlayer()
        {
            return Vector3.Distance(transform.position, _player.transform.position);
        }
        private void Die()
        {
            _swatCollider.enabled = false;
            _navMeshAgent.enabled = false;
            _vision.enabled = false;
            _health.OnDeath -= Die;
            _animator.SetTrigger(_deathParameter);
            _player.OnDeath -= HandlePlayerDead;
            OnDead?.Invoke();
        }
        private void HandlePlayerDead()
        {
            _player.OnDeath -= HandlePlayerDead;
            _playerIsDead = true;
            _navMeshAgent.isStopped = true;
            _vision.enabled = false;
            _animator.SetBool(_isShootParameter, false);
            _animator.SetBool(_isReloadParameter, false);
        }

        #endregion
    }
}

