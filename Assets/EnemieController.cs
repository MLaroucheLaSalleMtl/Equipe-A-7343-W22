using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class EnemieController : MonoBehaviour
{
    private NavMeshAgent agent;
    [SerializeField] private Transform target;
    Rigidbody rb;
    public float lookRadius = 10;
    public float radius;


    [SerializeField] private GameObject[] points;
    private int currPoint;

    //public Vector3 walkPointt;
    //bool walkPointSet;
    //public float walkPointRange;

    void Start()
    {
        target = PlayerManager.instance.player.transform;
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        currPoint = 0;
        agent.destination = points[currPoint].transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        ChasingPlayer();
        RotateToTarget();

        if (Vector3.Distance(this.transform.position, points[currPoint].transform.position) <= 2f)
        {
            Iterate();
        }
    }

    //private void Patroling()
    //{

    //}

    //private void SearchPlayer()
    //{

    //}

    private void ChasingPlayer()
    {
        float distance = Vector3.Distance(target.position, transform.position);
        if (distance <= lookRadius)
        {
            agent.SetDestination(target.position);
        }
    }

    void Iterate()
    {
        if (currPoint < points.Length - 1)
        {
            currPoint++;
        }
        else
        {
            currPoint = 0;
        }
        agent.destination = points[currPoint].transform.position;

    }


    private void RotateToTarget()
    {
        transform.LookAt(target);
        Vector3 direction = target.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = rotation;
    }



}

