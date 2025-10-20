using UnityEngine;

public class Monster : MonoBehaviour
{
    private BoxCollider2D boxCollider;
    private Rigidbody2D rb;
    private Animator anim;
    private float direction;
    public float life = 3f;
    private bool isDead = false;

    [SerializeField] private float speed;
    [SerializeField] private GameObject damageTextPrefab;
    private Transform canvas;

    void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        anim =  GetComponent<Animator>();
    }
    void Start()
    {
        canvas = FindFirstObjectByType<Canvas>().transform;
        if (transform.position.x < 0) direction = 1;
        else direction = -1;
    }

    void Update()
    {
        if (isDead) return;
        
        int intervals = Mathf.FloorToInt(Time.timeSinceLevelLoad / 20f);
        float speedMultiplier = Mathf.Pow(1.025f, intervals);
        float currentSpeed = speed * speedMultiplier;

        rb.linearVelocity = new Vector2(currentSpeed * direction, rb.linearVelocity.y);
    }

    private void LateUpdate()
    {
        float currentScaleX = transform.localScale.x;

        if (direction > 0 && currentScaleX > 0)
        {
            transform.localScale = new Vector2(-currentScaleX, transform.localScale.y);
        }
    }

    void Damage(float dmg, float critChance)
    {
        if (isDead) return;
        
        bool isCritical = Random.value < critChance;
        
        float finalDamage = isCritical ? dmg * 2 : dmg;
        
        life -=  finalDamage;

        GameObject dmgText = Instantiate(damageTextPrefab, transform.position + Vector3.up * 0.3f, Quaternion.identity, canvas); 
        
        dmgText.GetComponent<DamageText>().ShowDamage(finalDamage, isCritical);
        
        if (life <= 0)
        {
            anim.SetBool("IsDead", true);
            rb.bodyType = RigidbodyType2D.Static;
            Destroy(gameObject,1.1f);

            isDead = true;
        }
    }

    void Flip()
    {
        direction *= -1;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            Bullet bullet = other.GetComponent<Bullet>();
            Damage(bullet.damage, bullet.critChance);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (boxCollider.IsTouchingLayers(LayerMask.GetMask("Limit")))
        {
            Flip();
        }
    }
}