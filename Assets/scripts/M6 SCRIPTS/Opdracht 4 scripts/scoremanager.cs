using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private static int score;
    public static event System.Action<int> OnScoreUpdated;

    public static void AddScore(int amount)
    {
        score += amount;
        OnScoreUpdated?.Invoke(score);
        Debug.Log("Added Score: " + score);
    }
}