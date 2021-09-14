using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Weapon
{
    [SerializeField] private float deltaRotation;

    public override void Shoot(Transform shootPoint)
    {
        Instantiate(Bullet, shootPoint.position, Quaternion.Euler(0f, 0f, -deltaRotation));
        Instantiate(Bullet, shootPoint.position, Quaternion.identity);
        Instantiate(Bullet, shootPoint.position, Quaternion.Euler(0f, 0f, deltaRotation));
    }
}
