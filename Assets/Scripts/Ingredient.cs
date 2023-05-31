using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingredient : MonoBehaviour
{


    [SerializeField] private IngredientType ingredientType;

    [SerializeField] private bool prepared = false;
    
    private void Awake()
    {
    }

    public void Collected()
    {
        Debug.Log(ingredientType.ToString() + " collected!");
        
    }

    public void Delivered()
    {
        Debug.Log(ingredientType.ToString() + " delivered");
    }

    public bool IsPrepared()
    {
        return prepared;
    }
    
   /* private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            col.GetComponent<PlayerController>().SetSelectedIngredient(this);
            Debug.Log("Set Ingredient to Player");
        }
    }*/
}
