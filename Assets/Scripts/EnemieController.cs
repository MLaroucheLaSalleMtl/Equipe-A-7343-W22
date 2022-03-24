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
    public int damageAmount = 15;
    public enum AiState { idle, chasing, attacking};
    public AiState aiState = AiState.idle;

    public Animator anim;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        StartCoroutine(Think());
        target = GameObject.FindGameObjectWithTag("Player").transform;
        
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
                        PlayerDamage.TakeDamage(damageAmount);
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
