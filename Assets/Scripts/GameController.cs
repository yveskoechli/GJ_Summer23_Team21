using UnityEngine;

public class GameController : MonoBehaviour
{

    private int potionRange = 2; // 2 at beginning (easy) 5 at the End (hard) 3-4 in between

    public void RestartGame()
    {
        //TODO Restart mechanic
    }
    
    public void GameOver()
    {
        Debug.Log("Game-Over");
    }

    public void GameWon()
    {
        Debug.Log("You Won!");
    }
    
}
