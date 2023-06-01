using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class KettleController : MonoBehaviour
{
    private static readonly int IsCooking = Animator.StringToHash("isCooking");

    [SerializeField] private List<SpriteRenderer> ingredientsSprites;

    [SerializeField] private List<Ingredient> ingredients;

    [SerializeField] private SpriteRenderer brewButtonImage;
    
    //FillBar
    [SerializeField] private float timeToFulfill = 6f; // Check in Inspector! (not automatically set..)
    [SerializeField] private float timeToStayGoodPotion = 4f; // Check in Inspector! (not automatically set..)
    
    [SerializeField] private Image fillAmountImage;
    [SerializeField] private Image checkmarkImageOK;
    [SerializeField] private Image checkmarkImageWarning;
    [SerializeField] private Image checkmarkImageNOK;
    [SerializeField] private GameObject fillAmountUI;
    
    
    private float timeLeft = 0;
    private bool canCountDown = true;

    
    
    private OrderSpawner orderSpawner;
    private CombinationManager combinationManager;
    
    
    //Animations
    private Animator animator;
    
    private int maxListLength = 3;

    [SerializeField] private Potion brewedPotion; // Serialize Field only for Debug to see in Inspector

    private bool isBrewing;
    private bool isPotionReady;

    private float CheckmarkOKTime = 2f;
    
    private void Awake()
    {
        ingredients = new List<Ingredient>();

        //timeLeft = timeToFulfill + timeToStayGoodPotion;
        timeLeft = timeToFulfill;
        isBrewing = false;
        isPotionReady = false;
        canCountDown = false;
        SetCheckMark(0);
        
        orderSpawner = FindObjectOfType<OrderSpawner>();
        combinationManager = FindObjectOfType<CombinationManager>();
        animator = GetComponent<Animator>();
        
        ShowBrewButton(false);
        EnableBrewStateUI(false);

        ClearKettle();
    }

    private void Update()
    {
        if (isBrewing)
        {
            //CheckBrewState();
            if (canCountDown)
            {
                CountDown();
            }
        }
    }

    private void CountDown()
    {
        timeLeft -= Time.deltaTime;
        if (timeLeft > 0)
        {
            CheckmarkOKTime = 2f;
            float fillAmountNormalized = 1 - (1 / (timeToFulfill) * (timeLeft));
            if (fillAmountNormalized >= 1) { fillAmountNormalized = 1;}
        
            fillAmountImage.fillAmount = fillAmountNormalized;
            return;
        }
        
        if (timeLeft <= 0-timeToStayGoodPotion)
        {
            SetCheckMark(3);
            Debug.Log("You was to slow...");
            canCountDown = false;
            isPotionReady = false;
            brewedPotion = null;
            //orderSpawner.RemoveFromOrderList(this);
            //Destroy(this.gameObject, 0.1f);
            //StartCoroutine(DestroyDelayed(0f));
        }
        else
        {
            isPotionReady = true;
            CheckmarkOKTime -= Time.deltaTime;
            if (CheckmarkOKTime>0f)
            {
                SetCheckMark(1);
            }
            else
            {
                SetCheckMark(2);
            }
            float timeToStayGoodNormalized = (1 / (timeToStayGoodPotion) * -timeLeft);
            fillAmountImage.color = Color.Lerp(Color.green, Color.red, timeToStayGoodNormalized);
        }

    }

    private void SetCheckMark(int brewState)
    {
        switch (brewState)
        {
            case 0:
                checkmarkImageOK.enabled = false;
                checkmarkImageWarning.enabled = false;
                checkmarkImageNOK.enabled = false;
                break;
            case 1:
                checkmarkImageOK.enabled = true;
                checkmarkImageWarning.enabled = false;
                checkmarkImageNOK.enabled = false;
                break;
            case 2:
                checkmarkImageOK.enabled = false;
                checkmarkImageWarning.enabled = true;
                checkmarkImageNOK.enabled = false;
                break;
            case 3:
                checkmarkImageOK.enabled = false;
                checkmarkImageWarning.enabled = false;
                checkmarkImageNOK.enabled = true;
                break;
            
        }
    }
    
    private void CheckBrewState()
    {
        if (isPotionReady)
        {
            
        }

    }

    private void EnableBrewStateUI(bool enable)
    {
        if (!enable)
        {
            SetCheckMark(0);
        }
        fillAmountUI.SetActive(enable);

    }
    

    public void AddToKettle(Ingredient ingredient)
    {
        Debug.Log("KettleAddTriggered");
        int listLength = ingredients.Count;
        if (listLength >= maxListLength)
        {
            //CheckOrder(); 
            return;
        }
        ingredients.Insert(listLength, ingredient);
        //ingredientTypes.Insert(listLength, ingredient.GetIngredientType());
        ingredientsSprites[listLength].sprite = ingredient.GetComponent<SpriteRenderer>().sprite;

        Debug.Log(ingredient.name + "added to Kettle");
    }


    public void BrewPotion()    // If all Ingredients ar in Kettle -> Start Brewing and Check for possible combination.
    {
        if (isBrewing) { return; }
        isBrewing = true;
        fillAmountImage.color = Color.green;
        Potion potion = combinationManager.CheckPotionCombination(ingredients.Select(ingredient => ingredient.GetIngredientType()).ToList());
        if (potion == null)
        {
            Debug.Log("Wrong Combination"); //TODO Trigger Wrong Combination Event
            //TODO Turn Ingredients to black -> That Player needs to Clear Kettle first before adding new Ingr.
            isBrewing = false;
            return;
        }
        else
        {       // TODO Maybe put this in a own Function like "ActivateKettle" (then also "DeactivateKettle" needed)
            EnableBrewStateUI(true);
            canCountDown = true;
            isPotionReady = false;
            brewedPotion = potion;
            animator.SetBool(IsCooking, true);
            //ClearKettle();
        }
        //orderSpawner.CheckOrders(); //TODO needs to be checked in Delivery-Area

    }
    
    
    public Potion GetBrewedPotion()     // Brewed Potion can be "Wrong Potion" -> Implement Check in Playercontroller
    {
        if (isPotionReady)
        {
            Potion potion = brewedPotion;
            animator.SetBool(IsCooking, false);
            ClearKettle();
            EnableBrewStateUI(false);
            return potion;
        }
        return null;
    }
    
    public void ClearKettle()
    {
        isBrewing = false;
        isPotionReady = false;
        brewedPotion = null;
        ClearIngredients();
        timeLeft = timeToFulfill;
        Debug.Log("Kettle cleared!");
    }

    private void ClearIngredients()
    {
        ingredients.Clear();
        foreach (var ingredientSprite in ingredientsSprites)
        {
            ingredientSprite.sprite = null;
        }
    }

    #region UI Elements

    public void ShowBrewButton(bool show)
    {
        var color = brewButtonImage.color;
        color.a = show ? 1 : 0;
        brewButtonImage.color = color;
    }

    #endregion

    private IEnumerator SetCheckMarkWarning(float time)
    {
        yield return new WaitForSeconds(time);
        
    }

}
