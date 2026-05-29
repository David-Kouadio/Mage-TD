using UnityEngine;
using UnityEngine.AI;

public class PukekoChaseState : StateMachineBehaviour
{

    NavMeshAgent agent;
    Transform player;

    public float chaseSpeed = 6f;

    public float stopChasingDistance = 21;
    public float attackingDistance = 2.5f;



    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // -- Inicialização -- //
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = animator.GetComponent<NavMeshAgent>();

        agent.speed = chaseSpeed;
    }


    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        if(SoundManager.Instance.pukekoChannel.isPlaying == false)
        {
            SoundManager.Instance.pukekoChannel.clip = SoundManager.Instance.pukekoChase;
            SoundManager.Instance.pukekoChannel.PlayDelayed(1f);
        }

        agent.SetDestination(player.position);
        animator.transform.LookAt(player);

        float distanceFromPlayer = Vector3.Distance(player.position, animator.transform.position);

        // -- Checar se o agente deve parar a perseguição -- //
        if (distanceFromPlayer > stopChasingDistance)
        {
            animator.SetBool("isChasing",false);
        }

        // -- Checar se o agente atacar -- //
        if (distanceFromPlayer < attackingDistance)
        {
            animator.SetBool("isAttacking",true);
        }        
    


    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       agent.SetDestination(animator.transform.position);
    }
}
