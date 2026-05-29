using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PukekoPatrolingState : StateMachineBehaviour
{
    float timer;
    public float patrolingTime = 10f;

    Transform player;
    NavMeshAgent agent;

    public float detectionArea = 18f;
    public float patrolSpeed = 2f;

    List<Transform> waypointsList = new List<Transform>();

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       // -- Inicialização -- //
       player = GameObject.FindGameObjectWithTag("Player").transform;
       agent = animator.GetComponent<NavMeshAgent>();

       agent.speed = patrolSpeed;
       timer = 0;

       // -- Mover para o primeiro waypoint -- //
       GameObject waypointCluster = GameObject.FindGameObjectWithTag("Waypoints");
       foreach ( Transform t in waypointCluster.transform)
        {
            waypointsList.Add(t);
        }

        Vector3 nextPosition = waypointsList[Random.Range(0,waypointsList.Count)].position;
        agent.SetDestination(nextPosition);
        
    }


    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        if(SoundManager.Instance.pukekoChannel.isPlaying == false)
        {
            SoundManager.Instance.pukekoChannel.clip = SoundManager.Instance.pukekoWalking;
            SoundManager.Instance.pukekoChannel.PlayDelayed(1f);
        }

       // -- Checar se o agente ja chegou no waypoint -- //
       if(agent.remainingDistance <= agent.stoppingDistance)
        {
            agent.SetDestination(waypointsList[Random.Range(0,waypointsList.Count)].position);
        }

        // -- Checar se o tempo de patrulha acabou -- //
        timer += Time.deltaTime;
        if(timer > patrolingTime)
        {
            animator.SetBool("isPatroling", false);
        }

        // -- Transição para o estado de perseguição -- //
        float distanceFromPlayer = Vector3.Distance(player.position, animator.transform.position);
        if(distanceFromPlayer < detectionArea)
        {
            animator.SetBool("isChasing", true);
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       // parar o agente
       agent.SetDestination(agent.transform.position);
    }
}
