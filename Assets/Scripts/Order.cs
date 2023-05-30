using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Order : MonoBehaviour
{

    [SerializeField] private List<IngredientType> neededIngredients;


    public bool CheckIngredients(List<IngredientType> checkIngredients)
    {
        int i = 0;
        foreach (IngredientType ingredient in checkIngredients)
        {
            if (ingredient != neededIngredients[i])
            {
                Debug.Log("Wrong ingredient for Potion...");
                return false;
            }

            i++;
        }
        Debug.Log("Potion matched Order!");
        return true;
    }

}
