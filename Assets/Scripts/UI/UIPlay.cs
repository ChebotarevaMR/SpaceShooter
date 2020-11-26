using System;
using UnityEngine;
using UnityEngine.UI;

public class UIPlay : MonoBehaviour
{
    public Text ScoreText;
    public Text LifeText;

    private Ship _ship;

    public event Action Restart;
    public event Action Exit;

    public void Init(Ship ship)
    {
        if (_ship != null) ShipRelease();

        _ship = ship;

        _ship.LifeUpdate += OnLifeUpdate;
        _ship.ScoreUpdate += OnScoreUpdate;
        LifeText.text = $"Life: {_ship.Life}";
        ScoreText.text = $"Score: {_ship.Score}";
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
        ScoreText.text = $"Score: {value}";
    }

    private void OnLifeUpdate(int value)
    {
        LifeText.text = $"Life: {value}";
    }
}
