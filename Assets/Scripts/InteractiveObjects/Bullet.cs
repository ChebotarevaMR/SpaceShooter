using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float Speed = 3.0f;
    public bool _isStop;

    private IShooter _shooter;
    public void SetShip(IShooter shooter)
    {
        _shooter = shooter;
    }
    public void StopGame()
    {
        _isStop = true;
    }

    void Update()
    {
        if (_isStop) return;
        transform.position += transform.up * Speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_isStop) return;
        var enemy = other.GetComponent<Enemy>();
        if (enemy != null)
        {
            _shooter.BulletHit(enemy.Cost, this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_isStop) return;
        if (other.GetComponent<Border>() != null)
        {
            _shooter.BulletMiss(this);
        }
    }
}
