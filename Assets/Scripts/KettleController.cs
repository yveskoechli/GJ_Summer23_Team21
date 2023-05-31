using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class KettleController : MonoBehaviour
{

    [SerializeField] private List<SpriteRenderer> ingredientsSprites;

    [SerializeField] private List<Ingredient> ingredients;

    [SerializeField] private SpriteRenderer brewButtonImage;
    
    private OrderSpawner orderSpawner;
    private CombinationManager combinationManager;
    
    private int maxListLength = 3;

    [SerializeField] private Potion brewedPotion; // Serialize Field only for Debug to see in Inspector
    //[SerializeField] private List<IngredientType> ingredientTypes; // Serialize Field only for Debug to see in Inspector

    private void Awake()
    {
        ingredients = new List<Ingredient>();

        orderSpawner = FindObjectOfType<OrderSpawner>();
        combinationManager = FindObjectOfType<CombinationManager>();
        
        ShowBrewButton(false);

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
            ClearKettle();
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
        Debug.Log("Kettle cleared!");
    }


    #region UI Elements

    public void ShowBrewButton(bool show)
    {
        var color = brewButtonImage.color;
        color.a = show ? 1 : 0;
        brewButtonImage.color = color;
    }

    #endregion


}
