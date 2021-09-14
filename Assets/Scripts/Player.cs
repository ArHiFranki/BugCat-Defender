using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
public class Player : MonoBehaviour
{
    [SerializeField] private int _health;
    [SerializeField] private List<Weapon> _weapons;
    [SerializeField] private List<Sprite> _weaponsSprite;
    [SerializeField] private Transform _shootPoint;
    [SerializeField] private Menu _menu;
    [SerializeField] private SpriteRenderer _currentWeaponSprite;

    private Weapon _currentWeapon;
    private int _currentWeaponNumber = 0;
    //private int _currentWeaponSpriteNumber = 0;
    private int _currentHealth;
    private Animator _animator;
    private bool _isGamePause = false;

    private const string _firePistolAninationTrigger = "FirePistol";
    private const string _fireShotgunAnimationTrigger = "FireShotgun";

    public int Money { get; private set; }

    public event UnityAction<int, int> HealhtChanged;
    public event UnityAction<int> MoneyChanged;

    private void OnEnable()
    {
        _menu.GamePause += ChangeGamePauseCondition;
    }

    private void OnDisable()
    {
        _menu.GamePause -= ChangeGamePauseCondition;
    }

    private void Start()
    {
        ChangeWeapon(_weapons[_currentWeaponNumber], _weaponsSprite[_currentWeaponNumber]);
        _currentHealth = _health;
        _animator = GetComponent<Animator>();
        AddMoney(100);
    }

    public void ApplyDamage(int damage)
    {
        _currentHealth -= damage;
        HealhtChanged?.Invoke(_currentHealth, _health);

        if (_currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void AddMoney(int reward)
    {
        Money += reward;
        MoneyChanged?.Invoke(Money);
    }

    public void BuyWeapon(Weapon weapon)
    {
        Money -= weapon.Price;
        MoneyChanged?.Invoke(Money);
        _weapons.Add(weapon);
    }

    public void NextWeapon()
    {
        if (_currentWeaponNumber == _weapons.Count - 1)
        {
            _currentWeaponNumber = 0;
        }
        else
        {
            _currentWeaponNumber++;
        }

        ChangeWeapon(_weapons[_currentWeaponNumber], _weaponsSprite[_currentWeaponNumber]);
    }

    public void PreviousWeapon()
    {
        if (_currentWeaponNumber == 0)
        {
            _currentWeaponNumber = _weapons.Count - 1;
        }
        else
        {
            _currentWeaponNumber--;
        }

        ChangeWeapon(_weapons[_currentWeaponNumber], _weaponsSprite[_currentWeaponNumber]);
    }

    private void ChangeWeapon(Weapon weapon, Sprite weaponSprite)
    {
        _currentWeapon = weapon;
        _currentWeaponSprite.sprite = weaponSprite;
    }

    public void FiretWithCurrentWeapon()
    {
        if (!_isGamePause)
        {
            _currentWeapon.Shoot(_shootPoint);
            if (_currentWeaponNumber == 0)
            {
                _animator.SetTrigger(_firePistolAninationTrigger);
            }
            else if (_currentWeaponNumber == 1)
            {
                _animator.SetTrigger(_fireShotgunAnimationTrigger);
            }
            else if (_currentWeaponNumber == 2)
            {

            }
            else if (_currentWeaponNumber == 3)
            {

            }
        }
    }

    private void ChangeGamePauseCondition(bool condition)
    {
        _isGamePause = condition;
    }
}
