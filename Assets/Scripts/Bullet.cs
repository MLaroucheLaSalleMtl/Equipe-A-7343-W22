using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Weapon wp;
    [SerializeField] private AudioSource bulletImpactAudioSource;
    [SerializeField] private GameObject[] bulletHole;

    private void Awake()
    {
        wp = FindObjectOfType<Weapon>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        AddExplosiveForce();

        if (collision.collider.CompareTag("Ground"))
        {
            Quaternion rotFX = Quaternion.LookRotation(collision.contacts[0].normal);
            GameObject bullet = Instantiate(bulletHole[0], collision.contacts[0].point, rotFX);
            bullet.transform.parent = collision.collider.transform;
            gameObject.SetActive(false);
            wp.WeaponSO.PlayBulletImpactSFX(collision.collider.tag, bulletImpactAudioSource);
            Destroy(bullet, 2f);
        }
        if (collision.collider.CompareTag("Wood"))
        {
            Quaternion rotFX = Quaternion.LookRotation(collision.contacts[0].normal);
            GameObject bullet = Instantiate(bulletHole[1], collision.contacts[0].point, rotFX);
            bullet.transform.parent = collision.collider.transform;
            gameObject.SetActive(false);
            wp.WeaponSO.PlayBulletImpactSFX(collision.collider.tag, bulletImpactAudioSource);
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
            gameObject.SetActive(false);
            wp.WeaponSO.PlayBulletImpactSFX(collision.collider.tag, bulletImpactAudioSource);
            Destroy(bullet, 2f);
        }
        if (collision.collider.CompareTag("Zombies"))
        {
            collision.collider.gameObject.GetComponent<EnemieController>().Hit();

            Quaternion rotFX = Quaternion.LookRotation(collision.contacts[0].normal);
            GameObject bullet = Instantiate(bulletHole[4], collision.contacts[0].point, rotFX);
            bullet.transform.parent = collision.collider.transform;
            gameObject.SetActive(false);
            wp.WeaponSO.PlayBulletImpactSFX(collision.collider.tag, bulletImpactAudioSource);
            Destroy(bullet, 2f);
        }
        else
            Destroy(gameObject, 20f);

        //gameObject.SetActive(false);
    }

    private void AddExplosiveForce()
    {
        Rigidbody rb; 
        rb = GetComponent<Rigidbody>();
        rb.AddForceAtPosition(rb.velocity, rb.position, ForceMode.Impulse);
        //rb.AddExplosionForce(wp.WeaponSO.WeaponMaxDMG, transform.position, 0.05f, 0f, ForceMode.Impulse);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
