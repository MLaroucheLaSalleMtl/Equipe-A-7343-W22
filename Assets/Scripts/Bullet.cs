using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Weapon wp;

    private void Awake()
    {
        wp = GetComponentInParent<Weapon>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        AddExplosiveForce();
        gameObject.SetActive(false);
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
