using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private Board board;
    [SerializeField] private UIManager uiManager;

    private PlayerType currentPlayer = PlayerType.X;

    private PlayerType?[] boardState = new PlayerType?[9];

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        uiManager.UpdateTurnText(currentPlayer);
    }

    public void SelectCell(int index)
    {
        if (boardState[index] != null)
            return;

        boardState[index] = currentPlayer;

        string symbol = currentPlayer == PlayerType.X ? "X" : "O";

        board.Cells[index].SetSymbol(symbol);

        SwitchTurn();
    }

    private void SwitchTurn()
    {
        currentPlayer = currentPlayer == PlayerType.X
            ? PlayerType.O
            : PlayerType.X;

        uiManager.UpdateTurnText(currentPlayer);
    }
}