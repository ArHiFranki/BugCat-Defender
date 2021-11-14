using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Weapon
{
    private const string _triggerName = "FirePistol";

    public override void Shoot(Transform shootPoint)
    {
        Instantiate(Bullet, shootPoint.position, shootPoint.rotation);
    }

    public override void PlayAnimation(Player player)
    {
        player.SetAnimationTrigger(_triggerName);
    }

    public override void StopShooting()
    {

    }
}
