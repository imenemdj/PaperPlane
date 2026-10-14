using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private string gameSceneName;
    [SerializeField] private string optionsSceneName;
    [SerializeField] private TextMeshProUGUI bestScoreText;

    void Start()
    {
        DisplayBestScore();
        AudioManager.Instance?.PlayMenuMusic();
        AudioManager.Instance?.UnmuffleMusic(); // Make sure it's not muffled
    }

    public void DisplayBestScore()
    {
        int bestScore = PlayerPrefs.GetInt("BestScore", 0);
        if (bestScoreText != null)
            bestScoreText.text = bestScore.ToString();
    }

    public void Play()
    {
        AudioManager.Instance?.PlayButtonClick();
        StartCoroutine(LoadSceneWithDelay(gameSceneName, 0.1f));
    }

    public void Options()
    {
        AudioManager.Instance?.PlayButtonClick();
        StartCoroutine(LoadSceneWithDelay(optionsSceneName, 0.1f));
    }

    public void Quit()
    {
        AudioManager.Instance?.PlayButtonClick();
        StartCoroutine(QuitWithDelay());
    }

    private IEnumerator LoadSceneWithDelay(string sceneName, float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneName);
    }

    private IEnumerator QuitWithDelay()
    {
        yield return new WaitForSeconds(0.3f); // Wait for button click to finish
        Application.Quit();
    }
}
