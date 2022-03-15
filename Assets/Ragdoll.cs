using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    Rigidbody[] rb;
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponentsInChildren<Rigidbody>();
        anim = GetComponent<Animator>();
       DesactivateRagdoll();
    }

    public void DesactivateRagdoll()
    {
        foreach(var rigidBody in rb)
        {
            rigidBody.isKinematic = true;
        }
        anim.enabled = true;
    }

    public void ActivateRagdoll()
    {
        foreach(var rigidBody in rb)
        {
            rigidBody.isKinematic = false;
        }
        anim.enabled = false;
    }
}
