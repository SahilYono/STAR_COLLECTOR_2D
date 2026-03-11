using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Gameplay UI")]
    public TextMeshProUGUI scoreText;

    [Header("Game Over Panel")]
    public GameObject gameOverPanel;
    public TextMeshProUGUI gameOverScoreText;   // Shows final score on panel
    public TextMeshProUGUI highScoreText;       // Shows high score on panel

    private int score = 0;
    private const string HighScoreKey = "HighScore";

    public bool IsGameOver { get; private set; } = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    void Start()
    {
        score = 0;
        IsGameOver = false;
        UpdateScoreUI();
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreUI();
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = "" + score;
    }

    public void ShowGameOver()
    {
        IsGameOver = true;

        // Save high score
        int savedHigh = PlayerPrefs.GetInt(HighScoreKey, 0);
        if (score > savedHigh)
        {
            savedHigh = score;
            PlayerPrefs.SetInt(HighScoreKey, savedHigh);
            PlayerPrefs.Save();
        }

        // Update Game Over panel texts
        if (gameOverScoreText != null)
            gameOverScoreText.text = "Score: " + score;

        if (highScoreText != null)
            highScoreText.text = "High Score: " + savedHigh;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        Time.timeScale = 0f;
    }

    // Assign this to the Restart Button OnClick()
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}