
using System;
using UnityEngine;

public class DeliveryController : MonoBehaviour
{
    private GameController gameController;
    private OrderSpawner orderSpawner;
    private TextUIController textUIController;

    [SerializeField] private Animator animator;
    
    [SerializeField] private SpriteRenderer selectSprite;

    [SerializeField] private GameObject arrow;
    private void Awake()
    {
        gameController = FindObjectOfType<GameController>();
        orderSpawner = FindObjectOfType<OrderSpawner>();
        textUIController = FindObjectOfType<TextUIController>();
    }

    private void ShowTutorialText(bool show)
    {
        if (show)
        {
            textUIController.ChangeTutorialText(2);
        }
        else
        {
            textUIController.HideTutorialText();
        }
    }

    public void ShowArrow(bool show)
    {
        arrow.SetActive(show);
    }
    
    public void DeliverOrder(Potion potion)
    {
        Debug.Log("Potion delivered");
        animator.SetTrigger("sendPotion");
        // TODO Check Potion with open Orders -> Check in OrderSpawner
        orderSpawner.CheckOrders(potion.GetPotionType());
        //gameController.AddProgressPoint();
    }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if ( col.CompareTag("Player"))
        {
            selectSprite.enabled = true;
            ShowTutorialText(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if ( other.CompareTag("Player"))
        {
            selectSprite.enabled = false;
            ShowTutorialText(false);
        }
    }
}
