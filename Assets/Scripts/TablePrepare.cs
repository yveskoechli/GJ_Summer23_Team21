
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TablePrepare : MonoBehaviour
{

    [SerializeField] private SpriteRenderer selectSprite;

    [SerializeField] private Image fillAmountImage;
    
    [SerializeField] private GameObject fillAmountUI;

    [SerializeField] private PrepareType prepareType;
    
    private Item itemTemporary;

    private PlayerController player;
    private TextUIController textUIController;

    private bool isPreparing = false;
    private float timeToFulfill = 2f;
    private float timeLeft = 0;

    private Ingredient newIngredient;

    private void Awake()
    {
        player = FindObjectOfType<PlayerController>();
        textUIController = FindObjectOfType<TextUIController>();
        fillAmountImage.fillAmount = 0f;
        timeLeft = timeToFulfill;
        fillAmountImage.color = Color.green;
        
        fillAmountUI.SetActive(false);
    }

    private void Update()
    {
        if (isPreparing)
        {
            CountDown();
        }
    }

    private void CountDown()
    {
        timeLeft -= Time.deltaTime;
        if (timeLeft > 0)
        {
            float fillAmountNormalized = 1 - (1 / (timeToFulfill) * (timeLeft));
            if (fillAmountNormalized >= 1)
            {
                fillAmountNormalized = 1;
            }

            fillAmountImage.fillAmount = fillAmountNormalized;
            
        }
        else
        {
            isPreparing = false;
            player.EnableInput();
            EnableBrewStateUI(false);
            player.ChangeIngredient(GiveBackNewIngredient(newIngredient));
            timeLeft = timeToFulfill;
        }
    }

    private void EnableBrewStateUI(bool enable)
    {
        fillAmountUI.SetActive(enable);

    }

    private void CheckIngredientToPrepare(Ingredient ingredient)
    {
        if (prepareType == PrepareType.Cutter)
        {
            if (ingredient.GetIngredientType() == IngredientType.DarkHerb || ingredient.GetIngredientType() == IngredientType.CrowFeet)
            {
                player.DisableInput();
                isPreparing = true;
                EnableBrewStateUI(true);
                newIngredient = ingredient;
                //player.ChangeIngredient(GiveBackNewIngredient(ingredient));
            }
        }
        else if (ingredient.GetIngredientType() == IngredientType.Stars)
        {
            player.DisableInput();
            isPreparing = true;
            EnableBrewStateUI(true);
            newIngredient = ingredient;
            
        }
        else
        {
            Debug.Log("Wrong Ingredient for this station");
        }
    }

    private IngredientType GiveBackNewIngredient(Ingredient ingredient)
    {
        IngredientType ingredientType = ingredient.GetIngredientType();
        switch (ingredientType)
        {
            case IngredientType.Stars:
                return IngredientType.StarDust;
            case IngredientType.DarkHerb:
                return IngredientType.DarkHerbCutted;
            case IngredientType.CrowFeet:
                return IngredientType.CrowFeetSliced;
        }
        
        Debug.Log("Wrong IngredientType in TablePrepare!"); // Should never appear in final game
        return IngredientType.Stars;
    }
    
    private void ShowTutorialText(bool show)
    {
        int index;
        index = prepareType == PrepareType.Cutter ? 1 : 3;

        if (show)
        {
            textUIController.ChangeTutorialText(index);
        }
        else
        {
            textUIController.HideTutorialText();
        }
    }
    
    public void PrepareIngredient(Ingredient ingredient)
    {
        CheckIngredientToPrepare(ingredient);
        Debug.Log("Prepare_Item_Triggered");
        
    }
    

    private void OnTriggerEnter2D(Collider2D col)
    {
        if ( col.CompareTag("Player"))
        {
            selectSprite.enabled = true;
            ShowTutorialText(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if ( other.CompareTag("Player"))
        {
            selectSprite.enabled = false;
            ShowTutorialText(false);
        }
    }
    
}
