using System;
using UnityEngine;

public class Entity : MonoBehaviour, IDamageable
{
    [SerializeField] protected GameplayCanvas _gameplayCanvas;

    [Header("Stats")]   
    [SerializeField] protected int _health;
    [SerializeField] protected int _speedMovement;

    public int Health => _health;
    
    public event Action<int> OnTakeDamage;
     
    public void TakeDamage(int damage)
    {
        _health -= damage;

        OnTakeDamage?.Invoke(_health);

        if (_health <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {         
        Debug.Log("Die");
    }
}
