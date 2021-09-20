using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
public class Player : MonoBehaviour
{
    [SerializeField] private int _health;
    [SerializeField] private List<Weapon> _weapons;
    [SerializeField] private Transform _shootPoint;
    [SerializeField] private Menu _menu;
    [SerializeField] private SpriteRenderer _currentWeaponSprite;
    [SerializeField] private Camera _camera;

    private Weapon _currentWeapon;
    private int _currentWeaponNumber = 0;
    private int _currentHealth;
    private Animator _animator;
    private bool _isGamePause = false;
    //private Vector3 mousePosition;
    //private Vector2 lookDirection;

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
        ChangeWeapon(_weapons[_currentWeaponNumber]);
        _currentHealth = _health;
        _animator = GetComponent<Animator>();
        AddMoney(100);
    }

    private void Update()
    {
        //mousePosition =_camera.ScreenToWorldPoint(Input.mousePosition);

        //Vector2 lookDirection = mousePosition - _shootPoint.position;
        //float lookAngle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 180f;
        //_shootPoint.rotation = Quaternion.Euler(0f, 0f, lookAngle);

        if (Input.GetMouseButtonDown(0))
        {
            FiretWithCurrentWeapon(Input.mousePosition);
        }
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

        ChangeWeapon(_weapons[_currentWeaponNumber]);
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

        ChangeWeapon(_weapons[_currentWeaponNumber]);
    }

    private void ChangeWeapon(Weapon weapon)
    {
        _currentWeapon = weapon;
        _currentWeaponSprite.sprite = weapon.WeaponSprite;
    }

    private void FiretWithCurrentWeapon(Vector3 mousePosition)
    {
        if (!_isGamePause)
        {
            //mousePosition = _camera.ScreenToWorldPoint(currentMousePosition);
            //Vector2 lookDirection = mousePosition - _shootPoint.position;
            Vector2 lookDirection = _camera.ScreenToWorldPoint(mousePosition) - _shootPoint.position;
            float lookAngle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 180f;
            _shootPoint.rotation = Quaternion.Euler(0f, 0f, lookAngle);

            _currentWeapon.Shoot(_shootPoint);
            _currentWeapon.TryGetComponent(out Weapon tmpWeapon);
            tmpWeapon.PlayAnimation(this);
        }
    }

    private void ChangeGamePauseCondition(bool condition)
    {
        _isGamePause = condition;
    }

    public void SetAnimationTrigger(string animationTriggerName)
    {
        _animator.SetTrigger(animationTriggerName);
    }
}
