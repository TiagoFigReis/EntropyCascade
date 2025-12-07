using System;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Water")) return;
        
        Destroy(gameObject);
    }
}
