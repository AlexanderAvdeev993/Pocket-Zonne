using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;


public class Enemy : Entity
{
    [SerializeField] private float _detectionRange = 5f;
    [SerializeField] private float _attackDistance = 1f;
    [SerializeField] private float _speedAttack = 10;
    [SerializeField] private int _damage = 10;
    [SerializeField] private Item[] _itemsForDrops;

    private Rigidbody2D _rb;
    private Player _player;
    private CancellationTokenSource _token;
    private bool isAttack = false;
    private bool isStartAttack = false;
    private bool facingRight = true;
    private ItemFactory _itemFactory;
    
    public event Action<Enemy> OnEnemyDie;

    [Inject]
    private void Construct(Player player, ItemFactory itemFactory)
    {
        _player = player;   
        _itemFactory = itemFactory; 
    }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _gameplayCanvas.ActiveDetectionZona(false);
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
    private void OnEnable()
    {
        OnTakeDamage += ProvokingAttack;
    }
    private void OnDisable()
    {
        OnTakeDamage -= ProvokingAttack;
    }

    private void ProvokingAttack(int damage)
    {
        if (isStartAttack) return;

        isStartAttack = true;
        MoveToTargen().Forget();
    }

    private void Attack()
    {       
        if (_player == null) return;
        Debug.Log("Attack melle");
        _player.TakeDamage(_damage);
    }

    protected override void Die()
    {       
        _itemFactory.DropItem(_itemsForDrops, gameObject.transform.position);
        _token?.Cancel();
        OnEnemyDie?.Invoke(this);
        gameObject.SetActive(false);             
    }

    public void RespawnEnemy()
    {   
        CurrentHealth = MaxHealth;
        _gameplayCanvas.ChangeHealth(MaxHealth);
        _token?.Cancel();
        isStartAttack = false;
    }
  
    private double AttackDelay(float speedAttack)
    {
        return 1 / speedAttack;
    }
    private void Flip()
    {
        facingRight = !facingRight;

        FlipObject(transform);
        FlipObject(_gameplayCanvas.transform);
    }
    private void FlipObject(Transform objectToFlip)
    {
        Vector2 localScale = objectToFlip.localScale;
        localScale.x *= -1;
        objectToFlip.localScale = localScale;
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
