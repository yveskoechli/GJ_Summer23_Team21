
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using FMODUnity;

using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class KettleController : MonoBehaviour
{
    private static readonly int IsCooking = Animator.StringToHash("isCooking");
    private static readonly int IsOvercooking = Animator.StringToHash("isOvercooking");

    [SerializeField] private List<SpriteRenderer> ingredientsSprites;

    [SerializeField] private SpriteRenderer selectSprite;
    
    [SerializeField] private List<Ingredient> ingredients;

    //[SerializeField] private SpriteRenderer brewButtonImage;
    
    //FillBar
    [SerializeField] private float timeToFulfill = 6f; // Check in Inspector! (not automatically set..)
    [SerializeField] private float timeToStayGoodPotion = 4f; // Check in Inspector! (not automatically set..)
    
    [SerializeField] private Image fillAmountImage;
    [SerializeField] private Image checkmarkImageOK;
    [SerializeField] private Image checkmarkImageWarningBG;
    [SerializeField] private Image checkmarkImageWarning;
    [SerializeField] private Image checkmarkImageNOK;
    [SerializeField] private GameObject fillAmountUI;
    
    
    private float timeLeft = 0;
    private bool canCountDown = true;

    private bool isPlayerNearby = false;
    
    private OrderSpawner orderSpawner;
    private CombinationManager combinationManager;
    private TextUIController textUIController;
    
    //Animations
    private Animator animator;
    [SerializeField] private Animator smokeAnimator;
    
    private int maxListLength = 3;

    [SerializeField] private Potion brewedPotion; // Serialize Field only for Debug to see in Inspector

    //Sounds
    [SerializeField] private StudioEventEmitter kettleVerkacktSound;
    [SerializeField] private StudioEventEmitter potionFinishSound;
    [SerializeField] private StudioEventEmitter blubberIdleSound;
    [SerializeField] private StudioEventEmitter brewStartSound;
    [SerializeField] private StudioEventEmitter clearSound;

    private bool playSoundOnce = true;
    
    private bool isBrewing;
    private bool isPotionReady;
    private bool isOvercooked;
    

    private float CheckmarkOKTime = 1.5f;

    private bool checkMarkFillOnce = true;
    
    private void Awake()
    {
        ingredients = new List<Ingredient>();

        //timeLeft = timeToFulfill + timeToStayGoodPotion;
        timeLeft = timeToFulfill;
        isBrewing = false;
        isPotionReady = false;
        canCountDown = false;
        isOvercooked = false;
        brewedPotion = null;
        SetCheckMark(0);

        orderSpawner = FindObjectOfType<OrderSpawner>();
        combinationManager = FindObjectOfType<CombinationManager>();
        animator = GetComponent<Animator>();
        textUIController = FindObjectOfType<TextUIController>();
        
        ShowTutorialText(false);
        EnableBrewStateUI(false);

        //ClearKettle();
        ClearIngredients();
        
        blubberIdleSound.Play();
    }

    private void OnDisable()
    {
        blubberIdleSound.Stop();
    }

    private void Update()
    {
        if (isBrewing)
        {
            //CheckBrewState();
            if (canCountDown)
            {
                CountDown();
            }
        }
    }

    private void CountDown()
    {
        timeLeft -= Time.deltaTime;
        if (timeLeft > 0)
        {
            CheckmarkOKTime = 1.5f;
            float fillAmountNormalized = 1 - (1 / (timeToFulfill) * (timeLeft));
            if (fillAmountNormalized >= 1) { fillAmountNormalized = 1;}
        
            fillAmountImage.fillAmount = fillAmountNormalized;
            return;
        }
        
        if (timeLeft <= 0-timeToStayGoodPotion)
        {
            SetCheckMark(3);
            Debug.Log("You was to slow...");
            OverCooked();
            canCountDown = false;
            isPotionReady = false;
            brewedPotion = null;
            animator.SetBool(IsCooking, false);
            checkMarkFillOnce = true;

        }
        else
        {
            isPotionReady = true;
            CheckmarkOKTime -= Time.deltaTime;
            if (CheckmarkOKTime>0f)
            {
                SetCheckMark(1);
                PlayOnce();
            }
            else
            {
                SetCheckMark(2);
                if (checkMarkFillOnce)
                {
                    checkMarkFillOnce = false;
                    checkmarkImageWarning.fillAmount = 0;
                    DOTween.To(() => checkmarkImageWarning.fillAmount, x => checkmarkImageWarning.fillAmount = x, 1f, 3);
                }
                
            }
            float timeToStayGoodNormalized = (1 / (timeToStayGoodPotion) * -timeLeft);
            fillAmountImage.color = Color.Lerp(Color.green, Color.red, timeToStayGoodNormalized);
        }

    }

    private void PlayOnce()
    {
        if (playSoundOnce)
        {
            potionFinishSound.Play();
            playSoundOnce = false;
            if (isPlayerNearby)
            {
                ShowTutorialText(true);
            }
            
        }
    }

    private void SetCheckMark(int brewState)
    {
        switch (brewState)
        {
            case 0:
                checkmarkImageOK.enabled = false;
                checkmarkImageWarningBG.enabled = false;
                checkmarkImageWarning.enabled = false;
                checkmarkImageNOK.enabled = false;
                break;
            case 1:
                checkmarkImageOK.enabled = true;
                checkmarkImageWarningBG.enabled = false;
                checkmarkImageWarning.enabled = false;
                checkmarkImageNOK.enabled = false;
                break;
            case 2:
                checkmarkImageOK.enabled = false;
                checkmarkImageWarningBG.enabled = true;
                checkmarkImageWarning.enabled = true;
                checkmarkImageNOK.enabled = false;
                break;
            case 3:
                checkmarkImageOK.enabled = false;
                checkmarkImageWarningBG.enabled = false;
                checkmarkImageWarning.enabled = false;
                checkmarkImageNOK.enabled = true;
                break;
            
        }
    }
    
    private void CheckBrewState()
    {
        if (isPotionReady)
        {
            
        }

    }

    private void EnableBrewStateUI(bool enable)
    {
        if (!enable)
        {
            SetCheckMark(0);
        }
        fillAmountUI.SetActive(enable);

    }
    

    public void AddToKettle(Ingredient ingredient)
    {
        if (isBrewing) { return; }
        if (isOvercooked) { return; }
        //Debug.Log("KettleAddTriggered");
        int listLength = ingredients.Count;
        if (listLength >= maxListLength) { return; }
        
        ingredients.Insert(listLength, ingredient);
        //ingredientTypes.Insert(listLength, ingredient.GetIngredientType());
        ingredientsSprites[listLength].sprite = ingredient.GetComponent<SpriteRenderer>().sprite;

        Debug.Log(ingredient.name + "added to Kettle");
    }

    public bool IsKettleFull()
    {
        int listLength = ingredients.Count;
        return listLength >= maxListLength;
        
    }

    public void BrewPotion()    // If all Ingredients ar in Kettle -> Start Brewing and Check for possible combination.
    {
        if (isBrewing) { return; }
        if (isOvercooked) { return; }
        
        isBrewing = true;
        fillAmountImage.color = Color.green;
        Potion potion = combinationManager.CheckPotionCombination(ingredients.Select(ingredient => ingredient.GetIngredientType()).ToList());
        if (potion == null)
        {
            Debug.Log("Wrong Combination"); //TODO Trigger Wrong Combination Event
            OverCooked();
            //TODO Turn Ingredients to black -> That Player needs to Clear Kettle first before adding new Ingr.
            isBrewing = false;
            //return;
        }
        else
        {       // TODO Maybe put this in a own Function like "ActivateKettle" (then also "DeactivateKettle" needed)
            EnableBrewStateUI(true);
            canCountDown = true;
            isPotionReady = false;
            brewedPotion = potion;
            animator.SetBool(IsCooking, true);
            brewStartSound.Play();
            //ClearKettle();
        }
        //orderSpawner.CheckOrders(); //TODO needs to be checked in Delivery-Area

    }

    public bool IsBrewing()
    {
        return isBrewing;
    }
    
    public Potion GetBrewedPotion()     // Brewed Potion can be "Wrong Potion" -> Implement Check in Playercontroller
    {
        if (isPotionReady)
        {
            Potion potion = brewedPotion;
            animator.SetBool(IsCooking, false);
            ClearKettle();
            EnableBrewStateUI(false);
            return potion;
        }
        return null;
    }
    
    public void ClearKettle()
    {
        isOvercooked = false;
        isBrewing = false;
        isPotionReady = false;
        brewedPotion = null;
        ClearIngredients();
        timeLeft = timeToFulfill;
        EnableBrewStateUI(false);
        ColorIngredientSprites(false);
        animator.SetBool(IsCooking, false);
        playSoundOnce = true;
        clearSound.Play();
        Debug.Log("Kettle cleared!");
    }

    private void ClearIngredients()
    {
        ingredients.Clear();
        foreach (var ingredientSprite in ingredientsSprites)
        {
            ingredientSprite.sprite = null;
        }
        animator.SetBool(IsOvercooking, false);
    }

    private void OverCooked()
    {
        isOvercooked = true;
        ColorIngredientSprites(true);
        smokeAnimator.SetTrigger("overcooked");
        animator.SetBool(IsOvercooking, true);
        kettleVerkacktSound.Play();
    }

    private void ColorIngredientSprites(bool overcooked)
    {
        if (overcooked)
        {
            foreach (var ingredientsSprite in ingredientsSprites)
            {
                ingredientsSprite.color = Color.gray;
            }
        }
        else
        {
            foreach (var ingredientsSprite in ingredientsSprites)
            {
                ingredientsSprite.color = Color.white;
            }
        }
    }

    #region UI Elements

    public void ShowTutorialText(bool show)
    {
        if (show)
        {
            if (isPotionReady)
            {
                textUIController.ChangeTutorialText(5);
            }
            else
            {
                textUIController.ChangeTutorialText(0);
            }
            
        }
        else
        {
            textUIController.HideTutorialText();
        }
    }

    #endregion

    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if ( col.CompareTag("Player"))
        {
            selectSprite.enabled = true;
            isPlayerNearby = true;
            
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if ( other.CompareTag("Player"))
        {
            selectSprite.enabled = false;
            isPlayerNearby = false;

        }
    }
    private IEnumerator SetCheckMarkWarning(float time)
    {
        yield return new WaitForSeconds(time);
        
    }

}
