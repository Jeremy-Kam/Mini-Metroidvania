using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject bulletPrefab;

    [SerializeField] private float attackRate;
    private float nextAttackTime = 0f;

    [SerializeField] private float attackBufferTime;
    private float attackBufferCounter;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            attackBufferCounter = attackBufferTime;
        }
        else
        {
            attackBufferCounter -= Time.deltaTime;
        }

        if (attackBufferCounter > 0 && Time.time >= nextAttackTime)
        {
            Shoot();
            nextAttackTime = Time.time + (1f / attackRate);
        }
    }

    void Shoot()
    {
        FindObjectOfType<AudioManager>().Play("pistolShoot");
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }
}
