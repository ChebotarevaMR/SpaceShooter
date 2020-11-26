using System;
using UnityEngine;
using UnityEngine.UI;

public class UIPlay : MonoBehaviour
{
    public Text ScoreText;
    public Text LifeText;

    public event Action Restart;
    public event Action Exit;

    private Ship _ship;

    public void Init(Ship ship)
    {
        if (_ship != null) ShipRelease();

        _ship = ship;

        _ship.LifeUpdate += OnLifeUpdate;
        _ship.ScoreUpdate += OnScoreUpdate;
        LifeText.text = _ship.Life.ToString();
        ScoreText.text = _ship.Score.ToString();
    }

    public void OnRestartClick()
    {
        Restart?.Invoke();
    }

    public void OnExitClick()
    {
        Exit?.Invoke();
    }

    private void ShipRelease()
    {
        _ship.LifeUpdate -= OnLifeUpdate;
        _ship.ScoreUpdate -= OnScoreUpdate;
    }

    private void OnScoreUpdate(int value)
    {
        ScoreText.text = value.ToString();
    }

    private void OnLifeUpdate(int value)
    {
        LifeText.text = value.ToString();
    }
}
