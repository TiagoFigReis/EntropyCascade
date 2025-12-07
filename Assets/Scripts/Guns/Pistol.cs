using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Pistol : Weapon
{
    
    public override void shoot(Quaternion bulletRotation, float damagePorcentange, float critChance,
        float facingDirection, bool doubleBullet)
    {
        GameObject bulletObject = Instantiate(bulletPrefab, firePoint.position, bulletRotation);
        Bullet bulletInstance = bulletObject.GetComponent<Bullet>();

        float finalDamage = damage * damagePorcentange;
        
        bulletInstance.Init(finalDamage, critChance);
        bulletInstance.Shoot(facingDirection, shootVelocity, 0f);
        
        AudioSource.PlayClipAtPoint(sound, transform.position, 0.30f);
        
        float delay = doubleBullet ? 0.1f : 0f;

        if (delay == 0) return;
        
        StartCoroutine(ShootWithDelay(delay, finalDamage, bulletRotation, critChance,
            facingDirection));
    }
    
    IEnumerator ShootWithDelay(float delay, float finalDamage, Quaternion bulletRotation, float critChance,
        float facingDirection)
    {
        yield return new WaitForSeconds(delay);

        GameObject bulletObject = Instantiate(bulletPrefab, firePoint.position, bulletRotation);
        Bullet bulletInstance = bulletObject.GetComponent<Bullet>();
        
        bulletInstance.Init(finalDamage, critChance);
        bulletInstance.Shoot(facingDirection, shootVelocity, 0f);
        
        AudioSource.PlayClipAtPoint(sound, transform.position, 0.30f);
    }
}
