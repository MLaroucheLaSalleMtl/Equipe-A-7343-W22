using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float enemyHealth = 100f;

    void Start()
    {
        SetRigidbodyState(true);
        SetColliderStae(true);
    }
    public void DeducHealth(float deductHealth)
    {
        enemyHealth -= deductHealth;
        if(enemyHealth <= 0) { Die(); }
    }

   

    void SetRigidbodyState(bool state)
    {
        Rigidbody[] rb = GetComponentsInChildren<Rigidbody>();

        foreach (Rigidbody rigidbody in rb)
        {
            rigidbody.isKinematic = state;
        }
    }


    void SetColliderStae(bool state)
    {
        Collider[] coll = GetComponentsInChildren<Collider>();

        foreach (Collider collider in coll)
        {
            collider.enabled = state;
        }
    }

    void Die()
    {
        Destroy(gameObject, 3f);
        GetComponent<Animator>().enabled = false;
        SetRigidbodyState(false);
        SetColliderStae(true);
    }
}
