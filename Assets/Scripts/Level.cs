using System;
using System.Collections.Generic;

public enum LevelState { Pass, Open, Close};

[Serializable]
public class Level : ILevel
{
    public LevelState State { get; set; }

    [field: NonSerialized]
    public List<EnemySet> Enemies { get; set; }

    public int Seed { get; }

    public Level(LevelState state, int seed)
    {
        Seed = seed;
        State = state;
    }
}
