using UnityEngine;
using UnityEngine.UIElements; // Required for UI Toolkit
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private Button startButton;

    void Start()
    {
        // Find the UI Document component and get the root visual element
        var uiDocument = GetComponent<UIDocument>();
        var root = uiDocument.rootVisualElement;

        // Find the button by name
        startButton = root.Q<Button>("startButton");

        // Add a click event listener
        startButton.clicked += StartGame;
    }

    // Method to handle the Start Game button click
    void StartGame()
    {
        // Load the next scene (ensure a scene is added in Build Settings)
        SceneManager.LoadScene("GameScene");
    }
}

