using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
public class Player : MonoBehaviour
{
    [SerializeField] private int _health;
    [SerializeField] private List<Weapon> _weapons;
    [SerializeField] private Transform _pistolShootPoint;
    [SerializeField] private Transform _shotgunShootPoint;
    [SerializeField] private Transform _rifleShootPoint;
    [SerializeField] private Transform _arm;
    [SerializeField] private Menu _menu;
    [SerializeField] private SpriteRenderer _currentWeaponSprite;
    [SerializeField] private Camera _camera;
    [SerializeField] private float _angleMin;
    [SerializeField] private float _angleMax;
    [SerializeField] private float _currentAngle;

    private Transform _currentShootPoint;
    private Weapon _currentWeapon;
    private int _currentWeaponNumber = 0;
    private int _currentHealth;
    private Animator _animator;
    private bool _isGamePause = false;
    private Vector3 mousePosition;
    private Vector2 lookDirection;
    private float lookAngle;

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
        mousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            FiretWithCurrentWeapon();
        }
    }

    private void FixedUpdate()
    {
        _currentAngle = lookAngle;
        ChangeRotation(_arm);
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

    private void FiretWithCurrentWeapon()
    {
        if (!_isGamePause)
        {
            if (_currentWeapon.GetComponent<Pistol>())
            {
                _currentShootPoint = _pistolShootPoint;
            }
            else if (_currentWeapon.GetComponent<Shotgun>())
            {
                _currentShootPoint = _shotgunShootPoint;
            }
            else if (_currentWeapon.GetComponent<Rifle>())
            {
                _currentShootPoint = _rifleShootPoint;
            }

            ChangeRotation(_currentShootPoint);
            _currentWeapon.Shoot(_currentShootPoint);
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

    private void ChangeRotation(Transform objectTransform)
    {
        lookDirection = mousePosition - objectTransform.position;
        lookAngle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 180f;

        if (lookAngle > -180f && lookAngle < _angleMax)
        {
            lookAngle = _angleMax;
        }
        else if (lookAngle > _angleMin && lookAngle < -180f)
        {
            lookAngle = _angleMin;
        }

        objectTransform.rotation = Quaternion.Euler(0f, 0f, lookAngle);
    }
}
