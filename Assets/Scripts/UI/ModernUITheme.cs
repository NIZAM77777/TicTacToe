using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public static class ModernUITheme
{
    private const string ThemeKey = "DarkMode";
    private static bool isDark = PlayerPrefs.GetInt(ThemeKey, 0) == 1;

    public static event Action ThemeChanged;

    public static bool IsDark => isDark;

    public static void SetDark(bool value)
    {
        if (isDark == value)
            return;

        isDark = value;
        PlayerPrefs.SetInt(ThemeKey, value ? 1 : 0);
        PlayerPrefs.Save();
        ThemeChanged?.Invoke();
    }

    public static void Toggle()
    {
        SetDark(!isDark);
    }
}

public sealed class ThemeGraphic : MonoBehaviour
{
    private Graphic graphic;
    private Color lightColor;
    private Color darkColor;

    public void Configure(Graphic target, Color light, Color dark)
    {
        graphic = target;
        lightColor = light;
        darkColor = dark;
        Refresh();
    }

    private void OnEnable()
    {
        ModernUITheme.ThemeChanged += Refresh;
        Refresh();
    }

    private void OnDisable()
    {
        ModernUITheme.ThemeChanged -= Refresh;
    }

    private void Refresh()
    {
        if (graphic != null)
            graphic.color = ModernUITheme.IsDark ? darkColor : lightColor;
    }
}

public sealed class ThemeBackground : MonoBehaviour
{
    private Image image;
    private Sprite lightSprite;
    private Sprite darkSprite;

    public void Configure(Image target, Sprite light, Sprite dark)
    {
        image = target;
        lightSprite = light;
        darkSprite = dark;
        Refresh();
    }

    private void OnEnable()
    {
        ModernUITheme.ThemeChanged += Refresh;
        Refresh();
    }

    private void OnDisable()
    {
        ModernUITheme.ThemeChanged -= Refresh;
    }

    private void Refresh()
    {
        if (image != null)
            image.sprite = ModernUITheme.IsDark ? darkSprite : lightSprite;
    }
}
