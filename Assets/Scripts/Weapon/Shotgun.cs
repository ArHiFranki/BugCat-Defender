using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Weapon
{
    [SerializeField] private float _deltaRotation;

    private const string _triggerName = "FireShotgun";

    public override void Shoot(Transform shootPoint)
    {
        float tmpAngle = shootPoint.rotation.eulerAngles.z - _deltaRotation;
        Instantiate(Bullet, shootPoint.position, Quaternion.Euler(0f, 0f, tmpAngle));
        Instantiate(Bullet, shootPoint.position, shootPoint.rotation);
        tmpAngle = shootPoint.rotation.eulerAngles.z + _deltaRotation;
        Instantiate(Bullet, shootPoint.position, Quaternion.Euler(0f, 0f, tmpAngle));
    }

    public override void PlayAnimation(Player player)
    {
        player.SetAnimationTrigger(_triggerName);
    }

    public override void StopShooting()
    {

    }
}
