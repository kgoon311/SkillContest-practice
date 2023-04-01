using UnityEngine;

public class EnemyBullet : Bullet
{
    protected bool dontshot;
    protected void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Player.Instance._hp -= dmg;
            Dead();
        }
        if (other.CompareTag("DontShot"))
        {
            dontshot = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("DontShot"))
        {
            dontshot = false;
        }
    }
}
