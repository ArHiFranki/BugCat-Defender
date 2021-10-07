using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minigun : Weapon
{
    [SerializeField] private float _fireRateDelay;

    private const string _triggerName = "FireMinigun";

    public override void Shoot(Transform shootPoint)
    {

    }

    public override void PlayAnimation(Player player)
    {
        player.SetAnimationTrigger(_triggerName);
    }
}
