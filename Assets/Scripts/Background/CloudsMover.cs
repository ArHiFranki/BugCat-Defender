using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class CloudsMover : MonoBehaviour
{
    [SerializeField] private float _speed;

    private Vector2 _startPos;
    private float _repeatWidth;

    private void Start()
    {
        _startPos = transform.position;
        _repeatWidth = GetComponent<BoxCollider2D>().size.x / 2;
    }

    private void Update()
    {
        transform.Translate(Vector2.left * Time.deltaTime * _speed);

        if (transform.position.x < _startPos.x - _repeatWidth)
        {
            transform.position = _startPos;
        }
    }
}
