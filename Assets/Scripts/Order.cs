using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Order : MonoBehaviour
{

    [SerializeField] private Potion potion;
    
    [SerializeField] private List<IngredientType> neededIngredients;

    [SerializeField] private float timeToFulfill = 4f;

    [SerializeField] private Image fillAmountImage;

    private OrderSpawner orderSpawner;
    
    private float timeLeft = 0;

    private bool canCountDown = true;

    

    private void Awake()
    {
        timeLeft = timeToFulfill;
        orderSpawner = FindObjectOfType<OrderSpawner>();
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
            orderSpawner.RemoveFromOrderList(this);
            Destroy(this.gameObject, 0.1f);
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

    public List<IngredientType> GetNeededIngredientTypes()
    {
        return neededIngredients;
    }

    public Potion GetPotion()
    {
        return potion;
    }
    
    private IEnumerator DestroyDelayed(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(this.gameObject);
    }
}
