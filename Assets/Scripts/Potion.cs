using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : Item
{

    [SerializeField] private PotionType potionType;
    

    private void OnValidate()
    {
        GetComponent<SpriteRenderer>().sprite = sprites[(int)potionType];
    }

    
    public PotionType GetPotionType()
    {
        return potionType;
    }
}
