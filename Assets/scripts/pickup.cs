using TMPro;
using UnityEngine;
using UnityEngine.UI;  // Add this for accessing UI elements

public class PickupScript : MonoBehaviour
{
    private bool isPlayerNearby = false; // Detecteer of de speler dichtbij is
    public static int score = 0; // Static variable for score
    public TextMeshProUGUI scoreText; // Reference to the Text component to display the score

    private AudioSource audioSource; // Reference to the AudioSource component

    // Start is called before the first frame update
    void Start()
    {
        // Get the AudioSource component attached to the item
        audioSource = GetComponent<AudioSource>();

        // Update the UI with the initial score
        UpdateScoreUI();
    }

    // Update is called once per frame
    void Update()
    {
        // Als de speler dichtbij is en de speler op de E-toets drukt
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E)) // Druk op 'E' om op te pakken
        {
            PickupItem();
        }
    }

    // Detecteer wanneer de speler in de buurt komt (via trigger)
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Zorg ervoor dat je speler de juiste tag heeft
        {
            isPlayerNearby = true; // Speler is dichtbij
            Debug.Log("Druk op E om op te pakken!"); // Debug bericht
        }
    }

    // Detecteer wanneer de speler de trigger verlaat
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false; // Speler is niet meer dichtbij
            Debug.Log("Speler verliet de trigger"); // Debug bericht
        }
    }

    // Functie die wordt aangeroepen wanneer de speler het object oppakt
    private void PickupItem()
    {
        // Verhoog de score wanneer het object wordt opgepakt
        score++;
        Debug.Log("Object opgepakt! Huidige score: " + score); // Toon score in de debug log

        // Play the pickup sound if the AudioSource exists
        if (audioSource != null)
        {
            audioSource.Play(); // Play the sound when the item is picked up
        }

        // Update the score text in the UI
        UpdateScoreUI();

        // Verwijder het object uit de scene nadat het is opgepakt
        Destroy(gameObject);
    }

    // Update the score UI on the screen
    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score; // Update the text component to show the current score
        }
    }
}
