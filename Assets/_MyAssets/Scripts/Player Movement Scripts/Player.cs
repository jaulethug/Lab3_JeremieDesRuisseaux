using System;
using UnityEngine;

public class PlayerOldInput : MonoBehaviour
{
    [SerializeField] private float _speed = 100f;
    [SerializeField] private float _rotationSpeed = 20f;
    [SerializeField] private float _runMultiplier = 3f;
    [SerializeField] private float _jumpForce = 4.5f;

    private Rigidbody _rb;
    private Animator animator;
    private bool _isJumping = false;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        // Gérer le saut
        if (Input.GetButtonDown("Jump") && !_isJumping)
        {
            _rb.AddForce(new Vector3(0, _jumpForce, 0), ForceMode.Impulse);
            _isJumping = true;
            animator.SetTrigger("Jumping");
        }

        // Vérifier si le joueur court
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Reception"))
            {
                animator.speed = 2;
            }
            else
            {
                animator.speed = 1;
            }
            Move(_speed * _runMultiplier);
        }
        else
        {
            animator.speed = 1;
            Move(_speed);
        }
    }

    private void Move(float speed)
    {
        float dirX = Input.GetAxis("Horizontal");
        float dirZ = Input.GetAxis("Vertical");
        Vector3 direction = transform.forward * dirZ;

        if (dirX != 0f)
        {
            transform.Rotate(Vector3.up, dirX * _rotationSpeed * Time.deltaTime);
        }
        direction = direction.normalized;

        // Déplacer le joueur
        Vector3 velocity = _rb.linearVelocity;
        velocity.x = direction.x * speed * Time.deltaTime;
        velocity.z = direction.z * speed * Time.deltaTime;
        _rb.linearVelocity = velocity;

        if (dirZ != 0f)
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground" && _isJumping)
        {
            _isJumping = false;
            animator.SetTrigger("Reception");
        }
    }
}
