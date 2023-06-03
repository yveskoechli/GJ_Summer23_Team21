using System;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    private static readonly int HorizontalSpeed = Animator.StringToHash("horizontalSpeed");
    private static readonly int VerticalSpeed = Animator.StringToHash("verticalSpeed");
    private static readonly int LookDirection = Animator.StringToHash("lookDirection");
    private static readonly int IsCarrying = Animator.StringToHash("isCarrying");
    
    #region Inspector

    [SerializeField] private float moveSpeed = 10f;

    [SerializeField] private SpriteRenderer carrySprite;
    
    //[SerializeField] private float jumpForce = 15f;

    //[SerializeField] private LayerMask groundLayer;

    #endregion

    private GameController gamecontroller;
    private KettleController kettle;
    private DeliveryController deliveryController;
    private IngredientController ingredientController;
    
    private GameInput input;
    private InputAction moveAction;

    private Rigidbody2D rbPlayer;
    private SpriteRenderer spritePlayer;
    
    private Vector2 moveInput;

    
    // Animations:
    private Animator animator;
    
    private CircleCollider2D interactionArea;

    [SerializeField] private Ingredient actualIngredient;
    private GameObject actualIngredientGameobject; // TESTING: To destroy this Gameobject when took from a Table
    [SerializeField] private Item carryedItem;

    private bool canInteractKettle = false;
    private bool canInteractDelivery = false;
    private bool canInteractTable = false;
    private bool canInteractPrepare = false;
    private Table actualTable;
    private TablePrepare actualPrepare;
    

    //Sounds
    [SerializeField] private StudioEventEmitter stepSound;
    [SerializeField] private StudioEventEmitter throwSound;
    [SerializeField] private StudioEventEmitter pickupItemSound;
    [SerializeField] private StudioEventEmitter placeItemSound;
    [SerializeField] private StudioEventEmitter pickupPotionSound;
    [SerializeField] private StudioEventEmitter deliverPotionSound;
    
    private bool IsCarryingPotion => carryedItem is Potion;
    private bool IsCarryingIngredient => carryedItem is Ingredient;
    private bool IsCarryingNull => carryedItem == null;


    #region Unity Event Functions
    private void Awake()
    {
        gamecontroller = FindObjectOfType<GameController>();
        kettle = FindObjectOfType<KettleController>();
        deliveryController = FindObjectOfType<DeliveryController>();
        ingredientController = FindObjectOfType<IngredientController>();
        
        input = new GameInput();
        rbPlayer = GetComponent<Rigidbody2D>();
        spritePlayer = GetComponent<SpriteRenderer>();

        animator = GetComponent<Animator>();

        carrySprite.enabled = false;
        
        interactionArea = GetComponentInChildren<CircleCollider2D>();

        moveAction = input.Player.Move;
        
        input.Player.Collect.performed += Collect;
        input.Player.Interact.performed += Interact;
        input.Player.ClearKettle.performed += ClearKettle;
        
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
        input.Player.ClearKettle.performed -= ClearKettle;
        
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
        if (canInteractPrepare && IsCarryingIngredient)     // Prepare Ingredient
        {
            if (actualPrepare.PreCheckIngredient((Ingredient)carryedItem))
            {
                actualPrepare.PrepareIngredient((Ingredient)carryedItem);
                carryedItem = null;
                ShowCarryItem(null, false);
            }
        }
        
        if (!IsCarryingNull && canInteractTable)    // Place Item
        {
            if (actualTable.IsEmpty())
            {
                PlaceItem();
                placeItemSound.Play();
            }
            
        }
        
        if (IsCarryingPotion && canInteractDelivery)    // Deliver Item
        {
            deliveryController.DeliverOrder((Potion)carryedItem);
            ShowCarryItem(null, false);
            carryedItem = null;
            deliverPotionSound.Play();
            return;
        }
        
        if (canInteractKettle)      // If in Interaction Area from Kettle
        {
            if (IsCarryingIngredient  && !kettle.IsBrewing()&& !kettle.IsKettleFull())
            {
                kettle.AddToKettle((Ingredient)carryedItem);
                ShowCarryItem(null, false);
                carryedItem = null;
                throwSound.Play();
                
                Debug.Log("Ingredient delivered!");
            }
            else if (IsCarryingNull)
            {
                carryedItem = kettle.GetBrewedPotion();
                if (IsCarryingPotion)
                {
                    ShowCarryItem(carryedItem, true);
                    pickupPotionSound.Play();
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
            
            //carryedItem = actualIngredient;
            carryedItem = ingredientController.GetBaseIngredient(actualIngredient);
            ShowCarryItem(carryedItem, true);
            pickupItemSound.Play();
            
            if (canInteractTable)
            {
                carryedItem = FindItemInScene(carryedItem);
                actualTable.DeleteItem();
                
                //Destroy(actualIngredientGameobject);
            }
            
            Debug.Log("Ingredient collected!");
        }
        else
        {
            Debug.Log("No Ingredient triggered");
        }



    }

    private Item FindItemInScene(Item item)
    {
        Item baseIngredient = null;
        if (IsCarryingIngredient)
        {
            baseIngredient = ingredientController.GetBaseIngredient((Ingredient)item);
            
        }

        return baseIngredient;
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

    private void ClearKettle(InputAction.CallbackContext _)
    {
        if (canInteractKettle)
        {
            kettle.ClearKettle();
        }
    }

    #endregion


    private void ShowCarryItem(Item carriedItem, bool show)
    {
        carrySprite.enabled = show;
        if(show)
        {
            carrySprite.sprite = carriedItem.GetComponent<SpriteRenderer>().sprite;
            carrySprite.color = carriedItem.GetComponent<SpriteRenderer>().color; // Maybe not needed (Was just for Testing with Colors??)
        }
    }

    private void PlaceItem()
    {
        // TODO Remove Item from Player and Add Item as Child-Object to Table Item-Place
        actualTable.PlaceItem(carryedItem);
        ShowCarryItem(null, false);
        carryedItem = null;
    }

    #region Public Functions

    public void ChangeIngredient(IngredientType newIngredientType)
    {
        carryedItem = ingredientController.GetBaseIngredient(newIngredientType);
        ShowCarryItem(carryedItem, true);
    }
    
    public void EnableMoveInput(bool enable)
    {
        
    }
    public void SetSelectedIngredient(Ingredient ingredient)
    {
        actualIngredient = ingredient;
    }
    
    #endregion

    #region Animations

    private void UpdateAnimation()
    {
        Vector2 velocity = rbPlayer.velocity;
        animator.SetFloat(HorizontalSpeed, velocity.x);
        animator.SetFloat(VerticalSpeed, velocity.y);
        
        animator.SetBool(IsCarrying, !IsCarryingNull);
        
        //Debug.Log("Veloctiy x: " + velocity.x + " Velocity y: " + velocity.y);
        
        if (velocity.x == 0 && velocity.y == 0) { return; }

        if (velocity.y <= -8) // Down
        {
            spritePlayer.flipX = true;
            carrySprite.transform.localPosition = new Vector3(0f, 0f, 0f);
            //carrySprite.sortingOrder = 11;
            carrySprite.sortingLayerName = "Foreground";
            animator.SetFloat(LookDirection, 0f);
            return;
        }

        //carrySprite.sortingOrder = 9;
        carrySprite.sortingLayerName = "Midground";
        if (velocity.y >= 8) // Up
        {
            spritePlayer.flipX = false;
            carrySprite.transform.localPosition = new Vector3(0f, 2.5f, 0f);
            animator.SetFloat(LookDirection, 0.5f);
            
            return;
        }

        if (velocity.x < 0)    // Left
        {   
            spritePlayer.flipX = true;
            carrySprite.transform.localPosition = new Vector3(-1.18f, 0.682f, 0f);
            animator.SetFloat(LookDirection, 1f);
            return;
        }
        // Right
        spritePlayer.flipX = false;
        carrySprite.transform.localPosition = new Vector3(1.18f, 0.682f, 0f);
        animator.SetFloat(LookDirection, 1f);
    }

    #endregion

    #region Sounds

    public void FMOD_PlayStepsound()
    {
        Debug.Log("Stepsound Played");
        stepSound.Play();
    }

    #endregion

    #region OnTriggerEnters
    private void OnTriggerEnter2D(Collider2D col)
    {
       if (col.CompareTag("Ingredient"))
       {
            actualIngredient = col.GetComponent<Ingredient>();
            //actualIngredientGameobject = col.gameObject;
            Debug.Log("Actual_Ingredient_Selected");
       }

       if (col.CompareTag("Kettle"))
       {
           canInteractKettle = true;
           kettle.ShowTutorialText(true);
       }
       
       if (col.CompareTag("DeliveryController"))
       {
           canInteractDelivery = true;
       }
       if (col.CompareTag("Table"))
       {
           actualTable = col.GetComponent<Table>();
           canInteractTable = true;
       }
       if (col.CompareTag("Prepare"))
       {
           actualPrepare = col.GetComponent<TablePrepare>();
           canInteractPrepare = true;
       }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Ingredient")&&actualIngredient == other.GetComponent<Ingredient>())
        {
            actualIngredient = null;
            actualIngredientGameobject = null;
        }
        
        if (other.CompareTag("Kettle"))
        {
            canInteractKettle = false;
            kettle.ShowTutorialText(false);
        }
        if (other.CompareTag("DeliveryController"))
        {
            canInteractDelivery = false;
        }
        if (other.CompareTag("Table"))
        {
            actualTable = null;
            canInteractTable = false;
        }

        if (other.CompareTag("Prepare"))
        {
            actualPrepare = null;
            canInteractPrepare = false;
        }
        
    }

    #endregion

    
}
