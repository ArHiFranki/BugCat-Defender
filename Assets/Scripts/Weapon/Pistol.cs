using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Weapon
{
    private const string _firePistolAninationTrigger = "FirePistol";

    public override void Shoot(Transform shootPoint)
    {
        Instantiate(Bullet, shootPoint.position, Quaternion.identity);
    }

    public override void PlayAnimation(Player player)
    {
        player.SetAnimationTrigger(_firePistolAninationTrigger);
    }
}
