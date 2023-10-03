using System;
using UnityEngine;

public class Entity : MonoBehaviour, IDamageable
{
    [SerializeField] protected GameplayCanvas _gameplayCanvas;

    [Header("Stats")]   
    [SerializeField] protected int _currentHealth;
    [SerializeField] protected int _maxHealth;
    [SerializeField] protected int _speedMovement;

    public int CurrentHealth => _currentHealth;
    public int MaxHealth
    {
        get { return _maxHealth; }
        set
        {          
            _maxHealth = Mathf.Max(value, _currentHealth);
        }
    }


    public event Action<int> OnTakeDamage;
     
    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;

        OnTakeDamage?.Invoke(_currentHealth);

        if (_currentHealth <= 0)
        {
            Die();
        }
    }
    protected virtual void Die()
    {                
    }
}
