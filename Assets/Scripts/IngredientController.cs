
using System.Collections.Generic;
using System.Linq;
using System.Net;
using UnityEngine;

public class IngredientController : MonoBehaviour
{

    [SerializeField] private List<Ingredient> baseIngredients;
    [SerializeField] private List<Potion> basePotions;


    public Ingredient GetBaseIngredient(Ingredient playerIngredient)
    {
        Ingredient baseIngredient;

        foreach (Ingredient ingredient in baseIngredients)
        {
            if (playerIngredient.GetIngredientType() == ingredient.GetIngredientType())
            {
                return ingredient;
            }
            
        }

        return null;
    }

    
    public Ingredient GetBaseIngredient(IngredientType playerIngredientType)    // To give back Ingredient with IngredientType input..
    {
        Ingredient baseIngredient;

        foreach (Ingredient ingredient in baseIngredients)
        {
            if (playerIngredientType == ingredient.GetIngredientType())
            {
                return ingredient;
            }
            
        }

        return null;
    }
}
