using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D rb;
    public float damage = 1f;
    public float critChance = 0f;

    public void Init(float dmg, float crit)
    {
        damage = dmg;
        critChance = crit;
    }
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Shoot(float direction, float velocity, float yVelocity)
    {
        rb.linearVelocity = new Vector2(direction * velocity, yVelocity);
    }
    
    private void OnCollisionEnter2D(Collision2D  other)
    {
        if (other.gameObject.CompareTag("Coin")) return;
        
        if (other.gameObject.CompareTag("Monster"))
        {
            Monster monster = other.gameObject.GetComponent<Monster>();
            
            if (monster.life <= 0) return;
        }
        
        Destroy(gameObject);
    }

}