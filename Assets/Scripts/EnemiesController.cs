using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemiesController : MonoBehaviour
{
    public Transform ParentFromEnemies;
    public List<Enemy> AllKindsEnemies;
    public int Count = 3;

    private const float DEEGREE = 15;
    private const float TIME_INTERVAL = 0.7f;
    private List<EnemyType> _randomListEnemies = new List<EnemyType>();
    private List<Enemy> _aliveEnemies = new List<Enemy>();
    private Border _border;
    private bool _isStop = true;
    private float _time;
    private int _beginCountEnemies;

    public event Action EnemiesEnded;
    public event Action<int, int> ChangedEnemiesCount;

    public void StartGame(ILevel level)
    {
        _randomListEnemies = GetRandomListEnemyTypes(level.Enemies);
        _beginCountEnemies = _randomListEnemies.Count;
        _isStop = false;
        _time = TIME_INTERVAL;
    }

    public void StopGame()
    {
        _isStop = true;
        for(int i = 0; i < _aliveEnemies.Count; i++)
        {
            _aliveEnemies[i].Stop();
        }
    }

    public void Release()
    {
        if (_aliveEnemies.Count > 0)
        {
            while (_aliveEnemies.Count != 0)
            {
                ReleaseEnemy(_aliveEnemies[0]);
            }
        }
    }
    private void Start()
    {
        _border = FindObjectOfType<Border>();
    }

    private void Update()
    {
        if (_isStop) return;
        _time += Time.deltaTime;
        if (_aliveEnemies.Count < Count && _time > TIME_INTERVAL)
        {
            AddEnemies();
            _time = 0;
        }
    }
    private void OnEnemyPass(Enemy enemy)
    {
        ReleaseEnemy(enemy);
        ChangedEnemiesCount?.Invoke(_beginCountEnemies, _randomListEnemies.Count + _aliveEnemies.Count);
    }

    private void OnEnemyHit(Enemy enemy)
    {
        ReleaseEnemy(enemy);
        ChangedEnemiesCount?.Invoke(_beginCountEnemies, _randomListEnemies.Count + _aliveEnemies.Count);
    }
    private void OnEnemyShipCollision(Enemy enemy)
    {
        ReleaseEnemy(enemy);
        ChangedEnemiesCount?.Invoke(_beginCountEnemies, _randomListEnemies.Count + _aliveEnemies.Count);
    }

    private void ReleaseEnemy(Enemy enemy)
    {
        enemy.Hit -= OnEnemyHit;
        enemy.Pass -= OnEnemyPass;
        enemy.ShipCollision -= OnEnemyShipCollision;
        _aliveEnemies.Remove(enemy);
        Destroy(enemy.gameObject);
    }

    private void AddEnemies()
    {
        if (_randomListEnemies.Count <= 0 && _aliveEnemies.Count <= 0)
        {
            EnemiesEnded?.Invoke();
            return;
        }
        if (_randomListEnemies.Count <= 0) return;

        var enemy = CreateEnemy(_randomListEnemies[0]);
        if (enemy == null) throw new Exception("Type enemy not found.");
        _randomListEnemies.RemoveAt(0);
        _aliveEnemies.Add(enemy);
        enemy.Hit += OnEnemyHit;
        enemy.Pass += OnEnemyPass;
        enemy.ShipCollision += OnEnemyShipCollision;
    }

    private Enemy CreateEnemy(EnemyType enemyType)
    {
        var currentEnemy = AllKindsEnemies.Find(x => x.EnemyType == enemyType);
        if (currentEnemy != null)
        {
            var x = UnityEngine.Random.Range(_border.XLeft, _border.XRight);
            var quaternion = Quaternion.Euler(0, 0, DEEGREE * UnityEngine.Random.Range(0, 360.0f / DEEGREE));
            var y = _border.YTop + currentEnemy.GetComponent<PolygonCollider2D>().bounds.size.magnitude;

            return Instantiate(currentEnemy, new Vector3(x, y, 0), quaternion, ParentFromEnemies);

        }
        return null;
    }

    private List<EnemyType> GetRandomListEnemyTypes(List<EnemySet> enemies)
    {
        List<int> listCounts = enemies.Select(enemy => enemy.Count).ToList();

        List<EnemyType> randomRang = new List<EnemyType>();

        while (listCounts.Exists(x => x > 0))
        {
            int id = UnityEngine.Random.Range(0, listCounts.Count - 1);
            int numberAttempts = 0;

            while (numberAttempts < listCounts.Count)
            {
                if (listCounts[id] > 0)
                {
                    randomRang.Add(enemies[id].Type);
                    listCounts[id]--;
                    break;
                }
                else
                {
                    listCounts.RemoveAt(id);
                    numberAttempts++;

                    if (listCounts.Count <= 0)
                    {
                        break;
                    }

                    if (id >= listCounts.Count)
                    {
                        id = 0;
                    }
                    else if (id < listCounts.Count - 1)
                    {
                        id++;
                    }
                }
            }
        }
        return randomRang;
    }

}
