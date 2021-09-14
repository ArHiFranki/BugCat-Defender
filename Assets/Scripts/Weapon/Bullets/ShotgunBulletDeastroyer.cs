using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunBulletDeastroyer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out ShotgunBullet bullet))
        {
            bullet.DestroyBullet();
        }
    }
}
