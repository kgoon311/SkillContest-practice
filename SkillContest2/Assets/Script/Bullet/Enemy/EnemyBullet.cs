using UnityEngine;

public class EnemyBullet : Bullet
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Player.Instance._hp -= dmg;
            Dead();
        }
    }
}
