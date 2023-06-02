using System;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public int progressPoints = 0;

    private MilestoneController milestoneController;
    private MenuController menuController;
    private PlayerController player;


    private void Awake()
    {
        milestoneController = FindObjectOfType<MilestoneController>();
        milestoneController.SetMileStoneProgress(progressPoints);
        menuController = FindObjectOfType<MenuController>();
        player = FindObjectOfType<PlayerController>();
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
    public void RestartGame()
    {
        //TODO Restart mechanic
    }
    
    public void GameOver()
    {
        Debug.Log("Game-Over");
        //TODO Show Game-Over UI with RESTART Button
    }

    public void GameWon()
    {
        Debug.Log("You Won!");
        //TODO Show Game-Won UI with MAIN-MENU Button
    }
    
}
