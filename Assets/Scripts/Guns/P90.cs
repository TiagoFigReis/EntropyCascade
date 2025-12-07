using UnityEngine;
using System.Collections;

public class P90 : Weapon
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
    }
}
