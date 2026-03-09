using UnityEngine;
using UnityEngine.SceneManagement;

public class CoinScript : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ScoreManager.AddScore(1);
            Destroy(gameObject);
        }
    }

}