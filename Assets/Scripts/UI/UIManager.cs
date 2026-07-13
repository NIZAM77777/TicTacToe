using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text turnText;
    [SerializeField] private TMP_Text winnertext;

    public void UpdateTurnText(PlayerType player)
    {
        turnText.text = $"Player {player} Turn";
    }

    public void ShowWinner(PlayerType player)
    {
        winnertext.text = $"{player}";
    }

    public void ShowDraw()
    {
        turnText.text = "Draw!";
    }
}