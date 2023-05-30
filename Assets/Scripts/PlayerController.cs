using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private static readonly int HorizontalSpeed = Animator.StringToHash("horizontalSpeed");
    private static readonly int VerticalSpeed = Animator.StringToHash("verticalSpeed");
    
    #region Inspector

    [SerializeField] private float moveSpeed = 10f;

    [SerializeField] private SpriteRenderer carryIngredientSprite;
    //[SerializeField] private float jumpForce = 15f;

    //[SerializeField] private LayerMask groundLayer;

    #endregion

    private GameController gamecontroller;
    private KettleController kettle;
    
    private GameInput input;
    private InputAction moveAction;

    private Rigidbody2D rbPlayer;
    private SpriteRenderer spritePlayer;
    
    private Vector2 moveInput;

    private Animator animator;
    
    private CircleCollider2D interactionArea;

    [SerializeField] private Ingredient actualIngredient;
    [SerializeField] private Ingredient carryedIngredient;

    private bool canInteractKettle = false;
    
    

    #region Unity Event Functions
    private void Awake()
    {
        gamecontroller = FindObjectOfType<GameController>();
        kettle = FindObjectOfType<KettleController>();
        
        input = new GameInput();
        rbPlayer = GetComponent<Rigidbody2D>();
        spritePlayer = GetComponent<SpriteRenderer>();

        animator = GetComponent<Animator>();

        carryIngredientSprite.enabled = false;
        
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
        UpdateAnimation();
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
        
        if (moveInput.x == 0) { return; }



        if (moveInput.y <= -0.25 || moveInput.y >= 0.25)
        {
            spritePlayer.flipX = true;
            return;
        }
        
        spritePlayer.flipX = moveInput.x < 0;
        
        /*
        if (moveInput.x < 0)
        {
            transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        }
        else
        {
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }
        
        */

        
        
    }
    
    private void Collect(InputAction.CallbackContext _)
    {
        if (canInteractKettle)
        {
            if (carryedIngredient != null)
            {
                kettle.AddToKettle(carryedIngredient);
                CarryIngredient(carryedIngredient, false);
                carryedIngredient = null;
                
                Debug.Log("Ingredient delivered!");
            }
        }
        
        if (actualIngredient != null)
        {
            actualIngredient.Collected();
            carryedIngredient = actualIngredient;
            CarryIngredient(carryedIngredient, true);
            
            Debug.Log("Ingredient collected!");
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
        if (canInteractKettle)
        {
            Debug.Log("Started Brewing");
            kettle.CheckOrder();
            /*if (carryedIngredient != null)
            {
                kettle.AddToKettle(carryedIngredient);
                CarryIngredient(carryedIngredient, false);
                carryedIngredient = null;
                
            }*/
        }
        else
        {
            Debug.Log("Kettle not near by Player");
        }
        
    }
    
    #endregion


    private void CarryIngredient(Ingredient carriedIngredient, bool show)
    {
        carryIngredientSprite.enabled = show;
        carryIngredientSprite.sprite = carriedIngredient.GetComponent<SpriteRenderer>().sprite;
        carryIngredientSprite.color = carriedIngredient.GetComponent<SpriteRenderer>().color;
    }
    
    #region Triggers

    private void OnTriggerEnter(Collider other)
    {


        
    }

    public void SetSelectedIngredient(Ingredient ingredient)
    {
        actualIngredient = ingredient;
    }


    #region Animations

    private void UpdateAnimation()
    {
        Vector2 velocity = rbPlayer.velocity;
        animator.SetFloat(HorizontalSpeed, velocity.x);
        animator.SetFloat(VerticalSpeed, velocity.y);
    }

    #endregion

    private void OnTriggerEnter2D(Collider2D col)
    {
       if (col.CompareTag("Ingredient"))
        {
            actualIngredient = col.GetComponent<Ingredient>();
            Debug.Log("Actual_Ingredient_Selected");
        }

       if (col.CompareTag("Kettle"))
       {
           canInteractKettle = true;
           kettle.ShowBrewButton(true);
       }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Ingredient")&&actualIngredient == other.GetComponent<Ingredient>())
        {
            actualIngredient = null;
        }
        
        if (other.CompareTag("Kettle"))
        {
            canInteractKettle = false;
            kettle.ShowBrewButton(false);
        }
    }

    #endregion

    
}
