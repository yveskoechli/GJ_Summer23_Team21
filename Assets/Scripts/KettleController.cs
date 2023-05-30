using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KettleController : MonoBehaviour
{

    [SerializeField] private List<Ingredient> ingredients;

    private int maxListLength = 3;

    private void Awake()
    {
        ingredients = new List<Ingredient>();
        
    }


    public void AddToKettle(Ingredient ingredient)
    {
        int listLength = ingredients.Count;
        if (listLength >= maxListLength) { return; }
        ingredients.Insert(listLength, ingredient);
        Debug.Log(ingredient.name + "added to Kettle");
    }
    
    
    public void EmptyKettle()
    {
        ingredients.Clear();
    }
}
