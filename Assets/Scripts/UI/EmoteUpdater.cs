using System;
using UnityEngine;
using UnityEngine.UI;

public class EmoteUpdater : MonoBehaviour
{
    [SerializeField] private Image emoteFascinated;
    [SerializeField] private Image emoteSmile;
    [SerializeField] private Image emoteNeutral;

    [SerializeField] private String playerPrefsLevel = "Level1.Progress";
    
    private float highlightedAlpha = 1f;
    private float disabledAlpha = 0.3f;

    private Color highlightedColor = Color.white;
    private Color disabledColor = Color.white;
    

    private void Awake()
    {
        highlightedColor.a = highlightedAlpha;
        disabledColor.a = disabledAlpha;
        SetEmote(PlayerPrefs.GetInt(playerPrefsLevel));
        
    }
    
    
    private void SetEmote(int progressAmount)
    {
        switch (progressAmount)
        {
            case >=10:
                emoteFascinated.color = highlightedColor;
                emoteSmile.color = disabledColor;
                emoteNeutral.color = disabledColor;
                break;
            case >= 7:
                emoteFascinated.color = disabledColor;
                emoteSmile.color = highlightedColor;
                emoteNeutral.color = disabledColor;
                break;
            case >= 4:
                emoteFascinated.color = disabledColor;
                emoteSmile.color = disabledColor;
                emoteNeutral.color = highlightedColor;
                break;
            default:
                emoteFascinated.color = disabledColor;
                emoteSmile.color = disabledColor;
                emoteNeutral.color = disabledColor;
                break;
        }
    }
}
