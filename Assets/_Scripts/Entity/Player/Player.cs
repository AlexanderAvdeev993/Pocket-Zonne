using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player : Entity
{   
    private InputSystem _inputSystem;
    private Vector2 _moveInput;
    private Rigidbody2D _rb;
    private bool facingRight = true;
    private Weapon _weapon;
    

    private void Awake()
    {    
        _rb = GetComponent<Rigidbody2D>();
        _inputSystem = new InputSystem();
        
        _weapon = GetComponentInChildren<Weapon>();

        _inputSystem.Player.Move.performed += context => _moveInput = context.ReadValue<Vector2>();
        _inputSystem.Player.Move.canceled += context => _moveInput = Vector2.zero;       
    }




    public void Attack()
    {
        _weapon.StartAttack(this);
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
        FlipObject(_hpBar.transform);
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
        base.Die();
    }

    private void Update()
    {
        Move();        
    }
}
