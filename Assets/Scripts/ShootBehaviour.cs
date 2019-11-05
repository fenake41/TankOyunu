using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ShootBehaviour : MonoBehaviour
{
    public GameObject bulletPrefab; 
    public Transform bulletOrigin;
    float timeFromLastShoot;

    public void Shoot(float shootFreq)
    {
        if((timeFromLastShoot += Time.deltaTime) >= 1f/5f)
        {
            InstantiateBullet();
            timeFromLastShoot = 0;
        }
    }

    public void Shoot()
    {
        InstantiateBullet();
    }

    private void InstantiateBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab, bulletOrigin.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody>().AddForce(1100 * transform.forward);
    }
}