public interface IShooter
{
    void BulletHit(int score, Bullet bullet);

    void BulletMiss(Bullet bullet);
}
