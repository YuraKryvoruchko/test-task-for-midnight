using System;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour, ITakeDamaging
{
    #region Fields

    [SerializeField] private int _defaultHealth = 100;

    private int _currentHealth = 100;

    #endregion

    #region Actions

    public event Action OnDeath;

    #endregion

    #region Unity Methods

    public void Awake()
    {
        _currentHealth = _defaultHealth;
    }

    #endregion

    #region Public Methods

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        if (_currentHealth <= 0)
            OnDeath?.Invoke();
    }

    #endregion
}
public interface ITakeDamaging
{
    void TakeDamage(int damage);
}
