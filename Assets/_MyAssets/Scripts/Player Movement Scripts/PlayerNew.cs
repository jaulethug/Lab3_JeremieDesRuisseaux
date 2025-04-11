using System;
using UnityEngine;

public class PlayerNew : MonoBehaviour
{
    [SerializeField] private float _speed = 600f;
    [SerializeField] private float _rotationSpeed = 150f;
    [SerializeField] private float _runMultipliyer = 1.5f;
    [SerializeField] private float _jumpForce = 4.5f;

    private Rigidbody _rb;
    private PlayerInputActions _playerInputActions;
    private Animator animator;
    private bool _isJumping = false;
    private int _objectToActivateIndex = -1;
    private bool _canActivate = false;
    private bool _canMove = true;


    private void Awake()
    {
        _playerInputActions = new PlayerInputActions();
        _playerInputActions.Player.Enable();
    }

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
    }

    private void OnDestroy()
    {
        _playerInputActions.Player.Disable();
    }

    void Update()
    {
        if (_playerInputActions.Player.Activate.triggered && _canActivate)
        {
            Level1Controller.Instance.ActivateTorch(_objectToActivateIndex);
        }

        if (_playerInputActions.Player.Jump.IsPressed() && !_isJumping)
        {
            _rb.AddForce(new Vector3(0, _jumpForce, 0), ForceMode.Impulse);
            _isJumping = true;
            animator.SetTrigger("Jumping");
        }

        if (_playerInputActions.Player.Run.IsPressed())
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Reception"))
            {
                animator.speed = 2;
            }
            else
            {
                animator.speed = 1;
            }
            move(_speed * _runMultipliyer);
        }
        else
        {
            animator.speed = 1;
            move(_speed);
        }
    }

    private void move(float speed)
    {
        
        if (_canMove)
        {
            Vector2 direction2D = _playerInputActions.Player.Move.ReadValue<Vector2>();
            float turnInput = direction2D.x;
            float moveInput = direction2D.y;

            if (turnInput != 0f)
            {
                transform.Rotate(Vector3.up, turnInput * _rotationSpeed * Time.deltaTime);
            }
            Vector3 direction = transform.forward * moveInput;

            direction = direction.normalized;
            Vector3 velocity = _rb.linearVelocity;
            velocity.x = direction.x * speed * Time.deltaTime;
            velocity.z = direction.z * speed * Time.deltaTime;
            _rb.linearVelocity = velocity;

            if (moveInput != 0f)
            {
                GameManager._instance.StartTime();
                if (!_isJumping)
                {
                    animator.SetBool("Walking", true);
                }
            }
            else
            {
                if (!_isJumping)
                {
                    animator.SetBool("Walking", false);
                }
            }
        }
    }

    public void SetInActiveZone(int index)
    {
        _canActivate = true;
        _objectToActivateIndex= index;
    }

    public void SetOutActiveZone()
    {
        _canActivate = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground" && _isJumping)
        {
            _isJumping = false;
            _canMove = true;
            animator.SetTrigger("Reception");
        }

        if (collision.gameObject.tag == "Axe_Trap")
        {
            _canMove = false;
        }
    }

    



}
