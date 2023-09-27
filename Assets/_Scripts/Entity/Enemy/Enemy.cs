using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Zenject;
using Zenject.SpaceFighter;

public class Enemy : Entity
{
    [SerializeField] private float _detectionRange = 5f;
    [SerializeField] private float _attackDistance = 1f;
    [SerializeField] private float _speedAttack = 10;
    [SerializeField] private int _damage = 10;

    private Rigidbody2D _rb;
    private Player _player;
    private bool isAttack = false;
    private bool isStartAttack = false;
    private bool facingRight = true;
    private CancellationTokenSource _token;
    

    [Inject]
    private void Construct(Player player)
    {
        _player = player;   
    }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private async UniTaskVoid MoveToTargen()
    {
        _token = new CancellationTokenSource();

        while (!_token.IsCancellationRequested)
        {           
            if(!isAttack)
            {
                Vector2 direction = (_player.transform.position - transform.position).normalized;
                _rb.velocity = direction * _speedMovement;

                if (direction.x > 0 && !facingRight || direction.x < 0 && facingRight)              
                    Flip();               
            }
                      
            if (Vector2.Distance(transform.position, _player.transform.position) < _attackDistance)
            {
                isAttack = true;
                Attack();
                await UniTask.Delay(TimeSpan.FromMinutes(AttackDelay(_speedAttack)), cancellationToken: _token.Token).
                       SuppressCancellationThrow();
            }
            else
            {   
                isAttack = false;
                await UniTask.Delay(TimeSpan.FromMinutes(AttackDelay(_speedAttack)), cancellationToken: _token.Token).
                       SuppressCancellationThrow();
            }         
        }
    }

    private void Attack()
    {
        Debug.Log("Attack melle");
        _player.TakeDamage(_damage);
    }

    protected override void Die()
    {
        gameObject.SetActive(false);
        _token.Cancel();
        base.Die();
    }

    private double AttackDelay(float speedAttack)
    {
        return 1 / speedAttack;
    }
    private void Flip()
    {
        facingRight = !facingRight;

        Vector2 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }
    private void Update()
    {   
        if(isStartAttack) return;
        float distanceToPlayer = Vector2.Distance(transform.position, _player.transform.position);

        if(distanceToPlayer < _detectionRange && !isStartAttack)
        {   
            isStartAttack = true;
            MoveToTargen().Forget();
            Debug.Log("Start Attack");
        }
    }
}
