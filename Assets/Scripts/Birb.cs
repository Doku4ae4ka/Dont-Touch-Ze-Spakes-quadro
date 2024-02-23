using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class Birb : MonoBehaviour, IMoveable
{
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Animator _animator;

    [SerializeField] private float jumpPower;
    private BirdInput _birdInput;
    private bool isDead = false;
    private bool isJumping = false;

    public event Action OnBirdDeath;
    public event Action OnWallHit;
    public event Action<bool> OnRightWallHit;
    public event Action<bool> OnLeftWallHit;

    public float Speed { get; set; }

    private void Awake()
    {
        _birdInput = new BirdInput();

        _birdInput.Bird.Jump.performed += OnJump;
        Speed = 3f;
    }

    private void OnEnable()
    {
        _birdInput.Enable();
        OnBirdDeath += OnDeath;
    }

    private void OnDisable()
    {
        _birdInput.Disable();
        OnBirdDeath -= OnDeath;
    }

    private void Update()
    {
        if (!isDead)
        {
            Move();
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Spike")
        {
            if(!isDead)
                OnBirdDeath?.Invoke();
            isDead = true;
        }
        else if (collision.gameObject.tag == "RightWall")
        {
            Flip();
            OnWallHit?.Invoke();
            OnRightWallHit?.Invoke(false);
        }
        else if (collision.gameObject.tag == "LeftWall")
        {
            Flip();
            OnWallHit?.Invoke();
            OnLeftWallHit?.Invoke(true);
        }
    }

    private void OnDeath()
    {
        _birdInput.Disable();
        _animator.SetBool("IsDead", true);
        _spriteRenderer.DOFade(0, 4);
        _rigidbody.constraints = RigidbodyConstraints2D.None;
        _rigidbody.DORotate(99999, 5);
        StartCoroutine(DeathCoroutine());
    }

    #region Move Methods
    public void Move()
    {
        _rigidbody.velocity = new Vector2(Speed * transform.localScale.x, _rigidbody.velocity.y);
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        StartCoroutine(JumpCoroutine());
        _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, jumpPower);
    }

    private void Flip()
    {
        Vector3 currentScale = transform.localScale;
        currentScale.x *= -1;
        transform.localScale = currentScale;
    }
    #endregion

    private IEnumerator DeathCoroutine()
    {
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }

    private IEnumerator JumpCoroutine()
    {
        isJumping = true;
        _animator.SetBool("IsJumping", isJumping);

        yield return new WaitForSeconds(0.5f);
        isJumping = false;
        _animator.SetBool("IsJumping", isJumping);
    }
}
