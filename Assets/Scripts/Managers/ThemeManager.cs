using UnityEngine;
using UnityEngine.UI;

public class ImageManager : MonoBehaviour
{
    [SerializeField] private Image backgroundImage;

    [SerializeField] private Sprite lightBackground;
    [SerializeField] private Sprite darkBackground;

    [SerializeField] private Toggle themeToggle;

    private const string ThemeKey = "DarkMode";

    private void Start()
    {
        // Load saved theme
        bool isDark = PlayerPrefs.GetInt(ThemeKey, 0) == 1;

        // Set toggle without triggering the event
        themeToggle.SetIsOnWithoutNotify(isDark);

        // Apply background
        backgroundImage.sprite = isDark
            ? darkBackground
            : lightBackground;
    }

    public void ChangeBackground(bool isDark)
    {
        Debug.Log("Toggle: " + isDark);

        // Save theme
        PlayerPrefs.SetInt(ThemeKey, isDark ? 1 : 0);
        PlayerPrefs.Save();

        // Change background
        backgroundImage.sprite = isDark
            ? darkBackground
            : lightBackground;
    }
}