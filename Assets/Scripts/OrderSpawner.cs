using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class OrderSpawner : MonoBehaviour
{
    private List<Order> orderList;

    [SerializeField] private List<GameObject> orderPrefabs;

    [SerializeField] private List<GameObject> actualOrders = new List<GameObject>(); // Just SerializeField to view in Inspector for Debugging

    private GameController gameController;
    
    private bool canStartNextOrder = false;

    [SerializeField] private int maxOrders = 10;
    private int orderCount = 0;
    private int possibleOrdersAmount = 0;
    
    private void Awake()
    {
        canStartNextOrder = false;
        gameController = FindObjectOfType<GameController>();
        possibleOrdersAmount = orderPrefabs.Count;
        
        StartCoroutine(WaitForNextOrder(1f));
    }

    private void Update()
    {
        if (canStartNextOrder)
        {
            StartCoroutine(WaitForNextOrder(4f));
            canStartNextOrder = false;
        }
        
        
    }

    private void InstantiateOrder(int range)
    {
        if (orderCount >=maxOrders)
        {
            canStartNextOrder = false;
            Debug.Log("All Orders for this Level got in.");
            return;
        }
        if (actualOrders.Count >=possibleOrdersAmount)
        {
            //gameController.GameOver();
            canStartNextOrder = false;
            return;
        }
        GameObject newOrder = Instantiate (orderPrefabs[Random.Range(0,range)], Vector3.zero, Quaternion.identity) as GameObject;
        newOrder.transform.SetParent(this.transform, false);
        actualOrders.Insert(0, newOrder);
        orderCount++;
        canStartNextOrder = true;
    }

    public void CheckOrders(PotionType checkPotionType) // Check all open Orders for a match -> If multiple matches, take match with lowest remaining time.
    {
        
        GameObject matchedOrder = actualOrders.FirstOrDefault(go => go.GetComponent<Order>().GetPotionType() == checkPotionType);
        if (matchedOrder != null)
        {
            Debug.Log("Potion Matched!");
            gameController.AddProgressPoint();
            RemoveFromOrderList(matchedOrder.GetComponent<Order>());
        }
       
    }

    public void RemoveFromOrderList(Order order)
    {
        actualOrders.Remove(order.gameObject);
       Destroy(order.gameObject);
        //StartCoroutine(WaitForCanStart(4f));
    }
    
    
    private IEnumerator WaitForNextOrder(float time)
    {
        yield return new WaitForSeconds(time);
        InstantiateOrder(possibleOrdersAmount);  // Range for more difficulty and variety in higher levels -> TODO Take Global Value for range from GameController
    }

    private IEnumerator WaitForCanStart(float time)
    {
        yield return new WaitForSeconds(time);
        canStartNextOrder = true;
    }
    
}
