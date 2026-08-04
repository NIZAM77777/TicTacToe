using UnityEngine;

public class ModeSelectionUI : MonoBehaviour
{
    [SerializeField] private GameObject difficultyPanel;
    [SerializeField] private GameObject SettingsPanel;


    public void SelectPlayerVsPlayer()
    {
        GameSettings.SelectedGameMode = GameMode.PlayerVsPlayer;

        SceneLoader.Instance.LoadGame();
    }

    public void SelectPlayerVsBot()
    {
        GameSettings.SelectedGameMode = GameMode.PlayerVsBot;

        difficultyPanel.SetActive(true);
    }

    public void SelectEasy()
    {
        GameSettings.SelectedBotDifficulty = BotDifficulty.Easy;

        SceneLoader.Instance.LoadGame();
    }

    public void SelectMedium()
    {
        GameSettings.SelectedBotDifficulty = BotDifficulty.Medium;

        SceneLoader.Instance.LoadGame();
    }

    public void SelectHard()
    {
        GameSettings.SelectedBotDifficulty = BotDifficulty.Hard;

        SceneLoader.Instance.LoadGame();
    }

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