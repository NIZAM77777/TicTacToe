using UnityEngine;

public class ButtonAction : MonoBehaviour
{
    public void LoadGame()
    {
        SceneLoader.Instance.LoadGame();
    }

    public void LoadMainMenu()
    {
        SceneLoader.Instance.LoadMainMenu();
    }

    public void ExitGame()
    {
        SceneLoader.Instance.ExitGame();
    }
}