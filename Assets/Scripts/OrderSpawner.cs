using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderSpawner : MonoBehaviour
{
    private List<Order> orderList;

    private void Awake()
    {
        orderList = new List<Order>(5);
    }

    private void Update()
    {
        
    }

    public void CheckOrders() // Check all open Orders for a match -> If multiple matches, take match with lowest remaining time.
    {
        
    }
}
