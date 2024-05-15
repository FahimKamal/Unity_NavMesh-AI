using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(AgentLinkMover))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField]
    private NavMeshAgent _agent;

    private AgentLinkMover linkMover;
    private RaycastHit[] _hits = new RaycastHit[1];
    [SerializeField] private Animator animator;
    private const string IsWalking = "isMoving";
    private const string Jump = "Jump";
    private const string Landed = "Landed";
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

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            var ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.RaycastNonAlloc(ray, _hits) > 0)
            {
                _agent.SetDestination(_hits[0].point);
            }
        }
        
        animator.SetBool(IsWalking, _agent.velocity.magnitude > 0.1f);
    }
    
    
}
