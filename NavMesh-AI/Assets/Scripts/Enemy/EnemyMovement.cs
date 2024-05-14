
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

[RequireComponent(typeof(NavMeshAgent), typeof(AgentLinkMover))]
public class EnemyMovement : MonoBehaviour
{
    public Transform targetPlayer;

    public float updateSpeed = 0.1f; // How frequently to recalculate path based on target transform's position. 

    private NavMeshAgent _agent;
    private AgentLinkMover linkMover;
    [SerializeField] private Animator animator;
    private const string IsWalking = "isMoving";
    private const string Jump = "Jump";
    private const string Landed = "Landed";

    private Coroutine followCoroutine;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        linkMover = GetComponent<AgentLinkMover>();
        
        linkMover.onLinkStart += HandleLinkStart;
        linkMover.onLinkEnd += HandleLinkEnd;
    }

    private void HandleLinkStart()
    {
        animator.SetTrigger(Jump);
    }
    
    private void HandleLinkEnd()
    {
        animator.SetTrigger(Landed);
    }

    private void OnDestroy()
    {
        linkMover.onLinkStart -= HandleLinkStart;
        linkMover.onLinkEnd -= HandleLinkEnd;
    }
    public void StartChasing()
    {
        if (followCoroutine == null )
        {
            Debug.Log("Started chasing");
            followCoroutine = StartCoroutine(FollowTarget());
        }
        else
        {
            Debug.LogWarning("Called StartChasing on Enemy that is already chasing! " +
                             "This is likely a bug in some calling class!");
        }
    }

    private void Update()
    {
        animator.SetBool(IsWalking, _agent.velocity.magnitude > 0.1f);
    }

    private IEnumerator FollowTarget()
    {
        WaitForSeconds wait = new WaitForSeconds(updateSpeed);

        while (enabled)
        {
            _agent.SetDestination(targetPlayer.position);
            
            yield return wait;
        }
    }
}
