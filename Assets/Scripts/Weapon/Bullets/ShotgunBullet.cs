using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunBullet : Bullet
{
    [SerializeField] private float _timeToLive;

    private void OnEnable()
    {
        Destroy(gameObject, _timeToLive);
    }
}
