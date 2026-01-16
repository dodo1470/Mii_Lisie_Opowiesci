using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using TMPro;



public enum GameState { GAME, PAUSE_MENU, LEVEL_COMPLETED, GAME_OVER, OPTIONS};
public class GameManager : MonoBehaviour
{
    const string keyHighScore = "HighScoreLevel1";
    private int killed = 0;
    public TMP_Text killedText;
    private int seconds = 0;
    private int minutes = 0;
    private float timer = 0.0f;
    AudioClip bsound;
    public TMP_Text timerText;
    public Canvas pauseMenuCanvas;
    public Canvas levelCompletedCanvas;
    public Canvas loseCanvas;
    public Image[] keysTab;
    public Canvas inGameCanvas;
    public Canvas optionsCanvas;
    public TMP_Text livesText;
    public TMP_Text scoreText;
    public TMP_Text scoreLabel;
    public TMP_Text highScoreLabel;
    private int keysFound = 0;
    public int lives = 3;
    public static int maxKeysNumber = 3;
    public bool keysCompleted = false;
    private bool hold = false;
    private int score = 0;
    public static GameManager instance;
    public GameState currentGameState = GameState.PAUSE_MENU;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //currentGameState = GameState.GAME;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        seconds = (int)(timer) % 60;
        minutes = (int)(timer) / 60;
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        if (Input.GetKey(KeyCode.Escape) && hold == false)
        {
            hold = true;
            
            if (currentGameState == GameState.GAME)
            {
                PauseMenu();
            }
            else if (currentGameState == GameState.PAUSE_MENU)
            {
                InGame();
            }
        } else if (Input.GetKey(KeyCode.Escape) != true)
        {
            hold = false;
        }
        if (lives == 0)
        {
            GameOver();
        }
    }

    void Awake()
    {
        InGame();
        if (!PlayerPrefs.HasKey(keyHighScore))
        {   
            PlayerPrefs.SetInt(keyHighScore, 0);
        }
        if (instance == null)
        {
            instance = this;
        } else
        {
            UnityEngine.Debug.LogError("Duplicated Game Manager", gameObject);
        }
        scoreText.text = score.ToString();
        livesText.text = lives.ToString();
        keysTab[0].color = Color.grey;
        keysTab[1].color = Color.grey;
        keysTab[2].color = Color.grey;
    }

    void SetGameState (GameState newGameState)
    {
        currentGameState = newGameState;
        if (currentGameState == GameState.GAME)
        {
            inGameCanvas.enabled = true;
        }
        else
        {
            inGameCanvas.enabled = false;
        }
        pauseMenuCanvas.enabled = currentGameState == GameState.PAUSE_MENU;
        optionsCanvas.enabled = currentGameState == GameState.OPTIONS;
        levelCompletedCanvas.enabled = currentGameState == GameState.LEVEL_COMPLETED;
        loseCanvas.enabled = currentGameState == GameState.GAME_OVER;
        if (GameState.LEVEL_COMPLETED == currentGameState)
        {
            Scene currentScene = SceneManager.GetActiveScene();
            if (currentScene.name == "Level1_86")
            {
                int highscore = PlayerPrefs.GetInt(keyHighScore);
                if (highscore < score)
                {
                    highscore = score;
                    PlayerPrefs.SetInt(keyHighScore, highscore);
                }
                scoreLabel.text = "Your Score = " + score;
                highScoreLabel.text = "High Score = " + highscore;
            }
        }
    }
    public void PauseMenu()
    {
        SetGameState (GameState.PAUSE_MENU);
        Time.timeScale = 0;
    }
    public void InGame()
    {
        SetGameState(GameState.GAME);
        Time.timeScale = 1;
    }
    public void LevelCompleted()
    {
        SetGameState(GameState.LEVEL_COMPLETED);
        Time.timeScale = 0;
    }
    public void GameOver()
    {
        SetGameState(GameState.GAME_OVER);
        Time.timeScale = 0;
    }
    public void SetGameStateOptions()
    {
        SetGameState(GameState.OPTIONS);
        Time.timeScale = 0;
    }

    public void AddPoints(int points)
    {
        score += points;
        UnityEngine.Debug.Log(score);
        scoreText.text = score.ToString();
    }

    public void AddKeys(int num)
    {
        keysFound += 1;
        //UnityEngine.Debug.Log("zebrano klucz");
        if (keysFound == maxKeysNumber) 
            {
            keysCompleted = true;
            //UnityEngine.Debug.Log("zebrano wszystkie klucze");
        }
        if(num == 0)
        {
            keysTab[0].color = Color.red;
        }
        if (num == 1)
        {
            keysTab[1].color = Color.green;
        }
        if (num == 2)
        {
            keysTab[2].color = Color.blue;
        }
    }

    public void AddLives(int livesNumber)
    {
        lives += livesNumber;
        livesText.text = lives.ToString();
    }

    public void OnResumeButtonClicked()
    {
        InGame();
    }
    public void OnRestartButtonClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void OnExitButtonClicked()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void AddKills(int points)
    {
        killed += points;
        UnityEngine.Debug.Log(killed);
        killedText.text = killed.ToString();
    }
    public void Options()
    {
        SetGameStateOptions();
    }
    public void PlusQ()
    {
        QualitySettings.IncreaseLevel();
    }
    public void MinusQ()
    {
        QualitySettings.DecreaseLevel();
    }
    public void SetVolume(float vol123)
    {
        AudioListener.volume = vol123;
    }
}
