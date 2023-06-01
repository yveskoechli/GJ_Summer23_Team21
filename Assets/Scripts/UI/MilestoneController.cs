
using UnityEngine;

using UnityEngine.UI;

public class MilestoneController : MonoBehaviour
{
    [SerializeField] private Image milestoneFillImage;
    [SerializeField] private Image emoteFascinated;
    [SerializeField] private Image emoteSmile;
    [SerializeField] private Image emoteNeutral;

    private float highlightedAlpha = 1f;
    private float disabledAlpha = 0.4f;

    private Color highlightedColor = Color.white;
    private Color disabledColor = Color.white;

    private int milestoneProgress;
    
    private void Awake()
    {
        highlightedColor.a = highlightedAlpha;
        disabledColor.a = disabledAlpha;
        SetEmote(0);
        milestoneProgress = 0;
    }

    private void UpdateProgress()
    {
        var fillAmountNormalized = (1f / 10f) * milestoneProgress;
        if (fillAmountNormalized<=0.5)      //TODO Polish Progressbar in relation to MilestoneMeterPoints
        {
            fillAmountNormalized -= 0.05f;
        }
        milestoneFillImage.fillAmount = fillAmountNormalized;
        Debug.Log("Milestoneprogress: " + milestoneProgress);
        Debug.Log("fillAmountNormalized: " + fillAmountNormalized);
        SetEmote(milestoneProgress);
    }
    
    private void SetEmote(int progressAmount)
    {
        switch (progressAmount)
        {
            case 10:
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
    
    public void SetMileStoneProgress(int progress)
    {
        milestoneProgress = progress;
        UpdateProgress();
        Debug.Log("MilestoneController: SetMilestoneProgress");
    }
}
