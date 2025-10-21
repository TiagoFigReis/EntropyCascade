using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

enum UpgradeType
{
    DoubleJump,
    Damage,
    Speed,
    BulletSpeed,
    CritChance,
    DoubleBullet
}


public class Player : MonoBehaviour
{
    private Rigidbody2D playerRb;
    private BoxCollider2D boxCollider;
    private Animator anim;
    private SpriteRenderer _spriteRenderer;
    private Animator HealtAnim;
    private SpriteRenderer HealtSpriteRenderer;

    private float playerHorDir,
        facingDirection = 1f,
        lastShot,
        flashDuration = 0.1f,
        cooldownHealth = 2f,
        lastHit,
        invulnerableTime = 1.0f,
        damage = 1f,
        critChance = 0f;

    private int flashCount = 5, life = 3, coinCounter, coinUpgradeCount = 5, upgradeCount = 6;
    private bool canDoubleJump, doubleBullet = false, doubleJumpUpgraded = false;

    public static int enemieCounter = 0;
    
    [SerializeField] private float velocityX, cooldown, velocityY, doubleJumpVelocityY, shootVelocity;
    
    [SerializeField] private GameObject bulletPrefab; 
    [SerializeField] private Transform firePoint; 
    [SerializeField] private SpriteRenderer gunSprite;
    [SerializeField] private Transform healthBar;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private GameObject upgradeTextPrefab;
    [SerializeField] private BoxCollider2D groundCheck;
    [SerializeField] private LayerMask groundLayer;
    private Transform canvas;
    private GameOver gameOver;
    
    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        anim = GetComponentInChildren<Animator>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        canvas = FindFirstObjectByType<Canvas>().transform;
        gameOver = FindFirstObjectByType<GameOver>();
        
        HealtAnim = healthBar.GetComponent<Animator>();
        HealtSpriteRenderer = healthBar.GetComponent<SpriteRenderer>();
        HealtSpriteRenderer.enabled = false;
    }
    
    void Update()
    {
        playerRb.linearVelocity = new Vector2(playerHorDir * velocityX, playerRb.linearVelocity.y);
        Flip();
        HealthBarTime();
        UpdateAnimationState(); 
    }

    void UpdateAnimationState()
    {
        bool isGrounded = groundCheck.IsTouchingLayers(LayerMask.GetMask("Foreground"));
        
        anim.SetBool("IsRunning", playerHorDir != 0 && isGrounded);
        anim.SetBool("IsJumping", !isGrounded);
    }

    void Flip()
    {
        if (playerHorDir != 0)
        {
            facingDirection = Mathf.Sign(playerHorDir);
            transform.localScale = new Vector2(facingDirection, 1);
            healthBar.localScale = new Vector2(0.6f * Math.Min(facingDirection, 1), 0.6f);
        }
    }

    void OnMove(InputValue value)
    {
        playerHorDir = value.Get<Vector2>().x;
    }

    void OnJump(InputValue value)
    {
        bool isGrounded = groundCheck.IsTouchingLayers(LayerMask.GetMask("Foreground"));
        print(isGrounded);

        if (value.isPressed && isGrounded)
        {
            print("salve");
            playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, velocityY);
            canDoubleJump = true;
        } 
        else if (value.isPressed && canDoubleJump && doubleJumpUpgraded) 
        {
            anim.SetTrigger("JumpTrigger");
            playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, doubleJumpVelocityY);
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
        
        bulletInstance.Init(damage, critChance);
        bulletInstance.Shoot(facingDirection, shootVelocity);
        
        float delay = doubleBullet ? 0.1f : 0f;

        if (delay == 0) return;
        
        StartCoroutine(ShootWithDelay(delay, bulletRotation));
    }
    IEnumerator ShootWithDelay(float delay, Quaternion bulletRotation)
    {
        yield return new WaitForSeconds(delay);

        GameObject bulletObject = Instantiate(bulletPrefab, firePoint.position, bulletRotation);
        Bullet bulletInstance = bulletObject.GetComponent<Bullet>();
        
        bulletInstance.Init(damage, critChance);
        bulletInstance.Shoot(facingDirection, shootVelocity);
    }

    void Damage()
    {
        life--;
        
        HealtAnim.SetInteger("HealthAnim", life);
        HealtSpriteRenderer.enabled = true;

        lastHit = Time.time;

        StartCoroutine(FlashSprite());

        if (life <= 0)
        {
            gameOver.GameOverMenu(coinCounter, enemieCounter);
        }
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

        coinCounter++;
        scoreText.text = coinCounter.ToString();
        
        spawner.Notify();

        if (coinCounter % coinUpgradeCount == 0) Upgrade();

        Destroy(other.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (lastHit + invulnerableTime > Time.time) return;
        
        if (other.gameObject.CompareTag("Monster")) Damage();
    }

    private void HealthBarTime()
    {
        if (lastHit + cooldownHealth > Time.time) return;
        
        HealtSpriteRenderer.enabled = false;
    }

    private void Upgrade()
    {
        int min = doubleJumpUpgraded ? 1 : 0;
        UpgradeType upgradeSelected = (UpgradeType)UnityEngine.Random.Range(min, upgradeCount);

        string upgrade = "";
        
        switch (upgradeSelected)
        {
            case UpgradeType.DoubleJump:
                doubleJumpUpgraded = true;
                upgrade = "Double Jump";
                break;
            case UpgradeType.Damage:
                damage *= 1.25f;
                upgrade = "+25% dmg";
                break;
            case UpgradeType.Speed:
                velocityX *= 1.05f;
                upgrade = "+5% Speed";
                break;
            case UpgradeType.BulletSpeed:
                cooldown *= 0.9f;
                upgrade = "+10% Fire Rate";
                break;
            case UpgradeType.CritChance:
                critChance += 0.1f;
                upgrade = "+10% Crit Chance";
                break;
            case UpgradeType.DoubleBullet:
                doubleBullet = true;
                upgradeCount--;
                upgrade = "Double Bullet";
                break;
        }   
        
        GameObject dmgText = Instantiate(upgradeTextPrefab, transform.position + Vector3.up * 0.3f, Quaternion.identity, canvas); 
        
        dmgText.GetComponent<DamageText>().ShowUpgrade(upgrade);
    }
}