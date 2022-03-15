using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;



public class EnemieController : MonoBehaviour
{
    NavMeshAgent agent;
    public Transform target;
    public float distanceRation = 10f;
    public float attackRation = 1.5f;
    public enum AiState { idle, chasing, attacking};
    public AiState aiState = AiState.idle;

    public Animator anim;

    public Transform[] wayPoints;
    public int speed;

    private int wayPointsIndex;
    private float dist;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        StartCoroutine(Think());
        target = GameObject.FindGameObjectWithTag("Player").transform;

        wayPointsIndex = 0;
        transform.LookAt(wayPoints[wayPointsIndex].position);
        
    }
     void Update()
    {
        dist = Vector3.Distance(transform.position, wayPoints[wayPointsIndex].position);
        if(dist < 1f)
        {
            IncreaseIndex();
        }

        Patrol();
    }
    void Patrol()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    void IncreaseIndex()
    {
        wayPointsIndex++;
        if(wayPointsIndex >= wayPoints.Length)
        {
            wayPointsIndex = 0;
        }
        transform.LookAt(wayPoints[wayPointsIndex].position);
    }

    IEnumerator Think()
    {
        while(true)
        {
            switch (aiState)
            {
                case AiState.idle:
                    float dist = Vector3.Distance(target.position, transform.position);
                    if(dist < distanceRation)
                    {
                        aiState = AiState.chasing;
                        anim.SetBool("Chasing", true);
                    }
                    agent.SetDestination(transform.position);
                    break;
                case AiState.chasing:
                    dist = Vector3.Distance(target.position, transform.position);
                    if (dist > distanceRation)
                    {
                        aiState = AiState.idle;
                        anim.SetBool("Chasing", false);
                    }
                    if (dist < attackRation)
                    {
                        aiState = AiState.attacking;
                        anim.SetBool("Attacking", true);
                    }
                    agent.SetDestination(target.position);
                    break;
                case AiState.attacking:
                    Debug.Log("Attack");
                    agent.SetDestination(transform.position);
                    dist = Vector3.Distance(target.position, transform.position);
                    if (dist > attackRation)
                    {
                        aiState = AiState.chasing;
                        anim.SetBool("Attacking", false);
                    }
                    break;
                default:
                    break;
            }
            yield return new WaitForSeconds(0.2f);
        }
    }

    public void  Hit()
    {
        
    }
}

