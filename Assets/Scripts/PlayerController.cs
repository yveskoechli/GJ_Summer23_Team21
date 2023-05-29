using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    #region Inspector

    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float jumpForce = 15f;

    [SerializeField] private LayerMask groundLayer;

    #endregion

    private GameInput input;
    private InputAction moveAction;

    private Rigidbody2D rbPlayer;
    
    #region Unity Event Functions
    private void Awake()
    {
        input = new GameInput();
        rbPlayer = GetComponent<Rigidbody2D>();

        input.Player.Jump.performed += Jump;
    }
    
    private void OnEnable()
    {
        EnableInput();
    }

    private void Update()
    {
        //TODO Update stuff..
        
    }

    private void OnDisable()
    {
        DisableInput();
    }

    private void OnDestroy()
    {
        //Unsubscribe from input events
        input.Player.Jump.performed -= Jump;
        
    }

    #endregion
    
    public void EnableInput()
    {
        input.Enable();
    }

    public void DisableInput()
    {
        input.Disable();
    }


    #region Inputs

    private void Jump(InputAction.CallbackContext _)
    {
        if (GroundCheck())
        {
            rbPlayer.velocity = new Vector2(rbPlayer.velocity.x, jumpForce);
            Debug.Log("Jump performed");
        }
    }

    #endregion


    private bool GroundCheck()
    {
        bool isGrounded = Physics2D.Raycast(transform.position, Vector2.down, 0.1f, groundLayer);

        return isGrounded;
    }
}
