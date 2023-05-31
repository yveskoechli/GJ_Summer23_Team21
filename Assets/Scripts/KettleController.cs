using System.Collections.Generic;
using UnityEngine;

public class KettleController : MonoBehaviour
{

    [SerializeField] private List<SpriteRenderer> ingredientsSprites;

    [SerializeField] private List<Ingredient> ingredients;

    [SerializeField] private SpriteRenderer brewButtonImage;
    
    private OrderSpawner orderSpawner;
    
    private int maxListLength = 3;

    private void Awake()
    {
        ingredients = new List<Ingredient>();

        orderSpawner = FindObjectOfType<OrderSpawner>();
        
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
        ingredientsSprites[listLength].sprite = ingredient.GetComponent<SpriteRenderer>().sprite;
        //ingredientsSprites[listLength].color = ingredient.GetComponent<SpriteRenderer>().color; // Color only for Prototype (can be deleted when adding real Sprites)
        Debug.Log(ingredient.name + "added to Kettle");
    }


    public void CheckOrder()
    {
        orderSpawner.CheckOrders();
    }
    
    public void ClearKettle()
    {
        ingredients.Clear();
        Debug.Log("Kettle cleared!");
    }

    public void ShowBrewButton(bool show)
    {
        var color = brewButtonImage.color;
        color.a = show ? 1 : 0;
        brewButtonImage.color = color;
    }
}
