using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyType EnemyType;
    [Space]
    public float Speed;
    public int Cost = 1;

    public event Action<Enemy> Hit;
    public event Action<Enemy> Pass;
    public event Action<Enemy> ShipCollision;
    private bool _isStop;
    public void Stop()
    {
        _isStop = true;
    }

    void Update()
    {
        if (_isStop) return;

        transform.position += -Vector3.up * Time.deltaTime * Speed;

    }
    private void OnTriggerExit(Collider other)
    {
        if (_isStop) return;
        var component = other.GetComponent<Border>();
        if (component != null) Pass?.Invoke(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_isStop) return;
        Bullet bullet = other.GetComponent<Bullet>();
        if (bullet != null) Hit?.Invoke(this);
        else
        {
            Ship ship = other.GetComponent<Ship>();
            if (ship != null) ShipCollision?.Invoke(this);
        }
    }
}
