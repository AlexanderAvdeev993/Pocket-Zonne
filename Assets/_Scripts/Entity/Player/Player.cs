using System;
using UnityEngine;
using Zenject;

public class Player : Entity , IDamageable
{
    [SerializeField] private float _detectionRadius = 5f;
    [SerializeField] private LayerMask _enemyLayer;

    private InputSystem _inputSystem;
    private Vector2 _moveInput;
    private Rigidbody2D _rb;
    private bool facingRight = true;
    private bool isDead = false;
    private Weapon _weapon;

    private Canvas _canvas;
    private Inventory _inventory;
    private SaveSystem _saveSystem;
    private AmmoCounter _ammoCounter;

    public event Action OnPlayerDie;

    [Inject]
    private void Construct(Canvas canvas, SaveSystem saveSystem, Inventory inventory, AmmoCounter ammoCounter)
    {
        _canvas = canvas;   
        _saveSystem = saveSystem;
        _inventory = inventory; 
        _ammoCounter = ammoCounter; 
    }

    private void Awake()
    {    
        _rb = GetComponent<Rigidbody2D>();       
        _inputSystem = new InputSystem();              
        _weapon = GetComponentInChildren<Weapon>();
        
        _inputSystem.Player.Move.performed += context => _moveInput = context.ReadValue<Vector2>();
        _inputSystem.Player.Move.canceled += context => _moveInput = Vector2.zero;
       
        _gameplayCanvas.InstallationDetectionZona(_detectionRadius);          
    }    
    public void Attack() 
    {   
        _weapon.StartAttack(EnemyDetector());
    }
    public void StopAttack()
    {
        _weapon.PauseAttack();
    }

    private void OnEnable()
    {
        _inputSystem.Enable();
    }
    private void OnDisable()
    {
        _inputSystem.Disable();
    }

    public void LoadPlayerData()
    {
        _saveSystem.Load<PlayerData>("player_data", OnPlayerStatsLoaded);
    }
    private void OnPlayerStatsLoaded(PlayerData playerStats)
    {
        if (playerStats != null)
        {            
            _currentHealth = playerStats.Health;
            _weapon.CurrentAmountAmmo = playerStats.AmountAmmo;
            _speedMovement = playerStats.SpeedMovement;
            _gameplayCanvas.ChangeHealth(CurrentHealth);
            _ammoCounter.UpdateCounter(_weapon.CurrentAmountAmmo);           
        }
    }
    private void OnDestroy()
    {      
        SavePlayerStats();
    }

    private void SavePlayerStats()
    {       
        PlayerData playerData = new PlayerData
        {
            Health = _currentHealth,
            SpeedMovement = _speedMovement,
            AmountAmmo = _weapon.CurrentAmountAmmo          
            
        };      
        _saveSystem.Save("player_data", playerData, (success) =>
        {
            if (success)           
                Debug.Log("player_data saved successfully.");           
            else            
                Debug.LogError("Failed to save player_data.");           
        });
    }


    private void Move()
    {                 
        float horizontal = _moveInput.x;
        float vertical = _moveInput.y;

        if (horizontal > 0 && !facingRight || horizontal < 0 && facingRight)
             Flip();
               
        Vector2 movement = new Vector2(horizontal, vertical).normalized * _speedMovement * Time.deltaTime;
        _rb.velocity = movement ;
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

    protected override void Die()
    {
        _weapon.StopAttack();
        isDead = true;
        OnPlayerDie?.Invoke();
    }

    private Enemy EnemyDetector()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _detectionRadius, _enemyLayer);
        foreach (Collider2D collider in colliders)
        {
            Enemy enemy = collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                return enemy;
            }
        }     
        return null; 
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Item item = other.GetComponent<Item>();
        if (item != null)
        {
            _inventory.AddItem(item);
        }
    }

    public void RestartPlayer()
    {
        _currentHealth = _maxHealth;
        _gameplayCanvas.ChangeHealth(MaxHealth);  
        _weapon.CurrentAmountAmmo = _weapon.MaxAmountAmmo;
        _weapon.canDoAction = true;
        isDead = false;
        _ammoCounter.UpdateCounter(_weapon.MaxAmountAmmo);        
    }

    private void FixedUpdate()
    {  
        if(!isDead)
            Move();             
    }
}
