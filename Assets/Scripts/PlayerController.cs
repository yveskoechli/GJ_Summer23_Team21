using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    #region Inspector

    [SerializeField] private float moveSpeed = 10f;
    //[SerializeField] private float jumpForce = 15f;

    [SerializeField] private LayerMask groundLayer;

    #endregion

    private GameInput input;
    private InputAction moveAction;

    private Rigidbody2D rbPlayer;
    private Vector2 moveInput;

    private CircleCollider2D interactionArea;

    private Ingredient actualIngredient;
    
    #region Unity Event Functions
    private void Awake()
    {
        input = new GameInput();
        rbPlayer = GetComponent<Rigidbody2D>();

        interactionArea = GetComponentInChildren<CircleCollider2D>();

        moveAction = input.Player.Move;
        
        input.Player.Collect.performed += Collect;
        input.Player.Interact.performed += Interact;
    }
    
    private void OnEnable()
    {
        EnableInput();
    }

    private void Update()
    {
        //TODO Update stuff..
        ReadInput();
        Move(moveInput);
    }

    private void OnDisable()
    {
        DisableInput();
    }

    private void OnDestroy()
    {
        //Unsubscribe from input events
        input.Player.Collect.performed -= Collect;
        input.Player.Interact.performed -= Interact;
        
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


    private void ReadInput()
    {
        moveInput = moveAction.ReadValue<Vector2>();
    }
    private void Move(Vector2 moveInput)
    {
        rbPlayer.velocity = moveInput * moveSpeed;
    }
    
    private void Collect(InputAction.CallbackContext _)
    {
        Debug.Log("Collected");

        if (actualIngredient != null)
        {
            actualIngredient.Collected();
        }
        else
        {
            Debug.Log("No Ingredient triggered");
        }
        
        /*if (GroundCheck())
        {
            rbPlayer.velocity = new Vector2(rbPlayer.velocity.x, jumpForce);
            Debug.Log("Jump performed");
        }*/
    }

    private void Interact(InputAction.CallbackContext _)
    {
        Debug.Log("Interacted");
        
        /*if (GroundCheck())
        {
            rbPlayer.velocity = new Vector2(rbPlayer.velocity.x, jumpForce);
            Debug.Log("Jump performed");
        }*/
    }
    
    #endregion


    #region Triggers

    private void OnTriggerEnter(Collider other)
    {


        
    }

    public void SetSelectedIngredient(Ingredient ingredient)
    {
        actualIngredient = ingredient;
    }
    

    private void OnTriggerEnter2D(Collider2D col)
    {
       if (col.CompareTag("Ingredient"))
        {
            actualIngredient = col.GetComponent<Ingredient>();
            Debug.Log("Actual_Ingredient_Selected");
        }
    }

    #endregion

    
}
