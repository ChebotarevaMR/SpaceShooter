using System.Collections.Generic;

public interface ILevel
{
    LevelState State { get; }
    List<IEnemySet> Enemies { get; }
}
