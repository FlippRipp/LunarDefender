using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    [SerializeField] private string mainGameScene;
    
    public void StartGame()
    {
        SceneManager.LoadScene(mainGameScene);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
