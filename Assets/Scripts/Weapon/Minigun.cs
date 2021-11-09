using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minigun : Weapon
{
    [SerializeField] private Minigun _minigunPrefab;
    [SerializeField] private float _fireRateDelay;
    [SerializeField] private float _deltaRotation;

    private const string _triggerName = "FireMinigun";
    private Minigun _minigun;
    private bool _isButtonUp;

    public override void Shoot(Transform shootPoint)
    {
        _isButtonUp = false;
        _minigun = Instantiate(_minigunPrefab);
        _minigun.StartCoroutine(_minigun.SpawnBullets(shootPoint, _fireRateDelay, _minigun));
    }

    IEnumerator SpawnBullets(Transform shootPoint, float fireRateDelay, Minigun minigun)
    {
        while(!_isButtonUp)
        {
            float tmpRotation = Random.Range(-_deltaRotation, _deltaRotation);
            float tmpAngle = shootPoint.rotation.eulerAngles.z + tmpRotation;
            Instantiate(Bullet, shootPoint.position, Quaternion.Euler(0f, 0f, tmpAngle));
            yield return new WaitForSeconds(fireRateDelay);
        }

        Destroy(minigun.gameObject);
    }

    public override void PlayAnimation(Player player)
    {
        player.SetAnimationTrigger(_triggerName);
    }

    public override void StopShooting()
    {
        _minigun.StopAllCoroutines();
    }
}
