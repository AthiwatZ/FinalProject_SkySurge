using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    public AudioSource uiAudioSource;
    public AudioClip buttonClickSfx;

    public void PlayGame()
    {
        SceneManager.LoadScene("Main");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit Game");
    }

    public void PlayButtonClick()
    {
        if (uiAudioSource != null && buttonClickSfx != null)
        {
            uiAudioSource.PlayOneShot(buttonClickSfx);
        }
    }
}

