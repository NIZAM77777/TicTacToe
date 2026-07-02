using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text turnText;

    public void UpdateTurnText(PlayerType player)
    {
        turnText.text = $"Player {player} Turn";
    }
}