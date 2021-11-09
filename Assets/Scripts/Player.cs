using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Animator))]
public class Player : MonoBehaviour
{
    [SerializeField] private int _health;
    [SerializeField] private List<Weapon> _weapons;
    [SerializeField] private Transform _pistolShootPoint;
    [SerializeField] private Transform _shotgunShootPoint;
    [SerializeField] private Transform _rifleShootPoint;
    [SerializeField] private Transform _minigunShootPoint;
    [SerializeField] private Transform _arm;
    [SerializeField] private Menu _menu;
    [SerializeField] private SpriteRenderer _currentWeaponSprite;
    [SerializeField] private Camera _camera;
    [SerializeField] private float _angleMin;
    [SerializeField] private float _angleMax;
    [SerializeField] private float _lookAngle;

    private Vector3 _mousePosition;
    private Vector2 _lookDirection;
    private Transform _currentShootPoint;
    private Weapon _currentWeapon;
    private Animator _animator;
    private int _currentWeaponNumber = 0;
    private int _currentHealth;
    private float _timeAfterLastShoot;
    private bool _isGamePause = false;
    private bool _isFireWithMinigun = false;

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
        _timeAfterLastShoot = 0;
    }

    private void Update()
    {
        _mousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);
        _timeAfterLastShoot += Time.deltaTime;

        if (Input.GetMouseButtonDown(0))
        {
            FiretWithCurrentWeapon();
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (_currentWeapon.GetComponent<Minigun>() && _isFireWithMinigun)
            {
                _currentWeapon.StopShooting();
                _isFireWithMinigun = false;
            }
        }
    }

    private void FixedUpdate()
    {
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
        if(EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if (!_isGamePause && _timeAfterLastShoot >= _currentWeapon.FireRate)
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
            else if (_currentWeapon.GetComponent<Minigun>())
            {
                _currentShootPoint = _minigunShootPoint;
                _isFireWithMinigun = true;
            }

            _currentWeapon.Shoot(_currentShootPoint);
            _currentWeapon.TryGetComponent(out Weapon tmpWeapon);
            tmpWeapon.PlayAnimation(this);
            _timeAfterLastShoot = 0;
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
        _lookDirection = _mousePosition - objectTransform.position;
        _lookAngle = Mathf.Atan2(_lookDirection.y, _lookDirection.x) * Mathf.Rad2Deg - 180f;

        if (_lookAngle > -180f && _lookAngle < _angleMax)
        {
            _lookAngle = _angleMax;
        }
        else if (_lookAngle > _angleMin && _lookAngle < -180f)
        {
            _lookAngle = _angleMin;
        }

        objectTransform.rotation = Quaternion.Euler(0f, 0f, _lookAngle);
    }
}
