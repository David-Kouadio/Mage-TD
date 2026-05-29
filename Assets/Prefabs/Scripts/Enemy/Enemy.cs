using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int HP = 100;
    private Animator animator;

    private NavMeshAgent navMeshAgent;

    public bool isDead;

    void Start()
    {
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    public void TakeDamage(int damageAmout)
    {
        HP -= damageAmout;

        if(HP <= 0)
        {
            isDead = true;

            animator.SetTrigger("DIE1");

            // som de morte
            SoundManager.Instance.pukekoChannel.PlayOneShot(SoundManager.Instance.pukekoDeath);

            StartCoroutine(HideSelf(0.5f));
        }
        else
        {
            isDead = false;

            animator.SetTrigger("DAMAGE");

            // som de hit
            SoundManager.Instance.pukekoChannel.PlayOneShot(SoundManager.Instance.pukekoHurt);

        }


    }

    private IEnumerator HideSelf(float hide)
    {
        yield return new WaitForSeconds(hide);
        gameObject.SetActive(false);
        
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position,3.5f); // attacking // stop attacking

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position,4f); // Detection
        
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position,5f); // stop chasing
    }

}
