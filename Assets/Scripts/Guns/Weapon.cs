using System;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    protected Transform firePoint;
    [SerializeField] protected GameObject bulletPrefab;
    [SerializeField] protected AudioClip pistolSound;
    [SerializeField] protected float damage, shootVelocity;

    private void Start()
    {
        firePoint = transform.Find("Shoot");
    }

    public abstract void shoot(Quaternion bulletRotation, float damage, float critChance,
                                float facingDirection, bool doubleBullet);
}
