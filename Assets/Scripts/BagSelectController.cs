using UnityEngine;

public class BagSelectController : MonoBehaviour
{
    private TextUIController textUIController;
    
    [SerializeField] private SpriteRenderer selectSprite;

    private void Awake()
    {
        textUIController = FindObjectOfType<TextUIController>();
    }
    private void ShowTutorialText(bool show)
    {
        if (show)
        {
            textUIController.ChangeTutorialText(5);
        }
        else
        {
            textUIController.HideTutorialText();
        }
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
