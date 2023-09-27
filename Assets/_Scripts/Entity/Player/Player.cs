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

    
    private void Awake()
    {    
        _rb = GetComponent<Rigidbody2D>();
        _inputSystem = new InputSystem();
       

        _inputSystem.Player.Move.performed += context => _moveInput = context.ReadValue<Vector2>();
        _inputSystem.Player.Move.canceled += context => _moveInput = Vector2.zero;
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

        Vector2 movement = new Vector2(horizontal, vertical).normalized * _speedMovement;

        _rb.velocity = movement * Time.deltaTime;
    }

    private void Update()
    {
        Move();        
    }
}
