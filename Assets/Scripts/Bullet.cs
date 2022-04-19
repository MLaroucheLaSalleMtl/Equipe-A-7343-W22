using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Weapon wp;
    [SerializeField] private AudioSource bulletImpactAudioSource;
    [SerializeField] private GameObject[] bulletHole;

    float bulletDestroyWaitTime = 5f;
    WaitForSeconds waitTime;

    private void Awake()
    {
        wp = FindObjectOfType<Weapon>();
        waitTime = new WaitForSeconds(bulletDestroyWaitTime);
    }

    private void OnEnable()
    {
        transform.GetComponent<Rigidbody>().WakeUp();
        StartCoroutine(HideBullet());        
    }

    private void OnDisable()
    {
        transform.GetComponent<Rigidbody>().Sleep();
        StopCoroutine(HideBullet());
    }    

    private void OnCollisionEnter(Collision collision)
    {
        AddExplosiveForce();

        if (collision.collider.CompareTag("Ground"))
        {
            Quaternion rotFX = Quaternion.LookRotation(collision.contacts[0].normal);
            GameObject bullet = Instantiate(bulletHole[0], collision.contacts[0].point, rotFX);
            bullet.transform.parent = collision.collider.transform;
            wp.WeaponSO.PlayBulletImpactSFX(collision.collider.tag, bulletImpactAudioSource);
            gameObject.SetActive(false);
            Destroy(bullet, 2f);
        }
        if (collision.collider.CompareTag("Wood"))
        {
            Quaternion rotFX = Quaternion.LookRotation(collision.contacts[0].normal);
            GameObject bullet = Instantiate(bulletHole[1], collision.contacts[0].point, rotFX);
            bullet.transform.parent = collision.collider.transform;
            wp.WeaponSO.PlayBulletImpactSFX(collision.collider.tag, bulletImpactAudioSource);
            gameObject.SetActive(false);
            Destroy(bullet, 2f);
        }
        if (collision.collider.CompareTag("Sand"))
        {
            Quaternion rotFX = Quaternion.LookRotation(collision.contacts[0].normal);
            GameObject bullet = Instantiate(bulletHole[2], collision.contacts[0].point, rotFX);
            bullet.transform.parent = collision.collider.transform;
            gameObject.SetActive(false);
            wp.WeaponSO.PlayBulletImpactSFX(collision.collider.tag, bulletImpactAudioSource);
            Destroy(bullet, 2f);
        }
        if (collision.collider.CompareTag("Rocks"))
        {
            Quaternion rotFX = Quaternion.LookRotation(collision.contacts[0].normal);
            GameObject bullet = Instantiate(bulletHole[3], collision.contacts[0].point, rotFX);
            bullet.transform.parent = collision.collider.transform;
            wp.WeaponSO.PlayBulletImpactSFX(collision.collider.tag, bulletImpactAudioSource);
            gameObject.SetActive(false);
            Destroy(bullet, 2f);
        }
        if (collision.collider.CompareTag("Zombies"))
        {
            collision.collider.gameObject.GetComponent<EnemieController>().Hit();

            Quaternion rotFX = Quaternion.LookRotation(collision.contacts[0].normal);
            GameObject bullet = Instantiate(bulletHole[4], collision.contacts[0].point, rotFX);
            bullet.transform.parent = collision.collider.transform;
            wp.WeaponSO.PlayBulletImpactSFX(collision.collider.tag, bulletImpactAudioSource);
            gameObject.SetActive(false);
            Destroy(bullet, 2f);
        }
    }

    IEnumerator HideBullet()
    {
        yield return waitTime;
        gameObject.SetActive(false);
    }

    private void AddExplosiveForce()
    {
        Rigidbody rb; 
        rb = GetComponent<Rigidbody>();
        rb.AddForceAtPosition(rb.velocity, rb.position, ForceMode.Impulse);        
    }        
}
