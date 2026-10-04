using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public event Action<PlayerType> TurnChanged;
    public event Action<int, string> CellChanged;
    public event Action BoardReset;
    public event Action<int, int, int> WinningCellsChanged;
    public event Action<PlayerType> GameWon;
    public event Action GameDrawn;

    private readonly PlayerType?[] boardState = new PlayerType?[9];
    private readonly int[,] winningPatterns =
    {
        { 0, 1, 2 }, { 3, 4, 5 }, { 6, 7, 8 },
        { 0, 3, 6 }, { 1, 4, 7 }, { 2, 5, 8 },
        { 0, 4, 8 }, { 2, 4, 6 }
    };

    private PlayerType currentPlayer = PlayerType.X;
    private GameMode gameMode;
    private BotDifficulty botDifficulty;
    private readonly PlayerType humanPlayer = PlayerType.X;
    private readonly PlayerType botPlayer = PlayerType.O;
    private bool gameOver;
    private bool isBotThinking;
    private int moveCount;

    public PlayerType CurrentPlayer => currentPlayer;
    public GameMode CurrentGameMode => gameMode;

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
        TurnChanged?.Invoke(currentPlayer);
    }

    public void SelectCell(int index)
    {
        if (gameOver || isBotThinking || index < 0 || index >= boardState.Length)
            return;
        if (gameMode == GameMode.PlayerVsBot && currentPlayer == botPlayer)
            return;

        MakeMove(index);
    }

    public void RestartGame()
    {
        StopAllCoroutines();
        for (int i = 0; i < boardState.Length; i++)
            boardState[i] = null;

        currentPlayer = PlayerType.X;
        moveCount = 0;
        gameOver = false;
        isBotThinking = false;

        AudioManager.Instance?.PlayRestart();
        BoardReset?.Invoke();
        TurnChanged?.Invoke(currentPlayer);
    }

    private void MakeMove(int index)
    {
        if (boardState[index] != null)
            return;

        boardState[index] = currentPlayer;
        moveCount++;
        CellChanged?.Invoke(index, currentPlayer == PlayerType.X ? "X" : "O");

        if (currentPlayer == PlayerType.X)
            AudioManager.Instance?.PlayX();
        else
            AudioManager.Instance?.PlayO();

        if (TryGetWinningCells(out int a, out int b, out int c))
        {
            gameOver = true;
            WinningCellsChanged?.Invoke(a, b, c);
            AudioManager.Instance?.PlayWin();
            StartCoroutine(ShowResultAfterDelay(currentPlayer));
            return;
        }

        if (moveCount >= boardState.Length)
        {
            gameOver = true;
            AudioManager.Instance?.PlayDraw();
            StartCoroutine(ShowDrawAfterDelay());
            return;
        }

        SwitchTurn();
    }

    private void SwitchTurn()
    {
        currentPlayer = currentPlayer == PlayerType.X ? PlayerType.O : PlayerType.X;
        TurnChanged?.Invoke(currentPlayer);

        if (gameMode == GameMode.PlayerVsBot && currentPlayer == botPlayer)
            StartCoroutine(BotTurn());
    }

    private IEnumerator BotTurn()
    {
        isBotThinking = true;
        yield return new WaitForSeconds(0.5f);
        int move = GetBotMove();
        isBotThinking = false;
        if (move >= 0)
            MakeMove(move);
    }

    private bool TryGetWinningCells(out int a, out int b, out int c)
    {
        for (int i = 0; i < winningPatterns.GetLength(0); i++)
        {
            a = winningPatterns[i, 0];
            b = winningPatterns[i, 1];
            c = winningPatterns[i, 2];
            if (boardState[a] != null && boardState[a] == boardState[b] && boardState[b] == boardState[c])
                return true;
        }

        a = b = c = -1;
        return false;
    }

    private int GetBotMove()
    {
        return botDifficulty switch
        {
            BotDifficulty.Easy => GetEasyMove(),
            BotDifficulty.Medium => GetMediumMove(),
            BotDifficulty.Hard => GetHardMove(),
            _ => GetEasyMove()
        };
    }

    private int GetEasyMove()
    {
        if (moveCount >= boardState.Length)
            return -1;

        int randomIndex;
        do
        {
            randomIndex = UnityEngine.Random.Range(0, boardState.Length);
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
        return blockingMove != -1 ? blockingMove : GetEasyMove();
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
            if (boardState[a] == player && boardState[b] == player && boardState[c] == player)
                return true;
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

        int bestScore = isMaximizing ? int.MinValue : int.MaxValue;
        PlayerType player = isMaximizing ? botPlayer : humanPlayer;
        for (int i = 0; i < boardState.Length; i++)
        {
            if (boardState[i] != null)
                continue;
            boardState[i] = player;
            int score = Minimax(!isMaximizing);
            boardState[i] = null;
            bestScore = isMaximizing ? Mathf.Max(score, bestScore) : Mathf.Min(score, bestScore);
        }
        return bestScore;
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

    private IEnumerator ShowResultAfterDelay(PlayerType winner)
    {
        yield return new WaitForSeconds(0.5f);
        GameWon?.Invoke(winner);
        AdsManager.Instance?.InterstitialController?.TryShowInterstitial();
    }

    private IEnumerator ShowDrawAfterDelay()
    {
        yield return new WaitForSeconds(0.5f);
        GameDrawn?.Invoke();
        AdsManager.Instance?.InterstitialController?.TryShowInterstitial();
    }
}
