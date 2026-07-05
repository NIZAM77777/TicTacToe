using UnityEngine;

public class ButtonAction : MonoBehaviour
{
    [SerializeField] private GameObject SettingsPanel;

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

    public void SettingsPanelOpen()
    {
        SettingsPanel.SetActive(true);
    }

    public void BackToMain()
    {
        SettingsPanel.SetActive(false);
    }
}