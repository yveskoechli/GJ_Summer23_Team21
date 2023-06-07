
using UnityEngine;

public class Ingredient : Item
{
    
    [SerializeField] private IngredientType ingredientType;

    private void OnValidate()
    {
        GetComponent<SpriteRenderer>().sprite = sprites[(int)ingredientType];
    }
    

    public IngredientType GetIngredientType()
    {
        return ingredientType;
    }


    
}
