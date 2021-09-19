using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : Weapon
{
    [SerializeField] private Rifle _riflePrefab;
    [SerializeField] private float _fireRateDelay;
    [SerializeField] private int _shotsNumber;

    private Rifle _rifle;
    private const string _triggerName = "FireRifle";

    public override void Shoot(Transform shootPoint)
    {
        _rifle = Instantiate(_riflePrefab);
        _rifle.StartCoroutine(_rifle.SpawnBullets(shootPoint, _fireRateDelay, _shotsNumber, _rifle));
    }

    IEnumerator SpawnBullets(Transform shootPoint, float fireRateDelay, int shotsNumber, Rifle rifle)
    {
        for (int i = 0; i < shotsNumber; i++)
        {
            Instantiate(Bullet, shootPoint.position, Quaternion.identity);
            yield return new WaitForSeconds(fireRateDelay);
        }
        Destroy(rifle.gameObject);
    }

    public override void PlayAnimation(Player player)
    {
        player.SetAnimationTrigger(_triggerName);
    }
}
