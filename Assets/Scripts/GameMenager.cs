using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMenager : MonoBehaviour
{


    public delegate void GameDelegate();
    public static event GameDelegate OnGameStarted;
    public static event GameDelegate OnGameOverConfirmed;


    public static GameMenager Instance;

    public GameObject StartPage;
    public GameObject GameOverPage;
    public GameObject CountdownPage;
    public Text ScoreText;


    enum PageState
    {
        None,
        Start,
        GameOver,
        Countdown
    }
    int score = 0;
    bool GameOver = true;
    bool isPlaying = false;
    
    public bool GameOver1 { get { return GameOver; }}
    public int Score { get { return score; } }

     void Awake()
    {
        Instance = this;

    }
    void OnEnable()
    {
        CountdownText.OnCountdownFinished += OnCountdownFinished;
        TapController.OnPlayerDied += OnPlayerDied;
        TapController.OnPlayerScore += OnPlayerScore;


    }
    void OnDisable()
    {
        CountdownText.OnCountdownFinished -= OnCountdownFinished;
        TapController.OnPlayerDied -= OnPlayerDied;
        TapController.OnPlayerScore -= OnPlayerScore;

    }

    void OnPlayerFinished()
    {
        SetPageState(PageState.None);
        OnGameStarted();
        score = 0;
        GameOver = false;
    }
    void OnPlayerDied()
    {

        GameOver = true;
        isPlaying = false;

        int saveScore = PlayerPrefs.GetInt("HighScore");
        if (score > saveScore)
        {
            PlayerPrefs.SetInt("HighScore", score);
        }
        SetPageState(PageState.GameOver);
        OnGameOverConfirmed();
    }
    void OnPlayerScore()
    {
        score++;
        ScoreText.text = score.ToString();
    }

    void OnCountdownFinished()
    {
        SetPageState(PageState.None);
        OnGameStarted();
        score = 0;
        GameOver = false;
    }
    void SetPageState(PageState state)
    {
        switch (state)
        {
            case PageState.None:
                StartPage.SetActive(false);
                GameOverPage.SetActive(false);
                CountdownPage.SetActive(false);

                break;
            case PageState.Start:
                StartPage.SetActive(true);
                GameOverPage.SetActive(false);
                CountdownPage.SetActive(false);
                break;
            case PageState.GameOver:
                StartPage.SetActive(false);
                GameOverPage.SetActive(true);
                CountdownPage.SetActive(false);
                break;
            case PageState.Countdown:
                StartPage.SetActive(false);
                GameOverPage.SetActive(false);
                CountdownPage.SetActive(true);
                break;
        }
    }

    public void ConfirmGameOver()
    {

        //odpala się gdy naciskamy replay button
        OnGameOverConfirmed();
        ScoreText.text = "0";
        SetPageState(PageState.Start);

    }

    public void StartGame()
    {
      if (isPlaying)
      {
        return;
      }

      if (GameOver)
      {
        ConfirmGameOver();
      }

      SetPageState(PageState.Countdown);
        //odpala się gdy naciskamy Start button
      isPlaying = true;
    }
    void Update()
    {
      if (Input.GetKeyDown(KeyCode.Space))
      {
          StartGame();

      }

    }
}
