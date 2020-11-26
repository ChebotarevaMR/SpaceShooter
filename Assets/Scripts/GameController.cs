using UnityEngine;

public class GameController : MonoBehaviour
{
    public Transform PositonPlayer;
    public Ship Player;

    [Space]
    public EnemiesController EnemiesController;

    [Space]
    public Map MapControl;

    [Space]
    public UIController UIController;

    private LevelsManager _levelsManager = new LevelsManager();
    private Ship _player;
    private ILevel _currentLevel;
    
    private void Start()
    {
        _levelsManager.LoadLevels();
        MapControl.GenerateMap(_levelsManager.Levels);

        EnemiesController.EnemiesEnded += OnEnemiesEnded;
        UIController.StartGame += OnStartGame;
        UIController.RestartGame += OnRestartGame;
        UIController.ExitGame += OnExitGame;

    }

    private void OnStartGame()
    {
        _currentLevel = _levelsManager.FindOpenedOrLastLevel();
        if (_currentLevel.State != LevelState.Pass)
        {
            MapControl.ChandgeLevel(_levelsManager.GetCurrentLevelId(), _currentLevel.State);
        }
        StartGame();
    }

    private void OnRestartGame()
    {
        if (_player.Life <= 0)
        {
            StartGame();
        }
        else
        {
            StopGame();
            StartGame();
        }
    }

    private void OnExitGame()
    {
        Application.Quit();
    }

    private void OnPlayerLifeUpdate(int life)
    {
        if(life == 0)
        {
            StopGame();
            UIController.ShowGameOver();
        }
    }

    private void OnEnemiesEnded()
    {
        StopGame();

        var levelId = _levelsManager.GetCurrentLevelId();

        if (_currentLevel.State != LevelState.Pass)
        {
            _levelsManager.SetLevelAsPass();
            MapControl.ChandgeLevel(levelId, LevelState.Pass);
        }            
        UIController.ShowWin(levelId + 1);
    }

    private void StopGame()
    {
        EnemiesController.StopGame();
        _player.StopGame();
    }

    private void StartGame()
    {
        if (_player != null)
        {
            _player.Release();
            Destroy(_player.gameObject);
        }
        _player = Instantiate(Player, PositonPlayer);
        _player.LifeUpdate += OnPlayerLifeUpdate;
        EnemiesController.Release();
        EnemiesController.StartGame(_currentLevel);
        UIController.ShowPlay(_player);
    }
}
