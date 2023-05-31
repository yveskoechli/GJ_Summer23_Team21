using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    private static readonly int HorizontalSpeed = Animator.StringToHash("horizontalSpeed");
    private static readonly int VerticalSpeed = Animator.StringToHash("verticalSpeed");
    
    #region Inspector

    [SerializeField] private float moveSpeed = 10f;

    [SerializeField] private SpriteRenderer carrySprite;
    
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
    [SerializeField] private Item carryedItem;

    private bool canInteractKettle = false;

    private bool IsCarryingPotion => carryedItem is Potion;
    private bool IsCarryingIngredient => carryedItem is Ingredient;

    private bool IsCarryingNull => carryedItem == null;


    #region Unity Event Functions
    private void Awake()
    {
        gamecontroller = FindObjectOfType<GameController>();
        kettle = FindObjectOfType<KettleController>();
        
        input = new GameInput();
        rbPlayer = GetComponent<Rigidbody2D>();
        spritePlayer = GetComponent<SpriteRenderer>();

        animator = GetComponent<Animator>();

        carrySprite.enabled = false;
        
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
        
        
        //spritePlayer.flipX = moveInput.x < 0;
        
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
        if (canInteractKettle)      // If in Interaction Area from Kettle
        {
            if (IsCarryingIngredient)
            {
                kettle.AddToKettle((Ingredient)carryedItem);
                CarryItem(null, false);
                carryedItem = null;
                
                Debug.Log("Ingredient delivered!");
            }
            else if (IsCarryingNull)
            {
                carryedItem = kettle.GetBrewedPotion();
                if (IsCarryingPotion)
                {
                    CarryItem(carryedItem, true);
                }
                
            }
            return;
        }

        if (!IsCarryingNull)
        {
            return;
        }
        
        if (actualIngredient != null)
        {
            //actualIngredient.Collected();
            carryedItem = actualIngredient;
            CarryItem(carryedItem, true);
            
            Debug.Log("Ingredient collected!");
        }
        else
        {
            Debug.Log("No Ingredient triggered");
        }
        

    }

    private void Interact(InputAction.CallbackContext _)
    {
        if (canInteractKettle)
        {
            Debug.Log("Started Brewing");
            kettle.BrewPotion();
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


    private void CarryItem(Item carriedItem, bool show)
    {
        carrySprite.enabled = show;
        if(show)
        {
            carrySprite.sprite = carriedItem.GetComponent<SpriteRenderer>().sprite;
            carrySprite.color = carriedItem.GetComponent<SpriteRenderer>().color;
        }
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
        
        Debug.Log("Veloctiy x: " + velocity.x + " Velocity y: " + velocity.y);
        
        if (velocity.x == 0 && velocity.y == 0) { return; }

        if (velocity.y <= -8) // Down
        {
            spritePlayer.flipX = true;
            carrySprite.transform.localPosition = new Vector3(0f, 0f, 0f);
            carrySprite.sortingOrder = 11;
            return;
        }

        carrySprite.sortingOrder = 9;
        
        if (velocity.y >= 8) // Up
        {
            spritePlayer.flipX = false;
            carrySprite.transform.localPosition = new Vector3(0f, 2.5f, 0f);
            return;
        }

        if (velocity.x < 0)    // Left
        {   
            spritePlayer.flipX = true;
            carrySprite.transform.localPosition = new Vector3(-1.18f, 0.682f, 0f);
            return;
        }
        spritePlayer.flipX = false;
        carrySprite.transform.localPosition = new Vector3(1.18f, 0.682f, 0f);
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
