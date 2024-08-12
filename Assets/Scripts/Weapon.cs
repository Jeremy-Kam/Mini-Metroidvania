using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject pistolBulletPrefab;
    [SerializeField] private GameObject magnumBulletPrefab;
    [SerializeField] private GameObject shotgunBulletPrefab;
    [SerializeField] private GameObject RPGBulletPrefab;

    [SerializeField] private PowerUp pistol;
    [SerializeField] private PowerUp magnum;
    [SerializeField] private PowerUp shotgun;
    [SerializeField] private PowerUp rocketLauncher;

    // 0 is pistol, 1 is magnum, 2 is shotgun, 3 is RPG
    [SerializeField] private float[] attackRate;
    private float nextAttackTime = 0f;

    [SerializeField] private float attackBufferTime;
    private float attackBufferCounter;

    [SerializeField] private IntVariable gunIndex;

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
            // The minus one is because gunIndex is 1-indexed, while the array is zero indexed
            nextAttackTime = Time.time + (1f / attackRate[gunIndex.GetValue() - 1]);
        }
    }

    void Shoot()
    {
        switch(gunIndex.GetValue())
        {
            case 1:
                if(pistol.GetValue())
                {
                    FindObjectOfType<AudioManager>().Play("pistolShoot");
                    Instantiate(pistolBulletPrefab, firePoint.position, firePoint.rotation);
                } else
                {
                    FindObjectOfType<AudioManager>().Play("noGun");
                }
                break;

            case 2:
                if (magnum.GetValue())
                {
                    FindObjectOfType<AudioManager>().Play("magnumShoot");
                    Instantiate(magnumBulletPrefab, firePoint.position, firePoint.rotation);
                } else
                {
                    FindObjectOfType<AudioManager>().Play("noGun");
                }
                break;

            case 3:
                if (shotgun.GetValue())
                {
                    FindObjectOfType<AudioManager>().Play("shotgunShoot");
                    Instantiate(shotgunBulletPrefab, firePoint.position, firePoint.rotation);
                } else
                {
                    FindObjectOfType<AudioManager>().Play("noGun");
                }
                break;

            case 4:
                if (rocketLauncher.GetValue())
                {
                    FindObjectOfType<AudioManager>().Play("rpgShoot");
                    Instantiate(RPGBulletPrefab, firePoint.position, firePoint.rotation);
                } else
                {
                    FindObjectOfType<AudioManager>().Play("noGun");
                }
                break;
        }
        
        
    }
}
