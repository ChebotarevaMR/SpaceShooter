using System.Collections.Generic;

public interface ILevel
{
    LevelState State { get; }
    List<EnemySet> Enemies { get; }
}
