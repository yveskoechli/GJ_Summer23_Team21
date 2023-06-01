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
    [SerializeField] private float timeToFulfill = 10f;
    [SerializeField] private float timeToStayGoodPotion = 4f;
    
    [SerializeField] private Image fillAmountImage;
    private float timeLeft = 0;
    private bool canCountDown = true;
    
    private OrderSpawner orderSpawner;
    private CombinationManager combinationManager;
    
    
    //Animations
    private Animator animator;
    
    private int maxListLength = 3;

    [SerializeField] private Potion brewedPotion; // Serialize Field only for Debug to see in Inspector

    private bool isBrewing;

    private void Awake()
    {
        ingredients = new List<Ingredient>();

        timeLeft = timeToFulfill + timeToStayGoodPotion;
        isBrewing = false;
        
        orderSpawner = FindObjectOfType<OrderSpawner>();
        combinationManager = FindObjectOfType<CombinationManager>();
        animator = GetComponent<Animator>();
        
        ShowBrewButton(false);

        ClearKettle();
    }

    private void Update()
    {
        if (isBrewing)
        {
            //CheckBrewState();
            CountDown();
        }
    }

    private void CountDown()
    {
        timeLeft -= Time.deltaTime;
        float fillAmountNormalized = 1 - (1 / (timeToFulfill) * timeLeft);
        fillAmountImage.fillAmount = fillAmountNormalized;
        //fillAmountImage.color = Color.Lerp(Color.green, Color.red, fillAmountNormalized);
        
        if (timeLeft <= 0)
        {
            Debug.Log("You was to slow...");
            canCountDown = false;
            //orderSpawner.RemoveFromOrderList(this);
            //Destroy(this.gameObject, 0.1f);
            //StartCoroutine(DestroyDelayed(0f));
        }

        //if (timeLeft <= )
        //{
            
        //}
        
    }

    private void CheckBrewState()
    {
        if (timeLeft < 0)
        {
            
        }
        if (brewedPotion != null)
        {
            isBrewing = false;
        }

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


    public void BrewPotion()
    {
        if (isBrewing) { return; }
        isBrewing = true;
        fillAmountImage.color = Color.green;
        Potion potion = combinationManager.CheckPotionCombination(ingredients.Select(ingredient => ingredient.GetIngredientType()).ToList());
        if (potion == null)
        {
            Debug.Log("Wrong Combination"); //TODO Trigger Wrong Combination Event
            //TODO Turn Ingredients to black -> That Player needs to Clear Kettle first before adding new Ingr.
            return;
        }
        else
        {
            brewedPotion = potion;
            animator.SetBool(IsCooking, true);
            //ClearKettle();
        }
        //orderSpawner.CheckOrders(); //TODO needs to be checked in Delivery-Area

    }
    
    
    public Potion GetBrewedPotion()     // Brewed Potion can be "Wrong Potion" -> Implement Check in Playercontroller
    {
        Potion potion = brewedPotion;
        brewedPotion = null;
        return potion;
    }
    
    public void ClearKettle()
    {
        ingredients.Clear();
        ClearIngredients();
        Debug.Log("Kettle cleared!");
    }

    private void ClearIngredients()
    {
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

    private IEnumerator WaitForCanStart(float time)
    {
        yield return new WaitForSeconds(time);
        
    }

}
