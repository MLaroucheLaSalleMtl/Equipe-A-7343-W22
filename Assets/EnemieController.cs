using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemieController : MonoBehaviour
{
    private NavMeshAgent agent = null;
    [SerializeField] private Transform target;
    Rigidbody rb;
    public float lookRadius = 10;
 
    void Start()
    { 
        target = PlayerManager.instance.player.transform;
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
  
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(target.position, transform.position);
        if( distance <=  lookRadius)
        {
            agent.SetDestination(target.position);
        }

        RotateToTarget();
    }

    private void RotateToTarget()
    {
        transform.LookAt(target);
        Vector3 direction = target.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = rotation;
    }
  

    //private void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawSphere(transform.position, lookRadius);
    //}

}
