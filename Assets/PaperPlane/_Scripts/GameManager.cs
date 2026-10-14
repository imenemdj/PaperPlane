using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI TextMeshProUGUI;
    public TextMeshProUGUI bestScoreText; // Optional: assign in Inspector to display best score

    public GameObject gameOverScreen;
    int score = 0;
    int bestScore = 0;

    void Start()
    {
        // Load the best score from PlayerPrefs
        bestScore = PlayerPrefs.GetInt("BestScore", 0);

        // Update UI if you have a best score text element
        if (bestScoreText != null)
            bestScoreText.text = bestScore.ToString();

        // Start playing music when game starts
        AudioManager.Instance?.PlayMusic();

        // Start playing ambiente sfx when game starts
        AudioManager.Instance?.PlayAmbient();
    }

    [ContextMenu("AddScore")]
    public void AddScore()
    {
        score++;
        TextMeshProUGUI.text = score.ToString();
        AudioManager.Instance?.CheckMusicSwitch(score);
    }

    public void GameOver()
    {
        // Stop music when player dies (but keep ambient sound playing)
        AudioManager.Instance?.StopMusic();

        // Reset parallax backgrounds that keep moving (not stopped on death) to normal speed
        foreach (var parallax in FindObjectsByType<Parallax>(FindObjectsSortMode.None))
        {
            if (!parallax.stopOnPlayerDeath)
                parallax.SetNormalSpeed();
        }

        // Check if current score is better than best score
        if (score > bestScore)
        {
            bestScore = score;
            PlayerPrefs.SetInt("BestScore", bestScore);
            PlayerPrefs.Save(); // Ensures immediate write to disk
        }

        // Update best score display on game over screen
        if (bestScoreText != null)
            bestScoreText.text = bestScore.ToString();

        gameOverScreen.SetActive(true);
    }

    public void Restart()
    {
        AudioManager.Instance?.PlayButtonClick();
        StartCoroutine(RestartWithDelay(0.1f));
    }

    public void LoadMainMenu()
    {
        AudioManager.Instance?.StopMusic(); // Stop game music
        AudioManager.Instance?.StopAmbient(); // Stop ambient sound before going to menu
        AudioManager.Instance?.PlayButtonClick(); // Play click after stopping ambient
        StartCoroutine(LoadMainMenuWithDelay());
    }

    public void Exit()
    {
        AudioManager.Instance?.PlayButtonClick();
        StartCoroutine(ExitWithDelay());
    }

    private IEnumerator RestartWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private IEnumerator LoadMainMenuWithDelay()
    {
        yield return new WaitForSeconds(0.3f); // Wait for button click to finish
        AudioManager.Instance?.PlayMenuMusic(); // Start menu music
        SceneManager.LoadScene(0);
    }

    private IEnumerator ExitWithDelay()
    {
        yield return new WaitForSeconds(0.3f); // Wait for button click to finish
        Application.Quit();
    }
}
