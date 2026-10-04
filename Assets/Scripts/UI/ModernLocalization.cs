using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum AppLanguage
{
    English,
    Spanish,
    French,
    German,
    Portuguese
}

public static class ModernLocalization
{
    private const string LanguageKey = "AppLanguage";

    private static readonly Dictionary<AppLanguage, string> LanguageNames = new()
    {
        { AppLanguage.English, "English" },
        { AppLanguage.Spanish, "Español" },
        { AppLanguage.French, "Français" },
        { AppLanguage.German, "Deutsch" },
        { AppLanguage.Portuguese, "Português" }
    };

    private static readonly Dictionary<AppLanguage, Dictionary<string, string>> Tables = new()
    {
        {
            AppLanguage.English, new Dictionary<string, string>
            {
                { "play", "PLAY" }, { "settings", "SETTINGS" }, { "exit", "EXIT" },
                { "select_mode", "SELECT GAME MODE" }, { "vs_human", "VS HUMAN" },
                { "vs_human_desc", "Play with a friend on the same device" },
                { "vs_bot", "VS BOT" }, { "vs_bot_desc", "Play against computer AI" },
                { "back", "BACK" }, { "select_difficulty", "SELECT DIFFICULTY" },
                { "easy", "EASY" }, { "easy_desc", "Relaxed and fun" },
                { "medium", "MEDIUM" }, { "medium_desc", "A balanced challenge" },
                { "hard", "HARD" }, { "hard_desc", "For experienced players" },
                { "music", "MUSIC" }, { "language", "LANGUAGE" }, { "theme", "THEME" },
                { "light", "LIGHT" }, { "dark", "DARK" }, { "rate_us", "RATE US" },
                { "player_turn", "Player {0} Turn" }, { "restart", "Restart" }, { "home", "Home" },
                { "player_wins", "PLAYER {0} WINS!" }, { "congratulations", "Congratulations!" },
                { "you_lose", "YOU LOSE!" }, { "better_luck", "Better luck next time!" },
                { "draw", "DRAW!" }, { "draw_desc", "Great game!" }
            }
        },
        {
            AppLanguage.Spanish, new Dictionary<string, string>
            {
                { "play", "JUGAR" }, { "settings", "AJUSTES" }, { "exit", "SALIR" },
                { "select_mode", "ELIGE EL MODO" }, { "vs_human", "CON AMIGO" },
                { "vs_human_desc", "Juega con un amigo en el mismo dispositivo" },
                { "vs_bot", "CON BOT" }, { "vs_bot_desc", "Juega contra la computadora" },
                { "back", "VOLVER" }, { "select_difficulty", "ELIGE DIFICULTAD" },
                { "easy", "FÁCIL" }, { "easy_desc", "Relajado y divertido" },
                { "medium", "MEDIO" }, { "medium_desc", "Un reto equilibrado" },
                { "hard", "DIFÍCIL" }, { "hard_desc", "Para jugadores expertos" },
                { "music", "MÚSICA" }, { "language", "IDIOMA" }, { "theme", "TEMA" },
                { "light", "CLARO" }, { "dark", "OSCURO" }, { "rate_us", "CALIFÍCANOS" },
                { "player_turn", "TURNO DE {0}" }, { "restart", "REINICIAR" }, { "home", "INICIO" },
                { "player_wins", "¡GANA {0}!" }, { "congratulations", "¡Felicidades!" },
                { "you_lose", "¡PERDISTE!" }, { "better_luck", "¡Más suerte la próxima vez!" },
                { "draw", "¡EMPATE!" }, { "draw_desc", "¡Gran partida!" }
            }
        },
        {
            AppLanguage.French, new Dictionary<string, string>
            {
                { "play", "JOUER" }, { "settings", "RÉGLAGES" }, { "exit", "QUITTER" },
                { "select_mode", "CHOISIR LE MODE" }, { "vs_human", "AVEC UN AMI" },
                { "vs_human_desc", "Jouez à deux sur le même appareil" },
                { "vs_bot", "CONTRE L'IA" }, { "vs_bot_desc", "Jouez contre l'ordinateur" },
                { "back", "RETOUR" }, { "select_difficulty", "CHOISIR LA DIFFICULTÉ" },
                { "easy", "FACILE" }, { "easy_desc", "Détendu et amusant" },
                { "medium", "MOYEN" }, { "medium_desc", "Un défi équilibré" },
                { "hard", "DIFFICILE" }, { "hard_desc", "Pour joueurs expérimentés" },
                { "music", "MUSIQUE" }, { "language", "LANGUE" }, { "theme", "THÈME" },
                { "light", "CLAIR" }, { "dark", "SOMBRE" }, { "rate_us", "NOTER LE JEU" },
                { "player_turn", "TOUR DU JOUEUR {0}" }, { "restart", "REJOUER" }, { "home", "ACCUEIL" },
                { "player_wins", "JOUEUR {0} GAGNE !" }, { "congratulations", "Félicitations !" },
                { "you_lose", "PERDU !" }, { "better_luck", "Bonne chance la prochaine fois !" },
                { "draw", "ÉGALITÉ !" }, { "draw_desc", "Belle partie !" }
            }
        },
        {
            AppLanguage.German, new Dictionary<string, string>
            {
                { "play", "SPIELEN" }, { "settings", "EINSTELLUNGEN" }, { "exit", "BEENDEN" },
                { "select_mode", "SPIELMODUS WÄHLEN" }, { "vs_human", "GEGEN FREUND" },
                { "vs_human_desc", "Spielt gemeinsam auf einem Gerät" },
                { "vs_bot", "GEGEN BOT" }, { "vs_bot_desc", "Spiele gegen den Computer" },
                { "back", "ZURÜCK" }, { "select_difficulty", "SCHWIERIGKEIT WÄHLEN" },
                { "easy", "LEICHT" }, { "easy_desc", "Entspannt und unterhaltsam" },
                { "medium", "MITTEL" }, { "medium_desc", "Eine ausgewogene Herausforderung" },
                { "hard", "SCHWER" }, { "hard_desc", "Für erfahrene Spieler" },
                { "music", "MUSIK" }, { "language", "SPRACHE" }, { "theme", "DESIGN" },
                { "light", "HELL" }, { "dark", "DUNKEL" }, { "rate_us", "BEWERTEN" },
                { "player_turn", "SPIELER {0} IST DRAN" }, { "restart", "NEUSTART" }, { "home", "START" },
                { "player_wins", "SPIELER {0} GEWINNT!" }, { "congratulations", "Glückwunsch!" },
                { "you_lose", "VERLOREN!" }, { "better_luck", "Mehr Glück beim nächsten Mal!" },
                { "draw", "UNENTSCHIEDEN!" }, { "draw_desc", "Tolles Spiel!" }
            }
        },
        {
            AppLanguage.Portuguese, new Dictionary<string, string>
            {
                { "play", "JOGAR" }, { "settings", "CONFIGURAÇÕES" }, { "exit", "SAIR" },
                { "select_mode", "ESCOLHA O MODO" }, { "vs_human", "COM AMIGO" },
                { "vs_human_desc", "Jogue com um amigo no mesmo aparelho" },
                { "vs_bot", "CONTRA BOT" }, { "vs_bot_desc", "Jogue contra o computador" },
                { "back", "VOLTAR" }, { "select_difficulty", "ESCOLHA A DIFICULDADE" },
                { "easy", "FÁCIL" }, { "easy_desc", "Relaxado e divertido" },
                { "medium", "MÉDIO" }, { "medium_desc", "Um desafio equilibrado" },
                { "hard", "DIFÍCIL" }, { "hard_desc", "Para jogadores experientes" },
                { "music", "MÚSICA" }, { "language", "IDIOMA" }, { "theme", "TEMA" },
                { "light", "CLARO" }, { "dark", "ESCURO" }, { "rate_us", "AVALIAR" },
                { "player_turn", "VEZ DO JOGADOR {0}" }, { "restart", "REINICIAR" }, { "home", "INÍCIO" },
                { "player_wins", "JOGADOR {0} VENCEU!" }, { "congratulations", "Parabéns!" },
                { "you_lose", "VOCÊ PERDEU!" }, { "better_luck", "Mais sorte na próxima!" },
                { "draw", "EMPATE!" }, { "draw_desc", "Ótima partida!" }
            }
        }
    };

    private static AppLanguage currentLanguage = LoadLanguage();

    public static event Action LanguageChanged;

    public static AppLanguage CurrentLanguage => currentLanguage;
    public static int LanguageCount => Enum.GetValues(typeof(AppLanguage)).Length;

    public static string Get(string key)
    {
        if (Tables[currentLanguage].TryGetValue(key, out string value))
            return value;

        return Tables[AppLanguage.English].TryGetValue(key, out value) ? value : key;
    }

    public static string Format(string key, params object[] args)
    {
        return string.Format(Get(key), args);
    }

    public static string CurrentLanguageName => LanguageNames[currentLanguage];

    public static void Cycle(int direction)
    {
        int count = LanguageCount;
        int next = ((int)currentLanguage + direction) % count;
        if (next < 0)
            next += count;

        SetLanguage((AppLanguage)next);
    }

    public static void SetLanguage(AppLanguage language)
    {
        if (currentLanguage == language)
            return;

        currentLanguage = language;
        PlayerPrefs.SetInt(LanguageKey, (int)language);
        PlayerPrefs.Save();
        LanguageChanged?.Invoke();
    }

    private static AppLanguage LoadLanguage()
    {
        int value = PlayerPrefs.GetInt(LanguageKey, (int)AppLanguage.English);
        return Enum.IsDefined(typeof(AppLanguage), value) ? (AppLanguage)value : AppLanguage.English;
    }
}

public sealed class LocalizedLabel : MonoBehaviour
{
    private TMP_Text label;
    private string localizationKey;

    public void Configure(TMP_Text target, string key)
    {
        label = target;
        localizationKey = key;
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
        if (label != null && !string.IsNullOrEmpty(localizationKey))
            label.text = ModernLocalization.Get(localizationKey);
    }
}
