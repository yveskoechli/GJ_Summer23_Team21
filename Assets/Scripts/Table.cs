
using System.Collections;
using UnityEngine;

public class Table : MonoBehaviour
{

    [SerializeField] private GameObject itemPlace;

    private TextUIController textUIController;
    private SpriteRenderer selectSprite;

    private Item itemTemporary;
    
    private bool IsHavingItem => itemTemporary != null;

    private void Awake()
    {
        selectSprite = itemPlace.GetComponent<SpriteRenderer>();
        textUIController = FindObjectOfType<TextUIController>();
    }

    private void ShowTutorialText(bool show)
    {
        int tutorialIndex = IsHavingItem ? 5 : 4;
        if (show)
        {
            textUIController.ChangeTutorialText(tutorialIndex);
        }
        else
        {
            textUIController.HideTutorialText();
        }
    }
    
    public void PlaceItem(Item item)
    {
        Debug.Log("Place_Item_Triggered");
        if (!itemPlace.GetComponentInChildren<Item>())
        {
            itemTemporary = Instantiate(item);
            //oldItemPlace = item.gameObject.GetComponentInParent<Transform>();
            itemTemporary.GetComponent<SpriteRenderer>().enabled = true;
            itemTemporary.transform.SetParent(itemPlace.transform, false);
            ShowTutorialText(true);
        }
        
        //item.transform.SetParent(itemPlace.transform, false);
    }

    public void DeleteItem() // Reset Transform to initial Place
    {
        if (itemPlace.GetComponentInChildren<Item>())
        {
            Destroy(itemTemporary.gameObject);
            ShowTutorialText(false);
            StartCoroutine(ResetTutorialUIDelayed(0.1f));
        }
        
    }

    public bool IsEmpty()
    {
        return !itemPlace.GetComponentInChildren<Item>();
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
    
    private IEnumerator ResetTutorialUIDelayed(float time)
    {
        yield return null;
        //yield return new WaitForSeconds(time);
        ShowTutorialText(true);
        
    }
}
