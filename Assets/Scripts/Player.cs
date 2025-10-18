using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private Rigidbody2D playerRb;
    private BoxCollider2D boxCollider;
    private Animator anim;
    private SpriteRenderer _spriteRenderer;
    
    private float playerHorDir, facingDirection = 1f, lastShot, life = 5, flashDuration = 0.1f;
    int flashCount = 5;
    private bool canDoubleJump;
    
    [SerializeField] private float velocityX, cooldown, velocityY, shootVelocity;
    
    [SerializeField] private GameObject bulletPrefab; 
    [SerializeField] private Transform firePoint; 
    [SerializeField] private SpriteRenderer gunSprite;
    
    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        anim = GetComponentInChildren<Animator>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }
    
    void Update()
    {
        playerRb.linearVelocity = new Vector2(playerHorDir * velocityX, playerRb.linearVelocity.y);
        Flip();
        UpdateAnimationState(); 
    }

    void UpdateAnimationState()
    {
        bool isGrounded = boxCollider.IsTouchingLayers(LayerMask.GetMask("Foreground"));
        
        anim.SetBool("IsRunning", playerHorDir != 0 && isGrounded);
        anim.SetBool("IsJumping", !isGrounded);
    }

    void Flip()
    {
        if (playerHorDir != 0)
        {
            facingDirection = Mathf.Sign(playerHorDir);
            transform.localScale = new Vector2(facingDirection, 1);
        }
    }

    void OnMove(InputValue value)
    {
        playerHorDir = value.Get<Vector2>().x;
    }

    void OnJump(InputValue value)
    {
        bool isGrounded = boxCollider.IsTouchingLayers(LayerMask.GetMask("Foreground"));

        if (value.isPressed && isGrounded)
        {
            playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, velocityY);
            canDoubleJump = true;
        } 
        else if (value.isPressed && canDoubleJump) 
        {
            anim.SetTrigger("JumpTrigger");
            playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, velocityY);
            canDoubleJump = false;
        }
    }

    void OnAttack(InputValue value)
    {

        if (lastShot + cooldown > Time.time) return;
        
        lastShot = Time.time;
        
        Quaternion bulletRotation;

        if (facingDirection > 0)
        {
            bulletRotation = Quaternion.Euler(0, 0, -90);
        }
        else
        {
            bulletRotation = Quaternion.Euler(0, 180, -90);
        }
        
        GameObject bulletObject = Instantiate(bulletPrefab, firePoint.position, bulletRotation);
        Bullet bulletInstance = bulletObject.GetComponent<Bullet>();
        
        bulletInstance.Shoot(facingDirection, shootVelocity);
    }

    void Damage()
    {
        life--;

        StartCoroutine(FlashSprite());
        
        if (life <= 0) Destroy(gameObject);
    }
    
    private IEnumerator FlashSprite()
    {
        for (int i = 0; i < flashCount; i++)
        {
            _spriteRenderer.enabled = false;
            gunSprite.enabled = false;
            yield return new WaitForSeconds(flashDuration); 
            _spriteRenderer.enabled = true;
            gunSprite.enabled = true;
            yield return new WaitForSeconds(flashDuration); 
        }
        _spriteRenderer.enabled = true; 
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Coin")) return;

        CoinSpawner spawner = FindFirstObjectByType<CoinSpawner>();
        
        spawner.Notify();

        Destroy(other.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Monster")) Damage();
    }
}