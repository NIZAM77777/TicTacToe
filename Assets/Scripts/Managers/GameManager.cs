using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private Board board;
    [SerializeField] private UIManager uiManager;

    private PlayerType currentPlayer = PlayerType.X;

    private PlayerType?[] boardState = new PlayerType?[9];

    private bool gameOver = false;

    private int moveCount = 0;

    private GameMode gameMode;
    private BotDifficulty botDifficulty;

    private PlayerType humanPlayer = PlayerType.X;
    private PlayerType botPlayer = PlayerType.O;

    private bool isBotThinking;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        gameMode = GameSettings.SelectedGameMode;
        botDifficulty = GameSettings.SelectedBotDifficulty;

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

        if (isBotThinking)
            return;

        if (gameMode == GameMode.PlayerVsBot &&
            currentPlayer == botPlayer)
            return;

        MakeMove(index);

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

        if (gameMode == GameMode.PlayerVsBot &&
        currentPlayer == botPlayer)
        {
            StartCoroutine(BotTurn());
        }
    }

    private IEnumerator BotTurn()
    {
        isBotThinking = true;

        yield return new WaitForSeconds(0.5f);

        int move = GetBotMove();

        MakeMove(move);

        isBotThinking = false;
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

    private void MakeMove(int index)
    {
        if (boardState[index] != null)
            return;

        boardState[index] = currentPlayer;

        moveCount++;

        string symbol = currentPlayer == PlayerType.X
            ? "X"
            : "O";

        board.Cells[index].SetSymbol(symbol);

        if (currentPlayer == PlayerType.X)
            AudioManager.Instance.PlayX();
        else
            AudioManager.Instance.PlayO();

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

    private int GetBotMove()
    {
        switch (botDifficulty)
        {
            case BotDifficulty.Easy:
                return GetEasyMove();

            case BotDifficulty.Medium:
                return GetMediumMove();

            case BotDifficulty.Hard:
                return GetHardMove();

            default:
                return GetEasyMove();
        }
    }

    private int GetEasyMove()
    {
        int randomIndex;

        do
        {
            randomIndex = Random.Range(0, boardState.Length);
        }
        while (boardState[randomIndex] != null);

        return randomIndex;
    }

    private int GetMediumMove()
    {
        int winningMove = FindWinningMove(botPlayer);

        if (winningMove != -1)
            return winningMove;

        int blockingMove = FindWinningMove(humanPlayer);

        if (blockingMove != -1)
            return blockingMove;

        return GetEasyMove();
    }
    private int FindWinningMove(PlayerType player)
    {
        for (int i = 0; i < boardState.Length; i++)
        {
            if (boardState[i] != null)
                continue;

            boardState[i] = player;

            bool canWin = HasWinner(player);

            boardState[i] = null;

            if (canWin)
                return i;
        }

        return -1;
    }

    private bool HasWinner(PlayerType player)
    {
        for (int i = 0; i < winningPatterns.GetLength(0); i++)
        {
            int a = winningPatterns[i, 0];
            int b = winningPatterns[i, 1];
            int c = winningPatterns[i, 2];

            if (boardState[a] == player &&
                boardState[b] == player &&
                boardState[c] == player)
            {
                return true;
            }
        }

        return false;
    }

    private int GetHardMove()
    {
        int bestScore = int.MinValue;
        int bestMove = -1;

        for (int i = 0; i < boardState.Length; i++)
        {
            if (boardState[i] != null)
                continue;

            boardState[i] = botPlayer;

            int score = Minimax(false);

            boardState[i] = null;

            if (score > bestScore)
            {
                bestScore = score;
                bestMove = i;
            }
        }

        return bestMove;
    }

    private int Minimax(bool isMaximizing)
    {
        if (HasWinner(botPlayer))
            return 10;

        if (HasWinner(humanPlayer))
            return -10;

        if (IsBoardFull())
            return 0;

        if (isMaximizing)
        {
            int bestScore = int.MinValue;

            for (int i = 0; i < boardState.Length; i++)
            {
                if (boardState[i] != null)
                    continue;

                boardState[i] = botPlayer;

                int score = Minimax(false);

                boardState[i] = null;

                bestScore = Mathf.Max(score, bestScore);
            }

            return bestScore;
        }
        else
        {
            int bestScore = int.MaxValue;

            for (int i = 0; i < boardState.Length; i++)
            {
                if (boardState[i] != null)
                    continue;

                boardState[i] = humanPlayer;

                int score = Minimax(true);

                boardState[i] = null;

                bestScore = Mathf.Min(score, bestScore);
            }

            return bestScore;
        }
    }

    private bool IsBoardFull()
    {
        for (int i = 0; i < boardState.Length; i++)
        {
            if (boardState[i] == null)
                return false;
        }

        return true;
    }
}