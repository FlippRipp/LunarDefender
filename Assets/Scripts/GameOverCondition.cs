using UnityEngine;
using UnityEngine.SceneManagement;

public static class GameState
{
    public static bool GameOver;
}


public class GameOverCondition : MonoBehaviour
{
    [SerializeField] GameOverScreen gameOverScreen;

    private void Start()
    {
        GameState.GameOver = false;
        EventManager.instance.OnStationDestroyed += EndGame;
    }

    private void EndGame(int stationID)
    {
        gameOverScreen.gameObject.SetActive(true);

        PlayerMovement player = FindObjectOfType<PlayerMovement>();
        if (player)
        {
            player.gameObject.SetActive(false);
        }

        GameState.GameOver = true;
    }
}

