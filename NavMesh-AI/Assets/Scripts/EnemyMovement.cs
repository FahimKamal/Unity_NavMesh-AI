
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyMovement : MonoBehaviour
{
    public Transform target;

    public float updateSpeed = 0.1f; // How frequently to recalculate path based on target transform's position. 

    private NavMeshAgent _agent;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        StartCoroutine(FollowTarget());
    }

    private IEnumerator FollowTarget()
    {
        WaitForSeconds wait = new WaitForSeconds(updateSpeed);

        while (enabled)
        {
            _agent.SetDestination(target.position);
            
            yield return wait;
        }
    }
}
