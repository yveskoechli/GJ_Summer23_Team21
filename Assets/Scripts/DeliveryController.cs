
using System;
using UnityEngine;

public class DeliveryController : MonoBehaviour
{
    private GameController gameController;
    private OrderSpawner orderSpawner;

    private void Awake()
    {
        gameController = FindObjectOfType<GameController>();
        orderSpawner = FindObjectOfType<OrderSpawner>();


    }

    public void DeliverOrder(Potion potion)
    {
        Debug.Log("Potion delivered");
        // TODO Check Potion with open Orders -> Check in OrderSpawner
        gameController.AddProgressPoint();
    }
}
