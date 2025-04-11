using System;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    [Header("Transversal movement")]
    private bool _changeDirection = false;
    [SerializeField] private float _speed = 20f;
    [SerializeField] private bool _movingVertically = true;

    [Header("Make the object rotate")]
    [SerializeField] private bool _rotating = false;
    [Header("Rotation variables")]
    [SerializeField] private float _rotationSpeed = 3f;
    [SerializeField] private bool _rotationClockWise = true;
    private Rigidbody _rb;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        _rb= GetComponentInChildren<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_rotating)
        {
            move();
        }
        else
        {
            rotate();
        }
    }

    private void move()
    {
        Vector3 transition;
        if (_movingVertically)
        {
            transition = _changeDirection ? new Vector3(0f, 0f, -1f) : new Vector3(0f, 0f, 1f); 
        }
        else
        {
            transition = _changeDirection ? new Vector3(-1f, 0f, 0f) : new Vector3(1f, 0f, 0f);
        }
        
        transition = transition.normalized;
        _rb.linearVelocity = transition * Time.deltaTime * _speed;
    }

    private void rotate()
    {
        float rotationAmount = _rotationSpeed * Time.deltaTime;
        if (_rotationClockWise)
        {
            transform.Rotate(0f, rotationAmount, 0f);
        }
        else
        {
            transform.Rotate(0f, -rotationAmount, 0f);
        }
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Player" || collision.gameObject.tag != "Ground")
        {
            _changeDirection = !_changeDirection;
            transform.Rotate(0f, 180f, 0f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "spider_changeDirection")
        {
            _changeDirection = !_changeDirection;
            transform.Rotate(0f, 180f, 0f);
        }
    }
}
