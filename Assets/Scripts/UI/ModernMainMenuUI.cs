using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class ModernMainMenuUI : MonoBehaviour
{
    private RectTransform mainPage;
    private RectTransform modePage;
    private RectTransform difficultyPage;
    private RectTransform settingsPage;
    private TMP_Text languageValue;
    private TMP_Text lightValue;
    private TMP_Text darkValue;

    public void Build()
    {
        Canvas canvas = ModernUIFactory.CreateCanvas("ModernUI");
        canvas.transform.SetParent(transform, false);

        Image background = ModernUIFactory.CreateImage(
            "Background",
            canvas.transform,
            ModernUIFactory.LoadSprite("ModernUI/Light"));
        ModernUIFactory.Stretch(background.rectTransform);
        ThemeBackground themeBackground = background.gameObject.AddComponent<ThemeBackground>();
        themeBackground.Configure(
            background,
            ModernUIFactory.LoadSprite("ModernUI/Light"),
            ModernUIFactory.LoadSprite("ModernUI/Dark"));

        mainPage = ModernUIFactory.CreateContainer("MainPage", canvas.transform);
        modePage = ModernUIFactory.CreateContainer("ModePage", canvas.transform);
        difficultyPage = ModernUIFactory.CreateContainer("DifficultyPage", canvas.transform);
        settingsPage = ModernUIFactory.CreateContainer("SettingsPage", canvas.transform);

        BuildMainPage();
        BuildModePage();
        BuildDifficultyPage();
        BuildSettingsPage();

        ModernLocalization.LanguageChanged += RefreshDynamicLabels;
        ModernUITheme.ThemeChanged += RefreshDynamicLabels;
        ShowPage("main");
    }

    private void OnDestroy()
    {
        ModernLocalization.LanguageChanged -= RefreshDynamicLabels;
        ModernUITheme.ThemeChanged -= RefreshDynamicLabels;
    }

    private void BuildMainPage()
    {
        Image hero = ModernUIFactory.CreateImage(
            "Hero",
            mainPage,
            ModernUIFactory.LoadSprite("ModernUI/ModernHeroV2"),
            true);
        ModernUIFactory.SetRect(hero.rectTransform, new Vector2(760f, 1040f), new Vector2(0f, 325f));

        ModernUIFactory.CreateIconButton(
            "PlayButton", mainPage, new Vector2(690f, 170f), new Vector2(0f, -350f),
            "play", ModernUIFactory.Icon.Play, ModernUIFactory.Blue, ModernUIFactory.Blue,
            Color.white, Color.white, () => ShowPage("mode"), ModernUIFactory.Cyan, 112f, 58f);

        ModernUIFactory.CreateIconButton(
            "SettingsButton", mainPage, new Vector2(610f, 140f), new Vector2(0f, -550f),
            "settings", ModernUIFactory.Icon.Settings, new Color(1f, 1f, 1f, 0.97f), ModernUIFactory.DarkPanel,
            ModernUIFactory.Navy, Color.white, () => ShowPage("settings"), ModernUIFactory.Cyan, 88f, 46f);

        ModernUIFactory.CreateIconButton(
            "ExitButton", mainPage, new Vector2(610f, 140f), new Vector2(0f, -715f),
            "exit", ModernUIFactory.Icon.Exit, new Color(1f, 1f, 1f, 0.97f), ModernUIFactory.DarkPanel,
            ModernUIFactory.Navy, Color.white, ExitGame, ModernUIFactory.Orange, 88f, 46f);
    }

    private void BuildModePage()
    {
        CreateHeader(modePage, "select_mode");

        CreateChoiceCard(
            modePage, new Vector2(0f, 190f), "vs_human", "vs_human_desc",
            ModernUIFactory.Icon.Human, ModernUIFactory.Icon.NextBlue, ModernUIFactory.Blue, () =>
            {
                GameSettings.SelectedGameMode = GameMode.PlayerVsPlayer;
                LoadGame();
            });

        CreateChoiceCard(
            modePage, new Vector2(0f, -190f), "vs_bot", "vs_bot_desc",
            ModernUIFactory.Icon.Bot, ModernUIFactory.Icon.NextOrange,
            ModernUIFactory.Orange, () => ShowPage("difficulty"));

        CreateBackButton(modePage, () => ShowPage("main"));
    }

    private void BuildDifficultyPage()
    {
        CreateHeader(difficultyPage, "select_difficulty");

        CreateChoiceCard(
            difficultyPage, new Vector2(0f, 270f), "easy", "easy_desc",
            ModernUIFactory.Icon.Easy, ModernUIFactory.Icon.NextBlue,
            new Color(0.1f, 0.7f, 0.2f, 1f), () => SelectDifficulty(BotDifficulty.Easy), new Vector2(830f, 245f));

        CreateChoiceCard(
            difficultyPage, new Vector2(0f, -30f), "medium", "medium_desc",
            ModernUIFactory.Icon.Medium, ModernUIFactory.Icon.NextOrange,
            new Color(1f, 0.62f, 0.05f, 1f), () => SelectDifficulty(BotDifficulty.Medium), new Vector2(830f, 245f));

        CreateChoiceCard(
            difficultyPage, new Vector2(0f, -330f), "hard", "hard_desc",
            ModernUIFactory.Icon.Hard, ModernUIFactory.Icon.NextOrange,
            ModernUIFactory.Red, () => SelectDifficulty(BotDifficulty.Hard), new Vector2(830f, 245f));

        CreateBackButton(difficultyPage, () => ShowPage("mode"));
    }

    private void BuildSettingsPage()
    {
        CreateHeader(settingsPage, "settings");

        Image musicCard = ModernUIFactory.CreatePanel(
            "MusicCard", settingsPage, new Vector2(820f, 220f), new Vector2(0f, 300f),
            new Color(1f, 1f, 1f, 0.96f), ModernUIFactory.DarkPanel, ModernUIFactory.Blue);
        CreateSettingsIconBadge(musicCard.transform, ModernUIFactory.Icon.Music);
        TMP_Text musicTitle = ModernUIFactory.CreateLocalizedText(
            "MusicTitle", musicCard.transform, "music", 42f,
            ModernUIFactory.Navy, Color.white, TextAlignmentOptions.TopLeft);
        ModernUIFactory.SetRect(musicTitle.rectTransform, new Vector2(570f, 58f), new Vector2(105f, 53f));
        CreateVolumeSlider(musicCard.transform);

        Image languageCard = ModernUIFactory.CreatePanel(
            "LanguageCard", settingsPage, new Vector2(820f, 220f), new Vector2(0f, 40f),
            new Color(1f, 1f, 1f, 0.96f), ModernUIFactory.DarkPanel, ModernUIFactory.Blue);
        CreateSettingsIconBadge(languageCard.transform, ModernUIFactory.Icon.Language);
        TMP_Text languageTitle = ModernUIFactory.CreateLocalizedText(
            "LanguageTitle", languageCard.transform, "language", 40f,
            ModernUIFactory.Navy, Color.white, TextAlignmentOptions.TopLeft);
        ModernUIFactory.SetRect(languageTitle.rectTransform, new Vector2(570f, 58f), new Vector2(105f, 53f));

        Image languageSelector = ModernUIFactory.CreatePanel(
            "LanguageSelector", languageCard.transform, new Vector2(565f, 82f), new Vector2(105f, -42f),
            new Color(0.93f, 0.97f, 1f, 1f), new Color(0.05f, 0.16f, 0.3f, 1f), ModernUIFactory.Blue);
        languageSelector.raycastTarget = true;
        Button languageButton = languageSelector.gameObject.AddComponent<Button>();
        languageButton.targetGraphic = languageSelector;
        languageButton.onClick.AddListener(() => ModernLocalization.Cycle(1));
        languageValue = ModernUIFactory.CreateText(
            "LanguageValue", languageSelector.transform, ModernLocalization.CurrentLanguageName, 38f,
            ModernUIFactory.Navy, Color.white, TextAlignmentOptions.Left);
        ModernUIFactory.SetRect(languageValue.rectTransform, new Vector2(410f, 70f), new Vector2(-45f, 0f));
        ModernUIFactory.CreateIcon(
            "OpenLanguage", languageSelector.transform, ModernUIFactory.Icon.NextBlue,
            new Vector2(55f, 55f), new Vector2(235f, 0f), -90f);

        Image themeCard = ModernUIFactory.CreatePanel(
            "ThemeCard", settingsPage, new Vector2(820f, 220f), new Vector2(0f, -220f),
            new Color(1f, 1f, 1f, 0.96f), ModernUIFactory.DarkPanel, ModernUIFactory.Blue);
        CreateSettingsIconBadge(themeCard.transform, ModernUIFactory.Icon.Theme);
        TMP_Text themeTitle = ModernUIFactory.CreateLocalizedText(
            "ThemeTitle", themeCard.transform, "theme", 40f,
            ModernUIFactory.Navy, Color.white, TextAlignmentOptions.TopLeft);
        ModernUIFactory.SetRect(themeTitle.rectTransform, new Vector2(570f, 58f), new Vector2(105f, 53f));
        Button lightButton = ModernUIFactory.CreateTextButton(
            "LightTheme", themeCard.transform, new Vector2(270f, 80f), new Vector2(-35f, -42f),
            "", ModernUIFactory.Blue, new Color(0.1f, 0.22f, 0.38f, 1f),
            Color.white, Color.white, () => ModernUITheme.SetDark(false), 34f);
        lightValue = lightButton.transform.Find("Label").GetComponent<TMP_Text>();
        Button darkButton = ModernUIFactory.CreateTextButton(
            "DarkTheme", themeCard.transform, new Vector2(270f, 80f), new Vector2(250f, -42f),
            "", new Color(0.9f, 0.95f, 1f, 1f), ModernUIFactory.Blue,
            ModernUIFactory.Navy, Color.white, () => ModernUITheme.SetDark(true), 34f);
        darkValue = darkButton.transform.Find("Label").GetComponent<TMP_Text>();

        CreateBackButton(settingsPage, () => ShowPage("main"));
        RefreshDynamicLabels();
    }

    private static void CreateSettingsIconBadge(Transform parent, ModernUIFactory.Icon icon)
    {
        Image badge = ModernUIFactory.CreatePanel(
            icon + "Badge", parent, new Vector2(130f, 130f), new Vector2(-320f, 0f),
            new Color(0.88f, 0.94f, 1f, 1f), new Color(0.04f, 0.2f, 0.38f, 1f));
        badge.GetComponent<Shadow>().effectDistance = new Vector2(0f, -4f);
        ModernUIFactory.CreateIcon("Icon", badge.transform, icon, new Vector2(94f, 94f), Vector2.zero);
    }

    private void CreateHeader(Transform parent, string key)
    {
        TMP_Text header = ModernUIFactory.CreateLocalizedText(
            "Header", parent, key, 82f,
            ModernUIFactory.Navy, Color.white);
        ModernUIFactory.SetRect(header.rectTransform, new Vector2(900f, 180f), new Vector2(0f, 650f));

        Image blueLine = ModernUIFactory.CreatePanel(
            "BlueAccent", parent, new Vector2(160f, 14f), new Vector2(-95f, 545f),
            ModernUIFactory.Blue, ModernUIFactory.Cyan);
        blueLine.GetComponent<Shadow>().enabled = false;
        Image orangeLine = ModernUIFactory.CreatePanel(
            "OrangeAccent", parent, new Vector2(120f, 14f), new Vector2(95f, 545f),
            ModernUIFactory.Orange, ModernUIFactory.Orange);
        orangeLine.GetComponent<Shadow>().enabled = false;
    }

    private void CreateChoiceCard(
        Transform parent,
        Vector2 position,
        string titleKey,
        string descriptionKey,
        ModernUIFactory.Icon icon,
        ModernUIFactory.Icon nextIcon,
        Color accent,
        UnityEngine.Events.UnityAction action,
        Vector2? customSize = null)
    {
        Vector2 size = customSize ?? new Vector2(850f, 290f);
        Image panel = ModernUIFactory.CreatePanel(
            titleKey + "Card", parent, size, position,
            new Color(1f, 1f, 1f, 0.96f), ModernUIFactory.DarkPanel, accent);
        panel.raycastTarget = true;
        Button button = panel.gameObject.AddComponent<Button>();
        button.targetGraphic = panel;
        button.onClick.AddListener(action);

        float iconSize = size.y > 260f ? 220f : 170f;
        float iconX = -size.x * 0.32f;
        Image iconBadge = ModernUIFactory.CreatePanel(
            "IconBadge", panel.transform, new Vector2(iconSize, iconSize), new Vector2(iconX, 0f),
            new Color(accent.r, accent.g, accent.b, 0.09f),
            new Color(accent.r, accent.g, accent.b, 0.18f));
        iconBadge.GetComponent<Shadow>().enabled = false;
        ModernUIFactory.CreateIcon(
            "Icon", iconBadge.transform, icon,
            new Vector2(iconSize * 0.88f, iconSize * 0.88f), Vector2.zero);

        Image divider = ModernUIFactory.CreateImage("Divider", panel.transform, null);
        divider.color = new Color(accent.r, accent.g, accent.b, 0.22f);
        ModernUIFactory.SetRect(divider.rectTransform, new Vector2(3f, size.y * 0.56f), new Vector2(-size.x * 0.17f, 0f));

        TMP_Text title = ModernUIFactory.CreateLocalizedText(
            "Title", panel.transform, titleKey, 54f,
            ModernUIFactory.Navy, Color.white, TextAlignmentOptions.Left);
        ModernUIFactory.SetRect(title.rectTransform, new Vector2(size.x * 0.47f, 80f), new Vector2(85f, 43f));

        TMP_Text description = ModernUIFactory.CreateLocalizedText(
            "Description", panel.transform, descriptionKey, 29f,
            ModernUIFactory.Muted, new Color(0.72f, 0.82f, 0.96f, 1f), TextAlignmentOptions.TopLeft, FontStyles.Normal);
        description.textWrappingMode = TextWrappingModes.Normal;
        description.overflowMode = TextOverflowModes.Truncate;
        ModernUIFactory.SetRect(description.rectTransform, new Vector2(size.x * 0.47f, 95f), new Vector2(85f, -45f));

        ModernUIFactory.CreateIcon(
            "Next", panel.transform, nextIcon, new Vector2(104f, 104f), new Vector2(size.x * 0.41f, 0f));
    }

    private void CreateBackButton(Transform parent, UnityEngine.Events.UnityAction action)
    {
        ModernUIFactory.CreateIconButton(
            "BackButton", parent, new Vector2(570f, 135f), new Vector2(0f, -720f),
            "back", ModernUIFactory.Icon.Back,
            new Color(1f, 1f, 1f, 0.97f), ModernUIFactory.DarkPanel,
            ModernUIFactory.Navy, Color.white, () => action(), ModernUIFactory.Blue, 82f, 45f);
    }

    private void CreateVolumeSlider(Transform parent)
    {
        Image track = ModernUIFactory.CreatePanel(
            "Track", parent, new Vector2(545f, 25f), new Vector2(105f, -45f),
            new Color(0.72f, 0.79f, 0.9f, 1f), new Color(0.25f, 0.36f, 0.53f, 1f));
        // CreatePanel uses non-interactive images by default. The slider needs
        // both its track and handle to receive pointer events when dragging.
        track.raycastTarget = true;
        track.GetComponent<Shadow>().enabled = false;

        Image fill = ModernUIFactory.CreatePanel(
            "Fill", track.transform, Vector2.zero, Vector2.zero,
            ModernUIFactory.Blue, ModernUIFactory.Cyan);
        fill.GetComponent<Shadow>().enabled = false;
        RectTransform fillRect = fill.rectTransform;
        fillRect.anchorMin = Vector2.zero;
        fillRect.anchorMax = Vector2.one;
        fillRect.offsetMin = Vector2.zero;
        fillRect.offsetMax = Vector2.zero;

        Image handle = ModernUIFactory.CreatePanel(
            "Handle", track.transform, new Vector2(62f, 62f), Vector2.zero,
            ModernUIFactory.Blue, ModernUIFactory.Cyan, Color.white);
        handle.raycastTarget = true;
        handle.GetComponent<Shadow>().effectDistance = new Vector2(0f, -4f);

        Slider slider = track.gameObject.AddComponent<Slider>();
        slider.fillRect = fillRect;
        slider.handleRect = handle.rectTransform;
        slider.targetGraphic = handle;
        slider.direction = Slider.Direction.LeftToRight;
        slider.minValue = 0f;
        slider.maxValue = 1f;
        slider.value = AudioListener.volume;
        slider.onValueChanged.AddListener(value => AudioListener.volume = value);
    }

    private void ShowPage(string page)
    {
        mainPage.gameObject.SetActive(page == "main");
        modePage.gameObject.SetActive(page == "mode");
        difficultyPage.gameObject.SetActive(page == "difficulty");
        settingsPage.gameObject.SetActive(page == "settings");
        RefreshDynamicLabels();
    }

    private void RefreshDynamicLabels()
    {
        if (languageValue != null)
            languageValue.text = ModernLocalization.CurrentLanguageName;
        if (lightValue != null)
            lightValue.text = ModernLocalization.Get("light");
        if (darkValue != null)
            darkValue.text = ModernLocalization.Get("dark");
    }

    private static void SelectDifficulty(BotDifficulty difficulty)
    {
        GameSettings.SelectedGameMode = GameMode.PlayerVsBot;
        GameSettings.SelectedBotDifficulty = difficulty;
        LoadGame();
    }

    private static void LoadGame()
    {
        if (SceneLoader.Instance != null)
            SceneLoader.Instance.LoadGame();
    }

    private static void ExitGame()
    {
        if (SceneLoader.Instance != null)
            SceneLoader.Instance.ExitGame();
    }

    private static void OpenPlayStore()
    {
#if UNITY_ANDROID
        Application.OpenURL("https://play.google.com/store/apps/details?id=com.mn7studios.tictactoe");
#endif
    }
}
