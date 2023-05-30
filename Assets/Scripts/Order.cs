using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Order : MonoBehaviour
{

    [SerializeField] private List<IngredientType> neededIngredients;

    [SerializeField] private float timeToFulfill = 10f;

    private float timeLeft = 0;

    private bool canCountDown = true;
    

    private void Awake()
    {
        timeLeft = timeToFulfill;
    }

    private void Update()
    {
        if (canCountDown)
        {
            CountDown();
        }
        
    }


    private void CountDown()
    {
        timeLeft -= Time.deltaTime;
        if (timeLeft <= 0)
        {
            Debug.Log("You was to slow...");
            canCountDown = false;
            
        }
        
    }
    
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
        StartCoroutine(DestroyDelayed(1f));
        return true;
    }


    private IEnumerator DestroyDelayed(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(this);
    }
}
