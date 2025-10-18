using UnityEngine;

public class Monster : MonoBehaviour
{
    private BoxCollider2D boxCollider;
    private Rigidbody2D rb;
    private Animator anim;
    private float direction;
    public float life = 3;

    [SerializeField] private float speed;

    void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        anim =  GetComponent<Animator>();
    }
    void Start()
    {
        if (transform.position.x < 0) direction = 1;
        else direction = -1;
    }

    void Update()
    {
        int intervals = Mathf.FloorToInt(Time.timeSinceLevelLoad / 10f);
        float speedMultiplier = Mathf.Pow(1.05f, intervals);
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

    void Damage()
    {
        life--;

        if (life <= 0)
        {
            anim.SetBool("IsDead", true);
            rb.bodyType = RigidbodyType2D.Static;
            boxCollider.enabled = false;
            Destroy(gameObject,1.1f);
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
            Damage();
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