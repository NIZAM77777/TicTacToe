using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private Board board;
    [SerializeField] private UIManager uiManager;

    private PlayerType currentPlayer = PlayerType.X;

    private PlayerType?[] boardState = new PlayerType?[9];

    private bool gameOver = false;

    private int moveCount = 0;

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
    private readonly int[,] winningPatterns =
{
    {0,1,2},
    {3,4,5},
    {6,7,8},

    {0,3,6},
    {1,4,7},
    {2,5,8},

    {0,4,8},
    {2,4,6}
};
    private bool CheckDraw()
    {
        return moveCount >= 9;
    }
    public void SelectCell(int index)
    {

        if (gameOver)
            return;

        if (boardState[index] != null)
            return;
        
        boardState[index] = currentPlayer;

        moveCount++;

        string symbol = currentPlayer == PlayerType.X ? "X" : "O";

        board.Cells[index].SetSymbol(symbol);

        if (currentPlayer == PlayerType.X)
        {
            AudioManager.Instance.PlayX();
        }
        else
        {
            AudioManager.Instance.PlayO();
        }

        if (CheckWinner())
        {
            gameOver = true;
            AudioManager.Instance.PlayWin();
            uiManager.ShowWinner(currentPlayer);
            return;
        }

        if (CheckDraw())
        {
            gameOver = true;
            AudioManager.Instance.PlayDraw();
            uiManager.ShowDraw();
            return;
        }

        SwitchTurn();

    }


    private void SwitchTurn()
    {
        currentPlayer = currentPlayer == PlayerType.X
            ? PlayerType.O
            : PlayerType.X;

        uiManager.UpdateTurnText(currentPlayer);
    }
    private bool CheckWinner()
    {

        for (int i = 0; i < winningPatterns.GetLength(0); i++)
        {
            int a = winningPatterns[i, 0];
            int b = winningPatterns[i, 1];
            int c = winningPatterns[i, 2];

            if (boardState[a] == null)
                continue;

            if (boardState[a] == boardState[b] &&
                boardState[b] == boardState[c])
            {
                HighlightWinningCells(a, b, c);

                return true;
            }
        }

        return false;
    }

    private void HighlightWinningCells(int a, int b, int c)
    {
        board.Cells[a].ShowGlow();
        board.Cells[b].ShowGlow();
        board.Cells[c].ShowGlow();
    }

    public void RestartGame()
    {
        board.ResetBoard();

        boardState = new PlayerType?[9];

        currentPlayer = PlayerType.X;

        moveCount = 0;

        gameOver = false;

        AudioManager.Instance.PlayRestart();

        uiManager.UpdateTurnText(currentPlayer);
    }
}