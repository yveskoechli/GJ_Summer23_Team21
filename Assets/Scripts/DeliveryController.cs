
using System;
using UnityEngine;

public class DeliveryController : MonoBehaviour
{
    private GameController gameController;
    private OrderSpawner orderSpawner;

    [SerializeField] private Animator animator;
    private void Awake()
    {
        gameController = FindObjectOfType<GameController>();
        orderSpawner = FindObjectOfType<OrderSpawner>();


    }

    public void DeliverOrder(Potion potion)
    {
        Debug.Log("Potion delivered");
        animator.SetTrigger("sendPotion");
        // TODO Check Potion with open Orders -> Check in OrderSpawner
        orderSpawner.CheckOrders(potion.GetPotionType());
        //gameController.AddProgressPoint();
    }
}
