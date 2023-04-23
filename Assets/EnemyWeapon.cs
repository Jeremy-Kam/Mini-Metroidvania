using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject bulletPrefab;

    public void Shoot()
    {
        // FindObjectOfType<AudioManager>().Play("pistolShoot");
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }
}
