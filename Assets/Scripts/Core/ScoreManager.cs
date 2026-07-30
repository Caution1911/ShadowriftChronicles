using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    public int currentScore = 0;
    public Text scoreText;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void AddScore(int points)
    {
        currentScore += points;
        if (scoreText != null)
            scoreText.text = "Score: " + currentScore;
    }
}
