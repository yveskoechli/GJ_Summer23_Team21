using System.Collections.Generic;
using FMODUnity;
using UnityEngine;


public class Item : MonoBehaviour
{
    [SerializeField] protected List<Sprite> sprites;
    [SerializeField] protected StudioEventEmitter pickUpSound;
    [SerializeField] protected StudioEventEmitter placeSound;
    
    public void PlayPickUpSound()
    {
        pickUpSound.Play();
    }
    
    public void PlayPlaceSound()
    {
        placeSound.Play();
    }
}