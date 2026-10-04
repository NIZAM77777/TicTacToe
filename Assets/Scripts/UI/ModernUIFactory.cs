using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public static class ModernUIFactory
{
    public static readonly Color Navy = new(0.025f, 0.105f, 0.25f, 1f);
    public static readonly Color Blue = new(0.02f, 0.44f, 1f, 1f);
    public static readonly Color Cyan = new(0.02f, 0.74f, 1f, 1f);
    public static readonly Color Orange = new(1f, 0.35f, 0.02f, 1f);
    public static readonly Color Red = new(1f, 0.12f, 0.22f, 1f);
    public static readonly Color Muted = new(0.38f, 0.47f, 0.64f, 1f);
    public static readonly Color DarkPanel = new(0.025f, 0.13f, 0.27f, 0.94f);

    private static Sprite roundedSprite;
    private static Texture2D iconSheet;
    private static TMP_FontAsset uiFontAsset;
    private static readonly Sprite[] iconSprites = new Sprite[16];

    public enum Icon
    {
        Play,
        Settings,
        Exit,
        Back,
        Human,
        Bot,
        Easy,
        Medium,
        Hard,
        Music,
        Language,
        Theme,
        Restart,
        Home,
        NextBlue,
        NextOrange
    }

    public static Canvas CreateCanvas(string name)
    {
        GameObject canvasObject = new(name, typeof(RectTransform), typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
        Canvas canvas = canvasObject.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 100;

        CanvasScaler scaler = canvasObject.GetComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1080f, 1920f);
        scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        scaler.matchWidthOrHeight = 0.5f;

        EnsureEventSystem();
        return canvas;
    }

    public static RectTransform CreateContainer(string name, Transform parent)
    {
        GameObject gameObject = new(name, typeof(RectTransform));
        RectTransform rect = gameObject.GetComponent<RectTransform>();
        rect.SetParent(parent, false);
        Stretch(rect);
        return rect;
    }

    public static Image CreateImage(string name, Transform parent, Sprite sprite, bool preserveAspect = false)
    {
        GameObject gameObject = new(name, typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
        RectTransform rect = gameObject.GetComponent<RectTransform>();
        rect.SetParent(parent, false);
        Image image = gameObject.GetComponent<Image>();
        image.sprite = sprite;
        image.preserveAspect = preserveAspect;
        image.raycastTarget = false;
        return image;
    }

    public static Image CreateIcon(
        string name,
        Transform parent,
        Icon icon,
        Vector2 size,
        Vector2 position,
        float rotation = 0f)
    {
        Image image = CreateImage(name, parent, LoadIcon(icon), true);
        SetRect(image.rectTransform, size, position);
        image.rectTransform.localRotation = Quaternion.Euler(0f, 0f, rotation);
        return image;
    }

    public static Image CreatePanel(
        string name,
        Transform parent,
        Vector2 size,
        Vector2 position,
        Color lightColor,
        Color darkColor,
        Color? outlineColor = null)
    {
        Image image = CreateImage(name, parent, RoundedSprite);
        image.type = Image.Type.Sliced;
        SetRect(image.rectTransform, size, position);
        image.color = ModernUITheme.IsDark ? darkColor : lightColor;

        ThemeGraphic theme = image.gameObject.AddComponent<ThemeGraphic>();
        theme.Configure(image, lightColor, darkColor);

        Shadow shadow = image.gameObject.AddComponent<Shadow>();
        shadow.effectColor = new Color(0.03f, 0.18f, 0.4f, ModernUITheme.IsDark ? 0.3f : 0.16f);
        shadow.effectDistance = new Vector2(0f, -10f);

        if (outlineColor.HasValue)
        {
            Outline outline = image.gameObject.AddComponent<Outline>();
            outline.effectColor = outlineColor.Value;
            outline.effectDistance = new Vector2(3f, -3f);
        }

        return image;
    }

    public static TMP_Text CreateText(
        string name,
        Transform parent,
        string text,
        float fontSize,
        Color lightColor,
        Color darkColor,
        TextAlignmentOptions alignment = TextAlignmentOptions.Center,
        FontStyles style = FontStyles.Bold)
    {
        GameObject gameObject = new(name, typeof(RectTransform), typeof(CanvasRenderer), typeof(TextMeshProUGUI));
        RectTransform rect = gameObject.GetComponent<RectTransform>();
        rect.SetParent(parent, false);

        TMP_Text label = gameObject.GetComponent<TMP_Text>();
        label.text = text;
        label.fontSize = fontSize;
        label.fontStyle = style;
        label.alignment = alignment;
        label.enableAutoSizing = true;
        label.fontSizeMin = Mathf.Max(18f, fontSize * 0.55f);
        label.fontSizeMax = fontSize;
        label.overflowMode = TextOverflowModes.Ellipsis;
        label.raycastTarget = false;
        if (UIFontAsset != null)
            label.font = UIFontAsset;
        label.color = ModernUITheme.IsDark ? darkColor : lightColor;

        ThemeGraphic theme = gameObject.AddComponent<ThemeGraphic>();
        theme.Configure(label, lightColor, darkColor);
        return label;
    }

    public static TMP_Text CreateLocalizedText(
        string name,
        Transform parent,
        string key,
        float fontSize,
        Color lightColor,
        Color darkColor,
        TextAlignmentOptions alignment = TextAlignmentOptions.Center,
        FontStyles style = FontStyles.Bold)
    {
        TMP_Text label = CreateText(name, parent, ModernLocalization.Get(key), fontSize, lightColor, darkColor, alignment, style);
        LocalizedLabel localized = label.gameObject.AddComponent<LocalizedLabel>();
        localized.Configure(label, key);
        return label;
    }

    public static Button CreateButton(
        string name,
        Transform parent,
        Vector2 size,
        Vector2 position,
        string localizationKey,
        Color lightColor,
        Color darkColor,
        Color textLight,
        Color textDark,
        Action onClick,
        Color? outlineColor = null,
        string prefix = "")
    {
        Image image = CreatePanel(name, parent, size, position, lightColor, darkColor, outlineColor);
        image.raycastTarget = true;

        Button button = image.gameObject.AddComponent<Button>();
        button.targetGraphic = image;
        button.transition = Selectable.Transition.ColorTint;
        ColorBlock colors = button.colors;
        colors.normalColor = Color.white;
        colors.highlightedColor = new Color(0.9f, 0.96f, 1f, 1f);
        colors.pressedColor = new Color(0.75f, 0.88f, 1f, 1f);
        colors.selectedColor = colors.highlightedColor;
        colors.disabledColor = new Color(0.6f, 0.65f, 0.72f, 0.6f);
        colors.colorMultiplier = 1f;
        button.colors = colors;
        if (onClick != null)
            button.onClick.AddListener(() => onClick());

        TMP_Text label = string.IsNullOrEmpty(prefix)
            ? CreateLocalizedText("Label", image.transform, localizationKey, 54f, textLight, textDark)
            : CreateText("Label", image.transform, prefix + ModernLocalization.Get(localizationKey), 54f, textLight, textDark);
        Stretch(label.rectTransform, 32f, 26f);
        if (!string.IsNullOrEmpty(prefix))
        {
            PrefixLocalizedLabel prefixLabel = label.gameObject.AddComponent<PrefixLocalizedLabel>();
            prefixLabel.Configure(label, localizationKey, prefix);
        }

        return button;
    }

    public static Button CreateIconButton(
        string name,
        Transform parent,
        Vector2 size,
        Vector2 position,
        string localizationKey,
        Icon icon,
        Color lightColor,
        Color darkColor,
        Color textLight,
        Color textDark,
        Action onClick,
        Color? outlineColor = null,
        float iconSize = 100f,
        float fontSize = 50f)
    {
        Image image = CreatePanel(name, parent, size, position, lightColor, darkColor, outlineColor);
        image.raycastTarget = true;

        Button button = image.gameObject.AddComponent<Button>();
        button.targetGraphic = image;
        button.transition = Selectable.Transition.ColorTint;
        ColorBlock colors = button.colors;
        colors.normalColor = Color.white;
        colors.highlightedColor = new Color(0.93f, 0.97f, 1f, 1f);
        colors.pressedColor = new Color(0.78f, 0.88f, 1f, 1f);
        colors.selectedColor = colors.highlightedColor;
        button.colors = colors;
        if (onClick != null)
            button.onClick.AddListener(() => onClick());

        float iconX = -size.x * 0.31f;
        CreateIcon("Icon", image.transform, icon, new Vector2(iconSize, iconSize), new Vector2(iconX, 0f));

        Image divider = CreateImage("Divider", image.transform, RoundedSprite);
        divider.type = Image.Type.Sliced;
        divider.color = new Color(textLight.r, textLight.g, textLight.b, 0.16f);
        SetRect(divider.rectTransform, new Vector2(3f, size.y * 0.54f), new Vector2(-size.x * 0.14f, 0f));

        TMP_Text label = CreateLocalizedText(
            "Label", image.transform, localizationKey, fontSize,
            textLight, textDark, TextAlignmentOptions.Center);
        RectTransform labelRect = label.rectTransform;
        SetRect(labelRect, new Vector2(size.x * 0.63f, size.y - 24f), new Vector2(size.x * 0.14f, 0f));
        return button;
    }

    public static Button CreateTextButton(
        string name,
        Transform parent,
        Vector2 size,
        Vector2 position,
        string text,
        Color lightColor,
        Color darkColor,
        Color textLight,
        Color textDark,
        Action onClick,
        float fontSize = 44f)
    {
        Image image = CreatePanel(name, parent, size, position, lightColor, darkColor);
        image.raycastTarget = true;
        Button button = image.gameObject.AddComponent<Button>();
        button.targetGraphic = image;
        if (onClick != null)
            button.onClick.AddListener(() => onClick());
        TMP_Text label = CreateText("Label", image.transform, text, fontSize, textLight, textDark);
        Stretch(label.rectTransform, 20f, 16f);
        return button;
    }

    public static Sprite LoadSprite(string resourcePath)
    {
        Sprite sprite = Resources.Load<Sprite>(resourcePath);
        if (sprite != null)
            return sprite;

        Texture2D texture = Resources.Load<Texture2D>(resourcePath);
        if (texture == null)
            return null;

        return Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100f);
    }

    public static Sprite LoadIcon(Icon icon)
    {
        int index = (int)icon;
        if (iconSprites[index] != null)
            return iconSprites[index];

        if (iconSheet == null)
            iconSheet = Resources.Load<Texture2D>("ModernUI/IconSheet");
        if (iconSheet == null)
            return null;

        const int columns = 4;
        const int rows = 4;
        float cellWidth = iconSheet.width / (float)columns;
        float cellHeight = iconSheet.height / (float)rows;
        int rowFromTop = index / columns;
        int column = index % columns;
        Rect rect = new(
            column * cellWidth,
            iconSheet.height - ((rowFromTop + 1) * cellHeight),
            cellWidth,
            cellHeight);
        iconSprites[index] = Sprite.Create(
            iconSheet,
            rect,
            new Vector2(0.5f, 0.5f),
            100f,
            0,
            SpriteMeshType.FullRect);
        iconSprites[index].name = "ModernUIIcon_" + icon;
        return iconSprites[index];
    }

    public static void SetRect(RectTransform rect, Vector2 size, Vector2 position)
    {
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.sizeDelta = size;
        rect.anchoredPosition = position;
        rect.localScale = Vector3.one;
    }

    public static void Stretch(RectTransform rect, float horizontalInset = 0f, float verticalInset = 0f)
    {
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = new Vector2(horizontalInset, verticalInset);
        rect.offsetMax = new Vector2(-horizontalInset, -verticalInset);
        rect.localScale = Vector3.one;
    }

    private static Sprite RoundedSprite
    {
        get
        {
            if (roundedSprite != null)
                return roundedSprite;

            const int size = 64;
            const float radius = 17f;
            Texture2D texture = new(size, size, TextureFormat.RGBA32, false)
            {
                name = "RuntimeRoundedRectangle",
                filterMode = FilterMode.Bilinear,
                wrapMode = TextureWrapMode.Clamp,
                hideFlags = HideFlags.HideAndDontSave
            };

            Color[] pixels = new Color[size * size];
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    float dx = Mathf.Max(radius - x - 0.5f, 0f, x + 0.5f - (size - radius));
                    float dy = Mathf.Max(radius - y - 0.5f, 0f, y + 0.5f - (size - radius));
                    float distance = Mathf.Sqrt(dx * dx + dy * dy);
                    float alpha = Mathf.Clamp01(radius + 0.5f - distance);
                    pixels[y * size + x] = new Color(1f, 1f, 1f, alpha);
                }
            }

            texture.SetPixels(pixels);
            texture.Apply(false, true);
            roundedSprite = Sprite.Create(
                texture,
                new Rect(0f, 0f, size, size),
                new Vector2(0.5f, 0.5f),
                100f,
                0,
                SpriteMeshType.FullRect,
                new Vector4(radius, radius, radius, radius));
            roundedSprite.name = "RuntimeRoundedRectangleSprite";
            return roundedSprite;
        }
    }

    private static TMP_FontAsset UIFontAsset
    {
        get
        {
            if (uiFontAsset != null)
                return uiFontAsset;

            Font sourceFont = Resources.Load<Font>("ModernUI/Fredoka");
            if (sourceFont != null)
            {
                uiFontAsset = TMP_FontAsset.CreateFontAsset(sourceFont);
                uiFontAsset.atlasPopulationMode = AtlasPopulationMode.Dynamic;
                uiFontAsset.name = "Fredoka Runtime SDF";
                uiFontAsset.hideFlags = HideFlags.HideAndDontSave;
            }
            else
            {
                uiFontAsset = TMP_Settings.defaultFontAsset;
            }

            return uiFontAsset;
        }
    }

    private static void EnsureEventSystem()
    {
        if (UnityEngine.Object.FindFirstObjectByType<EventSystem>() != null)
            return;

        new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
    }
}

public sealed class PrefixLocalizedLabel : MonoBehaviour
{
    private TMP_Text label;
    private string key;
    private string prefix;

    public void Configure(TMP_Text target, string localizationKey, string labelPrefix)
    {
        label = target;
        key = localizationKey;
        prefix = labelPrefix;
        Refresh();
    }

    private void OnEnable()
    {
        ModernLocalization.LanguageChanged += Refresh;
        Refresh();
    }

    private void OnDisable()
    {
        ModernLocalization.LanguageChanged -= Refresh;
    }

    private void Refresh()
    {
        if (label != null)
            label.text = prefix + ModernLocalization.Get(key);
    }
}
