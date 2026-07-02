using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Music")]
    [SerializeField] private AudioClip mainMenuMusic;
    [SerializeField] private AudioClip gameplayMusic;

    [Header("Sound Effects")]
    [SerializeField] private AudioClip buttonClick;
    [SerializeField] private AudioClip placeX;
    [SerializeField] private AudioClip placeO;
    [SerializeField] private AudioClip win;
    [SerializeField] private AudioClip draw;
    [SerializeField] private AudioClip restart;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayMusic(AudioClip clip)
    {
        if (musicSource.clip == clip)
            return;

        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    public void PlayButton()
    {
        PlaySFX(buttonClick);
    }

    public void PlayX()
    {
        PlaySFX(placeX);
    }

    public void PlayO()
    {
        PlaySFX(placeO);
    }

    public void PlayWin()
    {
        PlaySFX(win);
    }

    public void PlayDraw()
    {
        PlaySFX(draw);
    }

    public void PlayRestart()
    {
        PlaySFX(restart);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        switch (scene.name)
        {
            case "MainMenu":
                PlayMusic(mainMenuMusic);
                break;

            case "Game":
                PlayMusic(gameplayMusic);
                break;
        }
    }

    public AudioClip MainMenuMusic => mainMenuMusic;
    public AudioClip GameplayMusic => gameplayMusic;
}