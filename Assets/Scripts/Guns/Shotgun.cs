using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shotgun : Weapon
{
    
    public override void shoot(Quaternion bulletRotation, float damagePorcentange, float critChance,
        float facingDirection, bool doubleBullet)
    {
        Vector3 position;
        for (int i = 0; i < 3; i++)
        {
            position = firePoint.position;
            position.y += 0.2f - (0.2f * i);
            
            GameObject bulletObject = Instantiate(bulletPrefab, position, bulletRotation);
            Bullet bulletInstance = bulletObject.GetComponent<Bullet>();

            float finalDamage = damage * damagePorcentange;

            bulletInstance.Init(finalDamage, critChance);
            bulletInstance.Shoot(facingDirection, shootVelocity, 5f - (5f * i));

            AudioSource.PlayClipAtPoint(sound, transform.position, 0.08f);

            Destroy(bulletInstance.gameObject, 0.1f);
        }
    }
    
}
