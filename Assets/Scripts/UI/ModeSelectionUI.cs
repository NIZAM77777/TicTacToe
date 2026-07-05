using UnityEngine;

public class ModeSelectionUI : MonoBehaviour
{
    [SerializeField] private GameObject difficultyPanel;

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
}