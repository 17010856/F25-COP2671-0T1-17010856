using UnityEngine;
using TMPro;

public class GameOverUI : MonoBehaviour
{
    public GameObject gameOverPanel;
    public TextMeshProUGUI scoreText;
    public TimeManager timeManager;
    public CurrencyManager currencyManager;
    public InventorySystem inventorySystem;

    void Start()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        if (timeManager != null)
        {
            timeManager.onGameOver += ShowGameOver;
        }
    }

    void OnDestroy()
    {
        if (timeManager != null)
        {
            timeManager.onGameOver -= ShowGameOver;
        }
    }

    void ShowGameOver()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        int finalScore = CalculateScore();

        if (scoreText != null)
        {
            scoreText.text = $"Final Score: ${finalScore}";
        }

        Debug.Log($"Game Over! Final Score: ${finalScore}");
    }

    int CalculateScore()
    {
        int score = 0;

        if (currencyManager != null)
        {
            score = currencyManager.GetMoney();
        }

        return score;
    }
}