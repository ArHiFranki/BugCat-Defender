using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] private string _lable;
    [SerializeField] private int _price;
    [SerializeField] private Sprite _shopIcon;
    [SerializeField] private Sprite _weaponSprite;
    [SerializeField] private bool _isBuyed = false;
    [SerializeField] private float _fireRate;

    [SerializeField] protected Bullet Bullet;

    public string Label => _lable;
    public int Price => _price;
    public Sprite Icon => _shopIcon;
    public Sprite WeaponSprite => _weaponSprite;
    public bool IsBuyed => _isBuyed;
    public float FireRate => _fireRate;

    public abstract void Shoot(Transform shootPoint);
    public abstract void PlayAnimation(Player player);
    public abstract void StopShooting();

    public void Buy()
    {
        _isBuyed = true;
    }
}
