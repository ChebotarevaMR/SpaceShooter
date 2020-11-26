using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

public class LevelsManager
{
    private List<Level> _levels;
    private Level _current;

    private const string _nameSettingsFile = "settings.dat";
    private const int MIN_COUNT_TYPE_ENEMY = 2;
    private const int MAX_COUNT_TYPE_ENEMY = 5;
    private const int MIN_COUNT_ENEMIES = 5;
    private const int MAX_COUNT_ENEMIES = 15;
    private const int DEFAULT_COUNT_LEVELS = 10;

    public void LoadLevels()
    {
        _levels = new List<Level>();

        BinaryFormatter formatter = new BinaryFormatter();
        try
        {
            using (FileStream fs = new FileStream(_nameSettingsFile, FileMode.Open))
            {
                _levels = (List<Level>)formatter.Deserialize(fs);
            }
            _levels = GenerateLevels(_levels);
        }
        catch (FileNotFoundException)
        {
            _levels = CreateLevels();
            SaveLevels();
            _levels = GenerateLevels(_levels);
        }
    }


    public ILevel FindOpenedOrLastLevel()
    {
        if (_levels == null || _levels.Count == 0)
        {
            _current = null;
            return null;
        }

        var openLevel = _levels.Find(x => x.State == LevelState.Open);
        if (openLevel != null)
        {
            _current = openLevel;
            return openLevel;
        }

        var firstCloseLevel = _levels.FirstOrDefault(x => x.State == LevelState.Close);
        if (firstCloseLevel != null)
        {
            firstCloseLevel.State = LevelState.Open;
            SaveLevels();
            _current = firstCloseLevel;
            return firstCloseLevel;
        }

        var lastPassLevel = _levels.LastOrDefault(x => x.State == LevelState.Pass);
        if (lastPassLevel != null)
        {
            _current = lastPassLevel;
            return lastPassLevel;
        }
        return null;
    }

    public void SetLevelAsPass()
    {
        _current.State = LevelState.Pass;
        
        SaveLevels();
        _current = null;

    }

    public int GetCurrentLevelId()
    {
        if (_current == null) return -1;
        return _levels.IndexOf(_current);
    }

    private List<Level> CreateLevels()
    {
        Random random = new Random();
        List<Level> levels = new List<Level>();
        for (int i = 0; i < DEFAULT_COUNT_LEVELS; i++)
        {
            levels.Add(new Level(i == 0 ? LevelState.Open : LevelState.Close, random.Next()));
        }
        return levels;
    }

    private List<Level> GenerateLevels(List<Level> levels)
    {
        Random random;
        for (int i = 0; i < levels.Count; i++)
        {
            random = new Random(levels[i].Seed);
            levels[i].Enemies = GenerateEnemySets(random);
        }
        return levels;
    }

    private List<IEnemySet> GenerateEnemySets(Random random)
    {
        int count = random.Next(MIN_COUNT_TYPE_ENEMY, MAX_COUNT_TYPE_ENEMY);
        List<IEnemySet> enemySets = new List<IEnemySet>();

        for (int i = 0; i < count; i++)
        {
            enemySets.Add(GenerateEnemySet(random));
        }
        return enemySets;
    }

    private EnemySet GenerateEnemySet(Random random)
    {
        var array = Enum.GetValues(typeof(EnemyType));
        var max = array.GetLength(0);
        var id = random.Next(0, max);
        EnemyType enemyType = (EnemyType)array.GetValue(id);

        int count = random.Next(MIN_COUNT_ENEMIES, MAX_COUNT_ENEMIES);

        return new EnemySet(enemyType, count);
    }

    public List<ILevel> Levels
    {
        get { return _levels.OfType<ILevel>().ToList(); }
    }

    private void SaveLevels()
    {
        BinaryFormatter formatter = new BinaryFormatter();

        using (FileStream fs = new FileStream(_nameSettingsFile, FileMode.OpenOrCreate))
        {
            formatter.Serialize(fs, _levels);
        }
    }
}
