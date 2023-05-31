using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombinationManager : MonoBehaviour
{

    [SerializeField] private List<Order> possibleOrders;
    

    public Potion CheckPotionCombination(List<IngredientType> ingredientTypes)
    {
        foreach (Order possibleOrder in possibleOrders)
        {
            List<IngredientType> neededIngredients = possibleOrder.GetNeededIngredientTypes();
            if (neededIngredients.Count != ingredientTypes.Count)
            {
                goto NextOrder; // Wäre auch continue möglich..
            }
            foreach (IngredientType ingredientType in neededIngredients)
            {
                if (!ingredientTypes.Contains(ingredientType))
                {
                    goto NextOrder;
                }
            }

            return possibleOrder.GetPotion();
            NextOrder: ;
        }

        return null;
    }
    
}
