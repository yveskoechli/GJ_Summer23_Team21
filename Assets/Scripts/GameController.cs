using System;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public int progressPoints = 0;

    private MilestoneController milestoneController;


    private void Awake()
    {
        milestoneController = FindObjectOfType<MilestoneController>();
        milestoneController.SetMileStoneProgress(progressPoints);
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
