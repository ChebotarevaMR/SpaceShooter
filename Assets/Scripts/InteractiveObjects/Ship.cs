using System;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour, IShooter
{
    public float Speed = 3.0f;
    public Bullet Bullet;
    public int Life = 3;
    public int Score = 0;
    public float MinIntervalBetweenShots = 0.9f;

    private float _time;
    private List<Bullet> _bullets = new List<Bullet>();
    private bool _isStop;
    private Border _border;
    private float _shipRadius;

    public event Action<int> LifeUpdate;
    public event Action<int> ScoreUpdate;

    public void BulletHit(int score, Bullet bullet)
    {
        ReleaseBullet(bullet);
        Score += score;
        ScoreUpdate?.Invoke(Score);
    }

    public void BulletMiss(Bullet bullet)
    {
        ReleaseBullet(bullet);
    }

    public void StopGame()
    {
        _isStop = true;
        for (int i = 0; i < _bullets.Count; i++)
        {
            _bullets[i].StopGame();
        }
    }

    public void Release()
    {
        while (_bullets.Count > 0)
        {
            ReleaseBullet(_bullets[0]);
        }
    }

    private void Start()
    {
        _border = FindObjectOfType<Border>();
        _time = MinIntervalBetweenShots;
        _shipRadius = GetComponent<PolygonCollider2D>().bounds.size.magnitude / 2.0f;
    }

    private void Update()
    {
        if (_isStop) return;
        _time += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.E) && _time > MinIntervalBetweenShots)
        {
            Shoot();
            _time = 0;
        }
        var x = Input.GetAxis("Horizontal");
        var y = Input.GetAxis("Vertical");
        Move(x, y);
    }

    // TODO rigidbody move
    private void Move(float x, float y)
    {
        var movement = new Vector3(x, y, 0) * Time.deltaTime * Speed;
        if (CheckMove(movement))
        {
            transform.position += movement;
        }
    }

    private bool CheckMove(Vector3 movement)
    {
        Vector3 point = transform.position + movement;
        return
            point.x + _shipRadius < _border.XRight &&
            point.x - _shipRadius > _border.XLeft &&
            point.y + _shipRadius < _border.YTop &&
            point.y - _shipRadius > _border.YBottom;
    }

    private void Shoot()
    {
        var bullet = Instantiate(Bullet, transform.position, Bullet.gameObject.transform.rotation);
        bullet.SetShooter(this);
        _bullets.Add(bullet);
    }

    private void ReleaseBullet(Bullet bullet)
    {
        _bullets.Remove(bullet);
        Destroy(bullet.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_isStop) return;
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null)
        {
            Life--;
            LifeUpdate?.Invoke(Life);
        }
    }
}
