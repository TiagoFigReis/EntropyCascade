using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private Rigidbody2D playerRb;
    BoxCollider2D boxCollider;
    private float playerHorDir;
    bool canDoubleJump;
    [SerializeField] private float velocityX, velocityY;
    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
    }
    
    void Update()
    {
        Running();
    }

    void Running()
    {
        playerRb.linearVelocityX = playerHorDir * velocityX;
        
        bool isRunning = Mathf.Abs(playerRb.linearVelocityX) > 0;
        
        if(isRunning) Flip();
    }

    void Flip()
    {
        transform.localScale = new Vector2(Mathf.Sign(playerRb.linearVelocityX) * 4, 4);
    }

    void OnMove(InputValue value)
    {
        playerHorDir = value.Get<Vector2>().x;
    }

    void OnJump(InputValue value)
    {
        if (value.isPressed && boxCollider.IsTouchingLayers(LayerMask.GetMask("Foreground")))
        {
            playerRb.linearVelocityY = velocityY;
            canDoubleJump = true;
        }else if (canDoubleJump)
        {
            playerRb.linearVelocityY = velocityY;
            canDoubleJump = false;
        }
    }
}
