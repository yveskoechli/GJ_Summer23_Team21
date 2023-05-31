using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Order : MonoBehaviour
{

    [SerializeField] private List<IngredientType> neededIngredients;

    [SerializeField] private float timeToFulfill = 4f;

    [SerializeField] private Image fillAmountImage;

    private float timeLeft = 0;

    private bool canCountDown = true;

    private Color greenColor = new Color(109, 213, 110);
    private Color redColor = new Color(192, 59, 56);

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
        float fillAmountNormalized = 1 - (1 / timeToFulfill * timeLeft);
        fillAmountImage.fillAmount = fillAmountNormalized;
        fillAmountImage.color = Color.Lerp(Color.green, Color.red, fillAmountNormalized);
        
        if (timeLeft <= 0)
        {
            Debug.Log("You was to slow...");
            canCountDown = false;
            Destroy(this.gameObject);
            //StartCoroutine(DestroyDelayed(0f));
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
        Destroy(this.gameObject);
    }
}
