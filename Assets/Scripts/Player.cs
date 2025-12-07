using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

enum UpgradeType
{
    DoubleJump,
    Damage,
    Speed,
    BulletSpeed,
    CritChance,
    LifePoint,
    DoubleBullet
}

public class Player : MonoBehaviour
{
    private Rigidbody2D playerRb;
    private Animator anim;
    private SpriteRenderer _spriteRenderer;
    private Animator HealtAnim;
    private SpriteRenderer HealtSpriteRenderer;
    private BoxCollider2D boxCollider;
    public static bool GamePaused;

    private float playerHorDir,
        facingDirection = 1f,
        lastShot,
        flashDuration = 0.1f,
        cooldownHealth = 2f,
        lastHit,
        invulnerableTime = 1.0f,
        damage = 1f,
        critChance = 0f;

    private int flashCount = 5, life = 3, coinCounter, coinUpgradeCount = 5, index;
    private bool canDoubleJump, doubleBullet = false, doubleJumpUpgraded = false, isShooting;
    private bool isDead = false;

    public static int enemieCounter;
    
    private List<UpgradeType> used = new List<UpgradeType>();
    
    [SerializeField] private float velocityX, cooldown, velocityY, doubleJumpVelocityY;
    
    [SerializeField] private GameObject bulletPrefab, PauseCanvas, pauseButtonUI; 
    [SerializeField] private Transform healthBar, WeaponPosition; 
    private SpriteRenderer gunSprite;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private GameObject upgradeTextPrefab;
    [SerializeField] private BoxCollider2D groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private AudioClip pistolSound, jumpSound, upgradeSound, coinSound;
    [SerializeField] private List<Weapon> gunList;
    
    private Weapon gun;
    
    private AudioSource audioSource, audioSourceCoin;
    private Transform canvas;
    private GameOver gameOver;
    
    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        canvas = FindFirstObjectByType<Canvas>().transform;
        gameOver = FindFirstObjectByType<GameOver>();
        boxCollider = GetComponent<BoxCollider2D>();

        enemieCounter = 0;
        
        GamePaused = false; 
        isShooting = false;
        isDead = false;
        Time.timeScale = 1f;
        lastShot = Time.time;
        
        if (pauseButtonUI != null) pauseButtonUI.SetActive(true);

        audioSource = GetComponent<AudioSource>();
        
        audioSource.volume = 0.015f;
        audioSource.clip = jumpSound;
        
        HealtAnim = healthBar.GetComponent<Animator>();
        HealtSpriteRenderer = healthBar.GetComponent<SpriteRenderer>();
        HealtSpriteRenderer.enabled = false;

        index = UnityEngine.Random.Range(0, gunList.Count);
        
        gun = Instantiate(gunList[index], WeaponPosition.transform.position, gunList[index].transform.rotation);
        
        gunSprite = gun.GetComponent<SpriteRenderer>();
        
        gun.transform.SetParent(transform);
    }
    
    void Update()
    {
        playerRb.linearVelocity = new Vector2(playerHorDir * velocityX, playerRb.linearVelocity.y);
        Flip();
        HealthBarTime();
        UpdateAnimationState(); 
        Shoot();
    }

    void UpdateAnimationState()
    {
        bool isGrounded = groundCheck.IsTouchingLayers(LayerMask.GetMask("Foreground"));
        
        anim.SetBool("IsRunning", playerHorDir != 0 && isGrounded);
        anim.SetBool("IsJumping", !isGrounded);
    }

    void OnPause()
    {
        if (isDead) return;
        
        Cursor.visible = true;

        if (GamePaused)
        {
            Time.timeScale = 1f;
            GamePaused = false;
            PauseCanvas.SetActive(false);
        }
        else
        {
            isShooting = false;
            Time.timeScale = 0f;
            GamePaused = true;
            PauseCanvas.SetActive(true);
        }
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
        if (GamePaused) return;
        
        playerHorDir = value.Get<Vector2>().x;
    }

    void OnJump(InputValue value)
    {
        if (GamePaused) return;
        
        bool isGrounded = groundCheck.IsTouchingLayers(LayerMask.GetMask("Foreground")) || groundCheck.IsTouchingLayers(LayerMask.GetMask("Monster"));

        audioSource.volume = 0.015f;
        
        if (value.isPressed && isGrounded)
        {
            playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, velocityY); 
            audioSource.PlayOneShot(jumpSound);
            canDoubleJump = true;
        } 
        else if (value.isPressed && canDoubleJump && doubleJumpUpgraded) 
        {
            anim.SetTrigger("JumpTrigger");
            audioSource.PlayOneShot(jumpSound);
            playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, doubleJumpVelocityY);
            canDoubleJump = false;
        }
    }

    void OnAttack(InputValue value)
    {
        if (GamePaused || isDead) return;
        
        if (EventSystem.current.IsPointerOverGameObject())
        {
            isShooting = false;
            return;
        }
        
        if (value.isPressed)
        {
            isShooting = true;
            return;
        }

        isShooting = false;
    }

    void Shoot()
    {
        if (lastShot + gun.cooldown * cooldown > Time.time || !isShooting) return;
        
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
        
        gun.shoot(bulletRotation, damage, critChance, facingDirection, doubleBullet);
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
            anim.SetTrigger("IsDead");
            GamePaused = true;
            StartCoroutine(PlayerDeath());
        }
    }

    private IEnumerator PlayerDeath()
    {
        isDead = true;
        if (pauseButtonUI != null) pauseButtonUI.SetActive(false);

        yield return new WaitForSecondsRealtime(1f);
        
        gameOver.GameOverMenu(coinCounter, enemieCounter);
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
        if (isDead) return;
        
        if (boxCollider.IsTouchingLayers(LayerMask.GetMask("Acid")))
        {
            StartCoroutine(WaterDeath());
            return;
        }
        
        if (!other.gameObject.CompareTag("Coin")) return;
        Destroy(other.gameObject);

        CoinSpawner spawner = FindFirstObjectByType<CoinSpawner>();

        coinCounter++;
        scoreText.text = coinCounter.ToString();
        
        spawner.Notify();

        if (coinCounter % coinUpgradeCount == 0)
        {
            Upgrade();
            audioSource.volume = 0.1f;
            audioSource.PlayOneShot(upgradeSound);
            return;
        }
        
        audioSource.volume = 0.015f;
        audioSource.PlayOneShot(coinSound);
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (lastHit + invulnerableTime > Time.time || isDead) return;

        if (other.gameObject.CompareTag("Fox") || other.gameObject.CompareTag("Monster") || other.gameObject.CompareTag("Monster2") || other.gameObject.CompareTag("Saw"))
        {
            Damage();
        }
    }
    
    private IEnumerator WaterDeath()
    {
        isDead = true;
        if (pauseButtonUI != null) pauseButtonUI.SetActive(false);
        
        yield return new WaitForSeconds(0.2f); 
        gameOver.GameOverMenu(coinCounter, enemieCounter);
    }

    private void HealthBarTime()
    {
        if (lastHit + cooldownHealth > Time.time) return;
        
        HealtSpriteRenderer.enabled = false;
    }

    private void Upgrade()
    {
        if (life == 3 && !used.Contains(UpgradeType.LifePoint))
        {
            used.Add(UpgradeType.LifePoint);
        }
        
        if (life < 3 && used.Contains(UpgradeType.LifePoint))
        {
            used.Remove(UpgradeType.LifePoint);
        }
        
        UpgradeType upgradeSelected = GetRandomEnumValue(used);

        string upgrade = "";
        
        switch (upgradeSelected)
        {
            case UpgradeType.DoubleJump:
                used.Add(UpgradeType.DoubleJump);
                doubleJumpUpgraded = true;
                upgrade = "Double Jump";
                break;
            case UpgradeType.Damage:
                damage *= 1.3f;
                upgrade = "+30% dmg";
                break;
            case UpgradeType.Speed:
                velocityX *= 1.1f;
                upgrade = "+10% Speed";
                break;
            case UpgradeType.BulletSpeed:
                cooldown *= 0.8f;
                upgrade = "+20% Fire Rate";
                break;
            case UpgradeType.CritChance:
                critChance += 0.1f;
                upgrade = "+10% Crit Chance";
                break;
            case UpgradeType.LifePoint:
                life++;
                upgrade = "+1 Life Point";
                break;
            case UpgradeType.DoubleBullet:
                used.Add(UpgradeType.DoubleBullet);
                doubleBullet = true;
                upgrade = "Pistol Double Bullet";
                break;
        }   
        
        GameObject dmgText = Instantiate(upgradeTextPrefab, transform.position + Vector3.up * 0.3f, Quaternion.identity, canvas); 
        
        dmgText.GetComponent<DamageText>().ShowUpgrade(upgrade);
        
        int newIndex = UnityEngine.Random.Range(0, gunList.Count);

        while (newIndex == index)
        {
            newIndex = UnityEngine.Random.Range(0, gunList.Count);
        }

        index = newIndex;
        
        Destroy(gun.gameObject);
        
        gun = Instantiate(gunList[index], WeaponPosition.transform.position, gunList[index].transform.rotation);
        gun.transform.SetParent(transform);
        
        gun.transform.localScale = new Vector3(3, 3, 1);
        
        gunSprite = gun.GetComponent<SpriteRenderer>();
    }
    
    private T GetRandomEnumValue<T>(List<T> used = null)
    {
        List<T> valores = Enum.GetValues(typeof(T)).Cast<T>().ToList();
        
        if (used != null && used.Count > 0)
            valores = valores.Except(used).ToList();
        
        if (valores.Count == 0)
            return default;
        
        int index = UnityEngine.Random.Range(0, valores.Count);
        return valores[index];
    }
}