
using UnityEngine;

public class Table : MonoBehaviour
{

    [SerializeField] private GameObject itemPlace;

    //private Transform oldItemPlace;

    private Item itemTemporary;
    public void PlaceItem(Item item)
    {
        Debug.Log("Place_Item_Triggered");
        if (!itemPlace.GetComponentInChildren<Item>())
        {
            itemTemporary = Instantiate(item);
            //oldItemPlace = item.gameObject.GetComponentInParent<Transform>();
            itemTemporary.GetComponent<SpriteRenderer>().enabled = true;
            itemTemporary.transform.SetParent(itemPlace.transform, false);
        }
        
        //item.transform.SetParent(itemPlace.transform, false);
    }

    public void DeleteItem() // Reset Transform to initial Place
    {
        if (itemPlace.GetComponentInChildren<Item>())
        {
            //itemTemporary.transform.SetParent(oldItemPlace.transform, false);
            //Destroy(itemPlace.GetComponentInChildren<Item>().gameObject);
            Destroy(itemTemporary.gameObject);
        }
    }

}
