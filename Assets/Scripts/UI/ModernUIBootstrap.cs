using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public sealed class ModernUIBootstrap : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        if (FindFirstObjectByType<ModernUIBootstrap>() != null)
            return;

        GameObject bootstrap = new("ModernUIBootstrap", typeof(ModernUIBootstrap));
        DontDestroyOnLoad(bootstrap);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "MainMenu" && scene.name != "Game")
            return;

        HideLegacyCanvases();
        GameObject root = new("ModernUIRuntime");
        SceneManager.MoveGameObjectToScene(root, scene);

        if (scene.name == "MainMenu")
        {
            ModernMainMenuUI menu = root.AddComponent<ModernMainMenuUI>();
            menu.Build();
            return;
        }

        GameManager manager = FindFirstObjectByType<GameManager>();
        if (manager != null)
        {
            ModernGameUI gameUI = root.AddComponent<ModernGameUI>();
            gameUI.Build(manager);
        }
        else
        {
            Debug.LogError("Modern UI could not find GameManager in the Game scene.");
        }
    }

    private static void HideLegacyCanvases()
    {
        Canvas[] canvases = FindObjectsByType<Canvas>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (Canvas legacyCanvas in canvases)
        {
            if (legacyCanvas.name == "ModernUI")
                continue;

            legacyCanvas.enabled = false;
            GraphicRaycaster raycaster = legacyCanvas.GetComponent<GraphicRaycaster>();
            if (raycaster != null)
                raycaster.enabled = false;
        }
    }
}
