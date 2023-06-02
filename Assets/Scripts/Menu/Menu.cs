using System.Collections;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    #region Inspector

    [Tooltip("Selectable to be selected when the menu is opened.")]
    [SerializeField] private Selectable selectOnOpen;

    [Tooltip("Remember the Selectable that was selected when the menu was opened an reselect it once when the menu is closed.")]
    [SerializeField] private bool selectPreviousOnClose = true;
    
    [Tooltip("Hide the menu when the game starts.")]
    [SerializeField] private bool disableOnAwake = true;

    #endregion

    private Selectable selectOnClose;

    #region Unity Event Functions

    private void Awake()
    {
        if (disableOnAwake)
        {
            gameObject.SetActive(false);
        }
        else
        {
            Open();
        }
    }

    #endregion
    
    public void Open(bool instant = false)
    {
        gameObject.SetActive(true);
        
        // Save the previous selection.
        if (selectPreviousOnClose)
        {
            GameObject previousSelection = EventSystem.current.currentSelectedGameObject;
            if (previousSelection != null)
            {
                selectOnClose = previousSelection.GetComponent<Selectable>();
            }
        }

        // Coroutine only necessary for select animation.
        // Select UI event is not called if it was enabled in the same frame.
        //StartCoroutine(DelayedSelect(selectOnOpen));
        
        Select(selectOnOpen);
        
        if (!instant)
        {
            // TODO DOTween animations
        }
        
    }

    public void Close()
    {
        if (selectPreviousOnClose && selectOnClose != null)
        {
            //selectOnClose.StartCoroutine(DelayedSelect(selectOnClose));
            selectOnClose.Select();
        }
        
        gameObject.SetActive(false);
        // TODO DOTween animations
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private IEnumerator DelayedSelect(Selectable newSelection)
    {
        yield return null;
        Select(newSelection);
    }

    private void Select(Selectable newSelection)
    {
        if (newSelection == null) { return; }
        newSelection.Select();
    }
    
}
