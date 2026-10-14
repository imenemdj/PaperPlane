using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class Options : MonoBehaviour
{
    [SerializeField] private string mainMenuSceneName;
    [SerializeField] private TextMeshProUGUI bestScoreText;

    [Header("Mute Toggle Visuals")]
    [SerializeField] private GameObject musicUnchecked;
    [SerializeField] private GameObject sfxUnchecked;
    [SerializeField] private GameObject bgSfxUnchecked;

    void Start()
    {
        DisplayBestScore();
        AudioManager.Instance?.MuffleMusic(); // Apply muffled effect
        UpdateToggleVisuals();
    }

    private void UpdateToggleVisuals()
    {
        if (AudioManager.Instance == null) return;

        // Show unchecked when muted
        if (musicUnchecked != null)
            musicUnchecked.SetActive(AudioManager.Instance.IsMusicMuted());
        if (sfxUnchecked != null)
            sfxUnchecked.SetActive(AudioManager.Instance.IsSfxMuted());
        if (bgSfxUnchecked != null)
            bgSfxUnchecked.SetActive(AudioManager.Instance.IsBgSfxMuted());
    }

    public void DisplayBestScore()
    {
        int bestScore = PlayerPrefs.GetInt("BestScore", 0);
        if (bestScoreText != null)
            bestScoreText.text = bestScore.ToString();
    }

    public void ResetBestScore()
    {
        AudioManager.Instance?.PlayButtonClick();
        PlayerPrefs.SetInt("BestScore", 0);
        PlayerPrefs.Save();
        DisplayBestScore(); // Update the display after reset
    }

    public void ToggleMusic()
    {
        AudioManager.Instance?.PlayButtonClick();

        bool wasMuted = AudioManager.Instance.IsMusicMuted();
        AudioManager.Instance?.ToggleMusic();

        // If we just unmuted, restart the menu music
        if (wasMuted && !AudioManager.Instance.IsMusicMuted())
        {
            AudioManager.Instance?.PlayMenuMusic();
        }

        UpdateToggleVisuals();
    }

    public void ToggleSfx()
    {
        AudioManager.Instance?.PlayButtonClick();
        AudioManager.Instance?.ToggleSfx();
        UpdateToggleVisuals();
    }

    public void ToggleBgSfx()
    {
        AudioManager.Instance?.PlayButtonClick();
        AudioManager.Instance?.ToggleBgSfx();
        UpdateToggleVisuals();
    }

    public void Back()
    {
        AudioManager.Instance?.PlayButtonClick();
        StartCoroutine(LoadSceneWithDelay(mainMenuSceneName, 0.1f));
    }

    private IEnumerator LoadSceneWithDelay(string sceneName, float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneName);
    }
}
