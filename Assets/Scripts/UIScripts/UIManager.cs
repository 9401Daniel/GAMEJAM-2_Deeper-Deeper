using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;


public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    public enum GameState
    {
        MainMenu,
        HowToPlay,
        Options,
        Credits,
        InGame,
        Paused,
        GameOver
    }

    [Header("Paneles (deben tener CanvasGroup)")]
    [SerializeField] private CanvasGroup mainMenuPanel;
    [SerializeField] private CanvasGroup howToPlayPanel;
    [SerializeField] private CanvasGroup optionsPanel;
    [SerializeField] private CanvasGroup creditsPanel;
    [SerializeField] private CanvasGroup inGamePanel;
    [SerializeField] private CanvasGroup pausedPanel;
    [SerializeField] private CanvasGroup gameOverPanel;

    [Header("Transition configuration")]
    [SerializeField] private float fadeDuration = 0.25f;

    [Header("Inicial State")]
    [SerializeField] private GameState initialState = GameState.MainMenu;

    // Evento para que otros sistemas (audio, gameplay) reaccionen a cambios de estado
    // sin que el UIManager necesite conocerlos directamente.
    public static event Action<GameState> OnStateChanged;

    private Dictionary<GameState, CanvasGroup> panels;
    private GameState currentState;
    private Coroutine transitionRoutine;
    // Dependencias
    public Timer timer;
    public PlayerClickMove playerMovement;
    public EnemySpawner spawner;
    public MoveUp background;

    public PlayerForms playerForms;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;


    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        panels = new Dictionary<GameState, CanvasGroup>
        {
            { GameState.MainMenu, mainMenuPanel },
            { GameState.HowToPlay, howToPlayPanel },
            { GameState.Options,  optionsPanel },
            { GameState.Credits,  creditsPanel },
            { GameState.InGame,   inGamePanel },
            { GameState.Paused,   pausedPanel },
            { GameState.GameOver, gameOverPanel }
        };
    }

    void Start()
    {
        // Desactiva todos los paneles menos el inicial, sin animación.
        foreach (var kvp in panels)
        {
            bool isInitial = kvp.Key == initialState;
            SetPanelInstant(kvp.Value, isInitial);
        }

        currentState = initialState;
        OnStateChanged?.Invoke(currentState);
    }

    void Update()
    {
        // Pausa/reanuda con Escape, además del botón de pausa en el HUD.
        // Solo actúa si estamos jugando o pausados; en menús no hace nada.
        // Nota: usamos Keyboard.current (nuevo Input System) porque el proyecto
        // tiene Active Input Handling = "Input System Package (New)", donde
        // Input.GetKeyDown (el Input Manager viejo) ya no funciona.
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (currentState == GameState.InGame)
                ShowPaused();
            else if (currentState == GameState.Paused)
                ShowInGame();
        }
    }

    private void DestroyEnemies()
    {
        EnemyBase[] enemies = FindObjectsByType<EnemyBase>(FindObjectsSortMode.None);
        foreach (EnemyBase enemy in enemies)
        {
            Destroy(enemy.gameObject);
        }
    }

    // ----- Métodos públicos para conectar desde botones (OnClick en el Inspector) -----

    public void ShowMainMenu()
    {
        ChangeState(GameState.MainMenu);
        Time.timeScale = 1f;
        playerForms.ResetPlayer();
        playerMovement.SetPlayerStart(false);
        background.SetIsPlaying(false);
        background.ResetPosition();
        spawner.ResetSpawn();
        DestroyEnemies();
        timer.StopTimer();
    }

    public void ShowHowToPlay() => ChangeState(GameState.HowToPlay);
    public void ShowOptions() => ChangeState(GameState.Options);
    public void ShowCredits() => ChangeState(GameState.Credits);
    public void ShowInGame()
    {
        ChangeState(GameState.InGame);
        if (!timer.Activo) timer.InitTimer();
        Time.timeScale = 1f;
        if (playerForms.IsGameOver)
        {
            playerForms.ResetPlayer();
            spawner.ResetSpawn();
            background.ResetPosition();
        }
        playerMovement.SetPlayerStart(true);
        spawner.StartSpawn(true);
        background.SetIsPlaying(true);
    }
    public void ShowPaused()
    {
        ChangeState(GameState.Paused);
        Time.timeScale = 0f;
    }
    public void ShowGameOver()
    {
        ChangeState(GameState.GameOver);
        spawner.ResetSpawn();
        DestroyEnemies();
        timer.StopTimer();
        playerMovement.SetPlayerStart(false);
        background.SetIsPlaying(false);
        //Falta guardar y mostrar el score en el panel de GameOver
        SaveScore.Instance.SaveHighScore(timer.formatTime);
        scoreText.text = "Final score: " + timer.formatTime;
        highScoreText.text = "High score: " + SaveScore.Instance.GetHighScore();
    }

    public void ChangeState(GameState next)
    {
        if (next == currentState) return;
        if (!panels.ContainsKey(next) || panels[next] == null)
        {
            Debug.LogWarning($"UIManager: No hay UI asignada para el estado {next}");
            return;
        }
        if (transitionRoutine != null)
            StopCoroutine(transitionRoutine);

        transitionRoutine = StartCoroutine(TransitionTo(next));
    }

    // ----- Transición con fade -----

    private IEnumerator TransitionTo(GameState next)
    {
        CanvasGroup from = panels.ContainsKey(currentState) ? panels[currentState] : null;
        CanvasGroup to = panels[next];

        // Bloquea interacción durante el fade para evitar clics dobles
        if (from != null) from.blocksRaycasts = false;
        to.gameObject.SetActive(true);
        to.blocksRaycasts = false;

        float t = 0f;
        float fromStartAlpha = from != null ? from.alpha : 0f;

        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime; // unscaled: funciona aunque el juego esté en pausa (Time.timeScale = 0)
            float p = Mathf.Clamp01(t / fadeDuration);

            if (from != null) from.alpha = Mathf.Lerp(fromStartAlpha, 0f, p);
            to.alpha = Mathf.Lerp(0f, 1f, p);

            yield return null;
        }

        if (from != null)
        {
            from.alpha = 0f;
            from.gameObject.SetActive(false);
        }

        to.alpha = 1f;
        to.interactable = true;
        to.blocksRaycasts = true;

        currentState = next;
        OnStateChanged?.Invoke(currentState);
        transitionRoutine = null;
    }

    private void SetPanelInstant(CanvasGroup panel, bool active)
    {
        if (panel == null) return;
        panel.gameObject.SetActive(active);
        panel.alpha = active ? 1f : 0f;
        panel.interactable = active;
        panel.blocksRaycasts = active;
    }

    public GameState CurrentState => currentState;
}