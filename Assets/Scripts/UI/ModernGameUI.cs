using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class ModernGameUI : MonoBehaviour
{
    private readonly Button[] cellButtons = new Button[9];
    private readonly TMP_Text[] cellLabels = new TMP_Text[9];

    private GameManager gameManager;
    private RectTransform gamePage;
    private RectTransform resultPage;
    private TMP_Text turnLabel;
    private TMP_Text resultTitle;
    private TMP_Text resultSubtitle;
    private Image resultIcon;
    private PlayerType displayedTurn = PlayerType.X;
    private PlayerType displayedWinner = PlayerType.X;
    private bool showingDraw;
    private bool showingLoss;

    public void Build(GameManager manager)
    {
        gameManager = manager;
        Canvas canvas = ModernUIFactory.CreateCanvas("ModernUI");
        canvas.transform.SetParent(transform, false);

        Image background = ModernUIFactory.CreateImage(
            "Background", canvas.transform, ModernUIFactory.LoadSprite("ModernUI/Light"));
        ModernUIFactory.Stretch(background.rectTransform);
        ThemeBackground themeBackground = background.gameObject.AddComponent<ThemeBackground>();
        themeBackground.Configure(
            background,
            ModernUIFactory.LoadSprite("ModernUI/Light"),
            ModernUIFactory.LoadSprite("ModernUI/Dark"));

        gamePage = ModernUIFactory.CreateContainer("GamePage", canvas.transform);
        resultPage = ModernUIFactory.CreateContainer("ResultPage", canvas.transform);

        BuildGamePage();
        BuildResultPage();

        gameManager.TurnChanged += OnTurnChanged;
        gameManager.CellChanged += OnCellChanged;
        gameManager.BoardReset += OnBoardReset;
        gameManager.WinningCellsChanged += OnWinningCellsChanged;
        gameManager.GameWon += OnGameWon;
        gameManager.GameDrawn += OnGameDrawn;
        ModernLocalization.LanguageChanged += RefreshLocalizedState;

        displayedTurn = gameManager.CurrentPlayer;
        RefreshTurnLabel();
        ShowGamePage();
    }

    private void OnDestroy()
    {
        if (gameManager != null)
        {
            gameManager.TurnChanged -= OnTurnChanged;
            gameManager.CellChanged -= OnCellChanged;
            gameManager.BoardReset -= OnBoardReset;
            gameManager.WinningCellsChanged -= OnWinningCellsChanged;
            gameManager.GameWon -= OnGameWon;
            gameManager.GameDrawn -= OnGameDrawn;
        }
        ModernLocalization.LanguageChanged -= RefreshLocalizedState;
    }

    private void BuildGamePage()
    {
        turnLabel = ModernUIFactory.CreateText(
            "TurnLabel", gamePage, "", 78f,
            ModernUIFactory.Navy, Color.white);
        ModernUIFactory.SetRect(turnLabel.rectTransform, new Vector2(900f, 150f), new Vector2(0f, 670f));

        Image blueAccent = ModernUIFactory.CreatePanel(
            "BlueAccent", gamePage, new Vector2(130f, 14f), new Vector2(-80f, 590f),
            ModernUIFactory.Blue, ModernUIFactory.Cyan);
        blueAccent.GetComponent<Shadow>().enabled = false;
        Image paleAccent = ModernUIFactory.CreatePanel(
            "PaleAccent", gamePage, new Vector2(82f, 14f), new Vector2(86f, 590f),
            new Color(0.82f, 0.87f, 0.94f, 1f), new Color(0.28f, 0.4f, 0.56f, 1f));
        paleAccent.GetComponent<Shadow>().enabled = false;

        const float spacing = 286f;
        for (int i = 0; i < cellButtons.Length; i++)
        {
            int index = i;
            int row = i / 3;
            int column = i % 3;
            Vector2 position = new((column - 1) * spacing, 280f - row * spacing);
            Button cell = ModernUIFactory.CreateTextButton(
                "Cell" + i, gamePage, new Vector2(258f, 258f), position,
                "", new Color(0.94f, 0.975f, 1f, 0.96f), ModernUIFactory.DarkPanel,
                ModernUIFactory.Blue, ModernUIFactory.Cyan,
                () => gameManager.SelectCell(index), 132f);
            Outline outline = cell.gameObject.AddComponent<Outline>();
            outline.effectColor = new Color(0.25f, 0.66f, 1f, 0.8f);
            outline.effectDistance = new Vector2(3f, -3f);
            cellButtons[i] = cell;
            cellLabels[i] = cell.transform.Find("Label").GetComponent<TMP_Text>();
            Shadow markShadow = cellLabels[i].gameObject.AddComponent<Shadow>();
            markShadow.effectColor = new Color(0.02f, 0.28f, 0.8f, 0.3f);
            markShadow.effectDistance = new Vector2(0f, -8f);
        }

        ModernUIFactory.CreateIconButton(
            "RestartButton", gamePage, new Vector2(400f, 145f), new Vector2(-230f, -690f),
            "restart", ModernUIFactory.Icon.Restart,
            new Color(1f, 1f, 1f, 0.97f), ModernUIFactory.DarkPanel,
            ModernUIFactory.Navy, Color.white, RestartGame, ModernUIFactory.Blue, 88f, 40f);

        ModernUIFactory.CreateIconButton(
            "HomeButton", gamePage, new Vector2(400f, 145f), new Vector2(230f, -690f),
            "home", ModernUIFactory.Icon.Home,
            new Color(1f, 1f, 1f, 0.97f), ModernUIFactory.DarkPanel,
            ModernUIFactory.Navy, Color.white, GoHome, ModernUIFactory.Blue, 88f, 40f);
    }

    private void BuildResultPage()
    {
        resultIcon = ModernUIFactory.CreateImage(
            "ResultIcon", resultPage, ModernUIFactory.LoadSprite("ModernUI/Trophy"), true);
        ModernUIFactory.SetRect(resultIcon.rectTransform, new Vector2(610f, 610f), new Vector2(0f, 390f));

        resultTitle = ModernUIFactory.CreateText(
            "ResultTitle", resultPage, "", 104f,
            ModernUIFactory.Navy, Color.white);
        resultTitle.textWrappingMode = TextWrappingModes.Normal;
        ModernUIFactory.SetRect(resultTitle.rectTransform, new Vector2(900f, 250f), new Vector2(0f, -35f));

        resultSubtitle = ModernUIFactory.CreateText(
            "ResultSubtitle", resultPage, "", 44f,
            ModernUIFactory.Muted, new Color(0.75f, 0.84f, 0.96f, 1f),
            TextAlignmentOptions.Center, FontStyles.Normal);
        ModernUIFactory.SetRect(resultSubtitle.rectTransform, new Vector2(850f, 100f), new Vector2(0f, -190f));

        ModernUIFactory.CreateIconButton(
            "RestartButton", resultPage, new Vector2(400f, 145f), new Vector2(-230f, -475f),
            "restart", ModernUIFactory.Icon.Restart,
            new Color(1f, 1f, 1f, 0.97f), ModernUIFactory.DarkPanel,
            ModernUIFactory.Navy, Color.white, RestartGame, ModernUIFactory.Blue, 88f, 40f);

        ModernUIFactory.CreateIconButton(
            "HomeButton", resultPage, new Vector2(400f, 145f), new Vector2(230f, -475f),
            "home", ModernUIFactory.Icon.Home,
            new Color(1f, 1f, 1f, 0.97f), ModernUIFactory.DarkPanel,
            ModernUIFactory.Navy, Color.white, GoHome, ModernUIFactory.Blue, 88f, 40f);
    }

    private void OnTurnChanged(PlayerType player)
    {
        displayedTurn = player;
        RefreshTurnLabel();
    }

    private void OnCellChanged(int index, string symbol)
    {
        if (index < 0 || index >= cellLabels.Length)
            return;

        cellLabels[index].text = symbol;
        cellLabels[index].color = symbol == "X" ? ModernUIFactory.Blue : ModernUIFactory.Orange;
        cellButtons[index].interactable = false;
    }

    private void OnBoardReset()
    {
        for (int i = 0; i < cellButtons.Length; i++)
        {
            cellLabels[i].text = "";
            cellButtons[i].interactable = true;
            Image image = cellButtons[i].GetComponent<Image>();
            image.color = ModernUITheme.IsDark
                ? ModernUIFactory.DarkPanel
                : new Color(0.94f, 0.975f, 1f, 0.96f);
        }
        ShowGamePage();
    }

    private void OnWinningCellsChanged(int a, int b, int c)
    {
        Color highlight = new(1f, 0.78f, 0.15f, 1f);
        cellButtons[a].GetComponent<Image>().color = highlight;
        cellButtons[b].GetComponent<Image>().color = highlight;
        cellButtons[c].GetComponent<Image>().color = highlight;
    }

    private void OnGameWon(PlayerType winner)
    {
        displayedWinner = winner;
        showingDraw = false;
        showingLoss = gameManager.CurrentGameMode == GameMode.PlayerVsBot && winner == PlayerType.O;
        resultIcon.sprite = ModernUIFactory.LoadSprite(showingLoss ? "ModernUI/LoseX" : "ModernUI/Trophy");
        resultIcon.rectTransform.sizeDelta = showingLoss ? new Vector2(510f, 510f) : new Vector2(610f, 610f);
        RefreshResultText();
        gamePage.gameObject.SetActive(false);
        resultPage.gameObject.SetActive(true);
    }

    private void OnGameDrawn()
    {
        showingDraw = true;
        showingLoss = false;
        resultIcon.sprite = ModernUIFactory.LoadSprite("ModernUI/Trophy");
        resultIcon.rectTransform.sizeDelta = new Vector2(520f, 520f);
        RefreshResultText();
        gamePage.gameObject.SetActive(false);
        resultPage.gameObject.SetActive(true);
    }

    private void RefreshLocalizedState()
    {
        RefreshTurnLabel();
        if (resultPage != null && resultPage.gameObject.activeSelf)
            RefreshResultText();
    }

    private void RefreshTurnLabel()
    {
        if (turnLabel != null)
        {
            Color turnColor = displayedTurn == PlayerType.X ? ModernUIFactory.Blue : ModernUIFactory.Orange;
            string coloredPlayer = $"<color=#{ColorUtility.ToHtmlStringRGB(turnColor)}>{displayedTurn}</color>";
            turnLabel.text = ModernLocalization.Format("player_turn", coloredPlayer);
        }
    }

    private void RefreshResultText()
    {
        if (showingDraw)
        {
            resultTitle.text = ModernLocalization.Get("draw");
            resultSubtitle.text = ModernLocalization.Get("draw_desc");
            resultTitle.color = ModernUIFactory.Blue;
        }
        else if (showingLoss)
        {
            resultTitle.text = ModernLocalization.Get("you_lose");
            resultSubtitle.text = ModernLocalization.Get("better_luck");
            resultTitle.color = ModernUIFactory.Red;
        }
        else
        {
            resultTitle.text = ModernLocalization.Format("player_wins", displayedWinner);
            resultSubtitle.text = ModernLocalization.Get("congratulations");
            resultTitle.color = ModernUIFactory.Blue;
        }
    }

    private void RestartGame()
    {
        gameManager.RestartGame();
    }

    private static void GoHome()
    {
        if (SceneLoader.Instance != null)
            SceneLoader.Instance.LoadMainMenu();
    }

    private void ShowGamePage()
    {
        gamePage.gameObject.SetActive(true);
        resultPage.gameObject.SetActive(false);
    }
}
