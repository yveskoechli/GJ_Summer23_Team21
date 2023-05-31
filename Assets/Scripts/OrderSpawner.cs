using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class OrderSpawner : MonoBehaviour
{
    private List<Order> orderList;

    [SerializeField] private List<GameObject> orderPrefabs;

    
    [SerializeField] private List<GameObject> actualOrders = new List<GameObject>(); // Just SerializeField to view in Inspector for Debugging

    private GameController gameController;
    
    private bool canStartNextOrder = false;
    
    
    private void Awake()
    {
        canStartNextOrder = true;
        gameController = FindObjectOfType<GameController>();
    }

    private void Update()
    {
        if (canStartNextOrder)
        {
            StartCoroutine(WaitForNextOrder(2f));
            canStartNextOrder = false;
        }
        
        
    }

    private void InstantiateOrder(int range)
    {
        if (actualOrders.Count >=5)
        {
            gameController.GameOver();
            return;
        }
        GameObject newOrder = Instantiate (orderPrefabs[Random.Range(0,orderPrefabs.Count)], Vector3.zero, Quaternion.identity) as GameObject;
        newOrder.transform.SetParent(this.transform, false);
        actualOrders.Insert(0, newOrder);
        canStartNextOrder = true;
    }

    public void CheckOrders() // Check all open Orders for a match -> If multiple matches, take match with lowest remaining time.
    {
        
    }
    
    private IEnumerator WaitForNextOrder(float time)
    {
        yield return new WaitForSeconds(time);
        InstantiateOrder(4);  // Range for more difficulty and variety in higher levels -> TODO Take Global Value for range from GameController
    }
    
}
