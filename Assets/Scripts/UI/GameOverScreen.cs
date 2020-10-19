using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour
{
    public float buttonSpawnWaitTime = 0.2f;
    public float buttonAlphaLerpSpeed = 10f;

    private Button[] gameOverScreenButtons;
    private CanvasGroup[] gameOverScreenButtonsCanvasGroups;

    private void Awake()
    {
        gameOverScreenButtons = GetComponentsInChildren<Button>();
        gameOverScreenButtonsCanvasGroups = new CanvasGroup[gameOverScreenButtons.Length];
        for (int i = 0; i < gameOverScreenButtonsCanvasGroups.Length; i++)
        {
            gameOverScreenButtonsCanvasGroups[i] = gameOverScreenButtons[i].GetComponent<CanvasGroup>();
        }
    }

    private void OnEnable()
    {
        InitButtons();
    }

    private void Update()
    {
        UpdateButtons();
    }

    private void InitButtons()
    {
        for (int i = 0; i < gameOverScreenButtons.Length; i++)
        {
            gameOverScreenButtons[i].gameObject.SetActive(false);
            gameOverScreenButtonsCanvasGroups[i].alpha = 0f;
        }

        StartCoroutine(DoEnableMenu());
    }

    private void UpdateButtons()
    {
        for (int i = 0; i < gameOverScreenButtons.Length; i++)
        {
            gameOverScreenButtonsCanvasGroups[i].alpha = Mathf.Lerp(gameOverScreenButtonsCanvasGroups[i].alpha, 1, Time.deltaTime * buttonAlphaLerpSpeed);
        }
    }

    private IEnumerator DoEnableMenu()
    {
        for (int i = 0; i < gameOverScreenButtons.Length; i++)
        {
            gameOverScreenButtons[i].gameObject.SetActive(true);
            yield return new WaitForSeconds(buttonSpawnWaitTime);
        }
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
