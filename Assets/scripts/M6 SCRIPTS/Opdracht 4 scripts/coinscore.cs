using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CoinUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;

    private void OnEnable()
    {
        ScoreManager.OnScoreUpdated += UpdateUI;
    }

    private void OnDisable()
    {
        ScoreManager.OnScoreUpdated -= UpdateUI;
    }

    private void UpdateUI(int newScore)
    {
        scoreText.text = newScore.ToString();
    }
}