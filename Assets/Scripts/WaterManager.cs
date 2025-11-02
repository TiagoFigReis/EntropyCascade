using System;
using UnityEngine;

public class WaterManager : MonoBehaviour
{
    private float curY, acidGrowth = 0.001f;

    private void Awake()
    {
        curY = transform.position.y;
    }

    private void Update()
    {
        if (curY < transform.position.y) return;
        
        Vector2 curPosition = transform.position;

        curPosition.y += acidGrowth;
        
        transform.position = curPosition;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Monster")) return;

        curY += 0.2f;
    }
}
