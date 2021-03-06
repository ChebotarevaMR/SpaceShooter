﻿using System;

public enum EnemyType { Small, Large };

[Serializable]
public class EnemySet
{
    public EnemyType Type { get; }

    public int Count { get; }

    public EnemySet(EnemyType type, int count)
    {
        Type = type;
        Count = count;
    }
}
