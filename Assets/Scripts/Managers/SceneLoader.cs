using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadMainMenu()
    {
        AudioManager.Instance.PlayMusic(AudioManager.Instance.MainMenuMusic);

        SceneManager.LoadScene("MainMenu");
    }

    public void LoadGame()
    {
        AudioManager.Instance.PlayMusic(AudioManager.Instance.GameplayMusic);

        SceneManager.LoadScene("Game");
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}