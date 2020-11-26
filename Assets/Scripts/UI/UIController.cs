using System;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public UIMenu Menu;
    public UIGameOver GameOver;
    public UIWin Win;
    public UIPlay Play;

    public event Action StartGame;
    public event Action RestartGame;
    public event Action ExitGame;

    private void Start()
    {
        Menu.StartClick += OnStart;
        Menu.ExitClick += OnExit;

        GameOver.Restart += OnRestart;
        GameOver.Exit += OnExit;

        Win.Next += OnStart;
        Win.Exit += OnExit;

        Play.Restart += OnRestart;
        Play.Exit += OnExit;

        Init();
    }

    public void OnStart()
    {
        StartGame?.Invoke();
    }

    public void OnRestart()
    {
        RestartGame?.Invoke();
    }

    public void OnExit()
    {
        ExitGame?.Invoke();
    }

    public void ShowWin(int level)
    {
        Win.Init(level);
        Win.gameObject.SetActive(true);
    }

    public void ShowGameOver()
    {
        GameOver.gameObject.SetActive(true);
    }

    public void ShowPlay(Ship ship)
    {
        Play.Init(ship);
        Play.gameObject.SetActive(true);
    }

    private void Init()
    {
        Menu.gameObject.SetActive(true);
        GameOver.gameObject.SetActive(false);
        Win.gameObject.SetActive(false);
        Play.gameObject.SetActive(false);
    }
}
