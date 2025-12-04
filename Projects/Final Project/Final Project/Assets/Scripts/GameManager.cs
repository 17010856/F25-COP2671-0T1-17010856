using UnityEngine;
using UnityEngine.SceneManagement;

// handles game start, restart, and main game state
public class GameManager : MonoBehaviour
{
    public GameObject startScreenPanel;
    public GameObject gameUI;

    void Start()
    {
        ShowStartScreen();
    }

    public void ShowStartScreen()
    {
        if (startScreenPanel != null)
        {
            startScreenPanel.SetActive(true);
        }

        if (gameUI != null)
        {
            gameUI.SetActive(false);
        }

        Time.timeScale = 0f; // pause game
    }

    public void StartGame()
    {
        if (startScreenPanel != null)
        {
            startScreenPanel.SetActive(false);
        }

        if (gameUI != null)
        {
            gameUI.SetActive(true);
        }

        Time.timeScale = 1f; // unpause game
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // reload current scene
    }
}