using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float Speed = 3.0f;
    public bool _isStop;

    private IShooter _shooter;

    public void SetShooter(IShooter shooter)
    {
        _shooter = shooter;
    }
    public void StopGame()
    {
        _isStop = true;
    }

    private void Update()
    {
        if (_isStop) return;
        transform.position += transform.up * Speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_isStop) return;
        var enemy = other.GetComponent<Enemy>();
        if (enemy != null)
        {
            _shooter.BulletHit(enemy.Cost, this);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (_isStop) return;
        if (other.GetComponent<Border>() != null)
        {
            _shooter.BulletMiss(this);
        }
    }
}
