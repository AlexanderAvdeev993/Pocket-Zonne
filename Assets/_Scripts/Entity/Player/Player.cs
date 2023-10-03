using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Zenject;

public class Player : Entity , IDamageable
{
    [SerializeField] private float _detectionRadius = 5f;
    [SerializeField] private LayerMask _enemyLayer;

    private InputSystem _inputSystem;
    private Vector2 _moveInput;
    private Rigidbody2D _rb;
    private bool facingRight = true;
    private Weapon _weapon;

    private Canvas _canvas;
    private Inventory _inventory;
    private SaveSystem _saveSystem;

    

    [Inject]
    private void Construct(Canvas canvas, SaveSystem saveSystem, Inventory inventory)
    {
        _canvas = canvas;   
        _saveSystem = saveSystem;
        _inventory = inventory; 
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
        _weapon.StopAttack();
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
            _weapon.AmountAmmo = playerStats.AmountAmmo;
            _speedMovement = playerStats.SpeedMovement;
            _gameplayCanvas.ChangeHealth(CurrentHealth);
            _weapon.OnWasteAmmunition(_weapon.AmountAmmo);           
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
            AmountAmmo = _weapon.AmountAmmo          
            
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
        {
            Flip();
        }       
        Vector2 movement = new Vector2(horizontal, vertical).normalized * _speedMovement;

        _rb.velocity = movement * Time.deltaTime;
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
        StopAttack();       
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
    private void Update()
    {
        Move();             
    }
}
