using System;
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
    
    [SerializeField] private string nextLevel = "Main_Menu";
    
    private void Awake()
    {
        milestoneController = FindObjectOfType<MilestoneController>();
        milestoneController.SetMileStoneProgress(progressPoints);
        menuController = FindObjectOfType<MenuController>();
        player = FindObjectOfType<PlayerController>();
        
        
    }

    private void SaveEndProgress()
    {
        PlayerPrefs.SetInt(playerPrefProgress, progressPoints);
        PlayerPrefs.Save();
    }
    private void OnEnable()
    {
        MenuController.BaseMenuOpening += EnterPauseMode;
        MenuController.BaseMenuClosed += EnterPlayMode;
        
    }

    private void Start()
    {
        EnterPlayMode();
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
        Debug.Log("GameFinish Triggered");
        StartCoroutine(WaitForGameFinish(2f));
        //EnterPauseMode();
        //endscreenUI.SetActive(true);
        //DOShow();
    }
    public Tween DOShow()
    {
        this.DOKill();
        Sequence sequence = DOTween.Sequence(this)
            .Append(DOFade(1).From(0));
        return sequence;    
    }
    private TweenerCore<float, float, FloatOptions> DOFade(float targetAlpha)
    {
        return canvasGroupEndscreen.DOFade(targetAlpha, 0.75f).SetEase(Ease.InOutSine);
    }
    
    private IEnumerator WaitForGameFinish(float time)
    {
        yield return new WaitForSeconds(time);
        EnterPauseMode();
        endscreenUI.SetActive(true);
        canvasGroupEndscreen.alpha = 1;
        selectableEndscreenButton.Select();
        //DOShow();
    }
    
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    public void LoadNextLevel()
    {
        SceneManager.LoadScene(nextLevel);
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    
}
