using System;
using UnityEngine;
using UnityEngine.AI;

public class PukekoAttackState : StateMachineBehaviour
{

    Transform player;
    NavMeshAgent agent;

    public float stopAttackingDistance = 2.5f;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // -- Inicialização -- //
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = animator.GetComponent<NavMeshAgent>();
    }


    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        if(SoundManager.Instance.pukekoChannel.isPlaying == false)
        {
            SoundManager.Instance.pukekoChannel.clip = SoundManager.Instance.pukekoAttack;
            SoundManager.Instance.pukekoChannel.PlayDelayed(1f);
        }

       LookAtPlayer();

        // -- Checar se o agente deve parar a perseguição -- //
        float distanceFromPlayer = Vector3.Distance(player.position, animator.transform.position);

        if (distanceFromPlayer > stopAttackingDistance)
        {
            animator.SetBool("isAttacking",false);
        }

    }

    private void LookAtPlayer()
    {
        Vector3 direction = player.position - agent.transform.position;
        agent.transform.rotation = Quaternion.LookRotation(direction);

        var yRotation = agent.transform.eulerAngles.y;
        agent.transform.rotation = Quaternion.Euler(0,yRotation,0);
    }

}
