
using System.Collections.Generic;
using UnityEngine;

public class TextUIController : MonoBehaviour
{

    [SerializeField] private List<GameObject> tutorialTexts;

    private void Awake()
    {
        foreach (GameObject tutorialText in tutorialTexts)
        {
            tutorialText.SetActive(false);
        }
    }

    public void ChangeTutorialText(int textIndex)
    {
        HideTutorialText();
        tutorialTexts[textIndex].SetActive(true);
    }

    public void HideTutorialText()
    {
        foreach (GameObject tutorialText in tutorialTexts)
        {
            tutorialText.SetActive(false);
        }
    }

}
