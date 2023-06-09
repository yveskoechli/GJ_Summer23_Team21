
using System.Collections;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public int progressPoints = 0;

    private MilestoneController milestoneController;
    private MenuController menuController;
    private PlayerController player;

    [SerializeField] private string playerPrefProgress = "Level1.Progress";

    [SerializeField] private CanvasGroup canvasGroupEndscreen;
    [SerializeField] private GameObject endscreenUI;
    [SerializeField] private Selectable selectableEndscreenButton;

    [SerializeField] private CanvasGroup fadeUI;
    
    [SerializeField] private string nextLevel = "Main_Menu";

    [SerializeField] private Image emoteFascinated;
    [SerializeField] private Image emoteSmile;
    [SerializeField] private Image emoteNeutral;
    
    private float highlightedAlpha = 1f;
    private float disabledAlpha = 0.3f;

    private Color highlightedColor = Color.white;
    private Color disabledColor = Color.white;
    private void Awake()
    {
        milestoneController = FindObjectOfType<MilestoneController>();
        milestoneController.SetMileStoneProgress(progressPoints);
        menuController = FindObjectOfType<MenuController>();
        player = FindObjectOfType<PlayerController>();
        
        highlightedColor.a = highlightedAlpha;
        disabledColor.a = disabledAlpha;
        fadeUI.alpha = 1;
        
    }

    private void SaveEndProgress()
    {
        
        PlayerPrefs.SetInt(playerPrefProgress, progressPoints);
        PlayerPrefs.Save();
        //StartCoroutine(WaitForEmote(0.1f));
        SetEmote(progressPoints);
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
    
    private void OnEnable()
    {
        MenuController.BaseMenuOpening += EnterPauseMode;
        MenuController.BaseMenuClosed += EnterPlayMode;
        
    }

    private void Start()
    {
        EnterPlayMode();
        DOTween.To(() => fadeUI.alpha, x => fadeUI.alpha = x, 0f, 1);
    }

    private void OnDisable()
    {
        MenuController.BaseMenuOpening -= EnterPauseMode;
        MenuController.BaseMenuClosed -= EnterPlayMode;
    }

    public void EnterPlayMode()
    {
        Time.timeScale = 1;
        // In the editor: Unlock with ESC.
        Cursor.lockState = CursorLockMode.Locked; // Macht auch gleichzeitig, dass der Cursor nicht mehr sichtbar ist -> Curosr.visible nicht nötig somit
        player.EnableInput();
        menuController.enabled = true;
    }
    
    public void EnterPauseMode()
    {
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        player.DisableInput();
        menuController.enabled = true;
    }
    
    public void AddProgressPoint()
    {
        progressPoints++;
        if (progressPoints>=10)
        {
            progressPoints = 10;
        }
        milestoneController.SetMileStoneProgress(progressPoints);
        Debug.Log("GameController: Added ProgressPoint");
    }

    public void RemoveProgressPoint()
    {
        progressPoints--;
        if (progressPoints<0)
        {
            progressPoints = 0;
        }
        milestoneController.SetMileStoneProgress(progressPoints);
    }
    

    public void GameFinish()
    {
        StartCoroutine(WaitForGameFinish(2f));

    }
    
    
    private IEnumerator WaitForGameFinish(float time)
    {
        yield return new WaitForSeconds(time);
        endscreenUI.SetActive(true);
        SaveEndProgress();
        selectableEndscreenButton.Select();
        canvasGroupEndscreen.alpha = 0f;
        //this.DOKill();
        DOTween.To(() => canvasGroupEndscreen.alpha, x => canvasGroupEndscreen.alpha = x, 1f, 1).OnComplete(EnterPauseMode);

    }
    
    private IEnumerator WaitForEmote(float time)
    {
        yield return new WaitForSeconds(time);
        SetEmote(progressPoints);
    }
    
    
    public void RestartLevel()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        fadeUI.alpha = 0f;
        DOTween.Sequence().Append(fadeUI.DOFade(1f,0.5f)).SetUpdate(true).OnComplete(() => { LoadScene(sceneName); });
    }

    
    
    public void LoadNextLevel()
    {
        DOTween.Sequence().Append(fadeUI.DOFade(1f,1f)).SetUpdate(true).OnComplete(() => { LoadScene(nextLevel); });
        //SceneManager.LoadScene(nextLevel);
    }

    public void BackToMenu()
    {
        DOTween.Sequence().Append(fadeUI.DOFade(1f,1f)).SetUpdate(true).OnComplete(() => { LoadScene("MainMenu"); });
        //SceneManager.LoadScene("MainMenu");
    }
    
    private TweenCallback LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        return null;
    }
    
    
}
