using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Shoot(float direction, float velocity)
    {
        rb.linearVelocity = new Vector2(direction * velocity, 0);
    }

    private void OnTriggerEnter2D(Collider2D other)
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