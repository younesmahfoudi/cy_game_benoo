using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameStatus { PLAY, POSTUNPAUSEDELAY, PAUSE, GAMEOVER, EXIT }
public class LevelManager : MonoBehaviour
{
    public List<AudioClip> ambianceSounds;
    public AudioClip gameOverSound;
    public GameObject PauseMenu;
    public GameObject LoseMenu;
    public GameObject WinMenu;
    public HUDManager HUDGO;
    public int gameMinutes = 3;
    public int gameSeconds = 0;
    public GameObject fadeGO;
    public GameStatus gameStatus = GameStatus.PLAY;
    private float _second = 1.0f;
    private bool _retryLevel = false;
    private int _ambianceSoundsFrequencyPercent = 10;
    private bool _isGameWon = false;
    public AudioClip MusicLevel;
    private int _ennemiesLayers = 0;

    private System.Random _random = ServiceProvider.random;

    void Start()
    {
        if (MusicLevel != null)
            SoundManager.Instance.PlayMusic(MusicLevel);
        PauseMenu.GetComponent<Animator>().SetBool("Paused", false);
        fadeGO.SetActive(true);
        HUDGO.Init(GameSaveInstance.Instance.gemCount, gameMinutes, gameSeconds);
        ChangeGameStatus(GameStatus.PLAY);
    }

    void Update()
    {
        if (gameStatus == GameStatus.POSTUNPAUSEDELAY && PauseMenu.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("GoOutScreen") && PauseMenu.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9)
            UnpauseGame();
        if (gameStatus == GameStatus.EXIT && fadeGO.GetComponent<GDTFadeEffect>().HasFinished())
        {
            if (_retryLevel)
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            else
                SceneManager.LoadScene(LevelNames.MainMenu);
        }
        else if (gameStatus == GameStatus.PLAY)
        {
            if (InputManager.Instance.GetcurrentKey(onlyPressed: true) == InputNames.pause)
                ChangeGameStatus(GameStatus.PAUSE);
            if (TimerCheck())
                ChangeGameStatus(GameStatus.GAMEOVER);
        }
    }

    //Check for the decrease of timer and ambiance sound random event
    public bool TimerCheck()
    {
        _second -= Time.deltaTime;
        if (_second <= 0)
        {
            _second = 1;
            if (_random.Next(100) < _ambianceSoundsFrequencyPercent)
                SoundManager.Instance.Play(ambianceSounds[_random.Next(ambianceSounds.Count)], SoundManager.SoundPriority.NORMAL);
            return HUDGO.DecreaseTimer();
        }
        return false;
    }

    public void IncreaseKillCounter()
    {
        HUDGO.IncreaseKillCounter();
    }

    public void IncreaseGemCounter()
    {
        HUDGO.IncreaseGemCounter();
    }

    public void ChangeGameStatus(GameStatus gs)
    {
        gameStatus = gs;
        switch (gs)
        {
            case GameStatus.PLAY:
            case GameStatus.EXIT:
                Time.timeScale = 1;
                break;
            case GameStatus.PAUSE:
                Time.timeScale = 0;
                PauseMenu.GetComponent<Animator>().SetBool("Paused", true);
                break;
            case GameStatus.GAMEOVER:
                //save score and gems
                GameSaveInstance.Instance.gemCount = HUDGO.GetGems();
                GameSaveInstance.Instance.AddScore(HUDGO.GetScore(), SceneManager.GetActiveScene().name);
                

                //if timer is at 0, that means the player wasn't killed
                if (HUDGO.DecreaseTimer())
                {
                    _isGameWon = true;
                    StartCoroutine(WinTransition());
                }
                else
                    StartCoroutine(LoseTransition());
                    break;
            case GameStatus.POSTUNPAUSEDELAY:
                PauseMenu.GetComponent<Animator>().SetBool("Paused", false);
                break;
            default:
                break;
        }
    }

    public void UnpauseGame()
    {
        ChangeGameStatus(GameStatus.PLAY);
    }

    public void PostUnpauseDelay()
    {
        ChangeGameStatus(GameStatus.POSTUNPAUSEDELAY);
    }

    public void OpenOptions()
    {
        SceneManager.LoadScene(LevelNames.settingsMenu, LoadSceneMode.Additive);
    }

    public void PlayAgain()
    {
        GDTFadeEffect fadeComponent = fadeGO.GetComponent<GDTFadeEffect>();
        fadeComponent.firstToLast = false;
        fadeComponent.disableDelay = 1;
        fadeComponent.StartEffect();
        fadeGO.SetActive(true);
        _retryLevel = true;
        ChangeGameStatus(GameStatus.EXIT);
    }

    public void ReturnToMenu()
    {
        GDTFadeEffect fadeComponent = fadeGO.GetComponent<GDTFadeEffect>();
        fadeComponent.firstToLast = false;
        fadeComponent.disableDelay = 1;
        fadeComponent.StartEffect();
        fadeGO.SetActive(true);
        ChangeGameStatus(GameStatus.EXIT);
    }

    public void GoToNextLevel()
    {
        string actualLevel = SceneManager.GetActiveScene().name;
        if (LevelNames.MansionLevel == actualLevel)
            SceneManager.LoadScene(LevelNames.CemeteryLevel);
        else if (LevelNames.CemeteryLevel == actualLevel)
            SceneManager.LoadScene(LevelNames.UnderGroundCityLevel);
        else
            SceneManager.LoadScene(LevelNames.MansionLevel);
    }

    private IEnumerator WinTransition()
    {
        yield return new WaitForSeconds(1);
        WinMenu.SetActive(true);
    }

    private IEnumerator LoseTransition()
    {
        yield return new WaitForSeconds(3);
        LoseMenu.SetActive(true);
        yield return new WaitForSeconds(0.25f);
        SoundManager.Instance.Play(gameOverSound, SoundManager.SoundPriority.IMPORTANT);
    }

    public bool IsGameWon()
    {
        return _isGameWon;
    }

    public int GetNewEnnemyLayer()
    {
        int tempLayer = _ennemiesLayers;

        _ennemiesLayers += 10;
        return tempLayer;
    }
}
