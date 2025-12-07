using System;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D other)
    {
        print("srgsgsgse");
        
        if (!other.CompareTag("Water")) return;
        
        print("ACidoodoodododod");
        
        Destroy(gameObject);
    }
}
