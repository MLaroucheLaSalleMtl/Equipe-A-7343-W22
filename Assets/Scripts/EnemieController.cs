using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;




public class EnemieController : MonoBehaviour
{
    RigidBodyFPSController FPSController;

    NavMeshAgent agent;
    private Transform target;
    public float distanceRation = 10f;
    public float attackRation = 1.5f;
    public float damageAmount = 15;
    public enum AiState { idle, chasing, attacking};
    public AiState aiState = AiState.idle;

    public Animator anim;

    private void Awake()
    {
        //agent.Warp(transform.position);
        //agent.SetDestination(target.position);
    }

    void Start()
    {
        FPSController = FindObjectOfType<RigidBodyFPSController>();
        agent = GetComponent<NavMeshAgent>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(Think());
        //agent.Warp(transform.position);
    }
    void Update()
    {
        
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
                        FPSController.TakeDamage(damageAmount);                        
                    }
                    //agent.Warp(transform.position);
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

