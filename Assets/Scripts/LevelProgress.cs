using UnityEngine;
using UnityEngine.UI;

public class LevelProgress : MonoBehaviour
{
    public Image[] Steps;
    public Image Pass;
    public float StartAlpha = 0.1f;
    public float EndAlpha = 1.0f;

    private float _current;
    private float _interval;
    private int _id;

    public void Init(EnemiesController enemiesController)
    {
        enemiesController.ChangedEnemiesCount += OnChangedEnemiesCount;
    }

    public void StartGame()
    {
        Reset();
    }

    private void OnChangedEnemiesCount(int total, int left)
    {

        if (_interval < 0)
        {
            _interval = total * 1.0f / (Steps.Length + 2);
            _current = _interval;
        }
        while (total - left >= _current)
        {
            _current += _interval;
            if (_id < Steps.Length)
            {
                Mark(Steps[_id], EndAlpha);
            }
            _id++;
        }
        if (left == 0)
        {
            Mark(Pass, EndAlpha);
        }
    }

    private void Reset()
    {
        _id = 0;
        _current = 0;
        _interval = -1;
        Mark(Pass, StartAlpha);
        for (int i = 0; i < Steps.Length; i++)
        {
            Mark(Steps[i], StartAlpha);
        }
    }

    private void Mark(Image image, float alpha)
    {
        var color = image.color;
        image.color = new Color(color.r, color.g, color.b, alpha);
    }
}
