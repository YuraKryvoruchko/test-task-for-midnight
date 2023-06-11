using System;
using UnityEngine;

public class Health : MonoBehaviour, ITakeDamaging
{
    #region Fields

    [SerializeField] private int _defaultHealth = 100;

    private int _currentHealth = 100;

    #endregion

    #region Properties

    public bool IsDead { get; private set; }

    #endregion

    #region Actions

    public event Action<int, int> OnChange;
    public event Action OnDeath;

    #endregion

    #region Unity Methods

    public void Awake()
    {
        IsDead = false;
        _currentHealth = _defaultHealth;
    }

    #endregion

    #region Public Methods

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        OnChange?.Invoke(_currentHealth, _defaultHealth);

        if (_currentHealth > 0)
            return;

        IsDead = true;
        OnDeath?.Invoke();
    }

    #endregion
}
public interface ITakeDamaging
{
    void TakeDamage(int damage);
}
