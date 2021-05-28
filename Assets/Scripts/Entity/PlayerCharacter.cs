using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerCharacter : Entity
{
    [Header("Movement")]
    [SerializeField] NavMeshAgent agent;

    Transform currentDestination;
    [SerializeField] List<Transform> destinations;  

    [Header("GameEffect")]
    [SerializeField] AudioSource audioSource;

    [Header("Animation")]
    [SerializeField] Animator animator;

    //[Header("Run state")]
    //[SerializeField] float runSpeed = 2f;
    
    // STATE MACHINE BEHAVIOUR
    public override void HandleStateBehaviour(int _stateBehaviourId)
    {
        if (onStateBehaviours.ContainsKey(_stateBehaviourId))
        {
            onStateBehaviours[_stateBehaviourId](this);
        }
    }

    // Enter = EntityState * 10
    // Update = EntityState * 10 + 1
    // Exit = EntityState * 10 + 2
    private delegate void OnStateBehaviour(PlayerCharacter _character);
    private static Dictionary<int, OnStateBehaviour> onStateBehaviours =
        new Dictionary<int, OnStateBehaviour>()
        {
            { (int) EntityState.Idle * 10, IdleEnter },
            { (int) EntityState.Idle * 10 + 1, IdleUpdate },
            { (int) EntityState.Walk * 10, WalkEnter },
            { (int) EntityState.Walk * 10 + 1, WalkUpdate },
        };

    #region Idle state
    [Header("Idle state")]
    [SerializeField] float idleTime = 3f;
    [SerializeField] float idleTimeRange = 1f;
    float idleCountdown;

    private static void IdleEnter(PlayerCharacter _playerCharacter)
    {
        _playerCharacter.IdleEnter();
    }

    private static void IdleUpdate(PlayerCharacter _playerCharacter)
    {
        _playerCharacter.IdleUpdate();
    }

    private void IdleEnter()
    {
        idleCountdown = idleTime + Random.Range(-idleTimeRange, idleTimeRange);
    }

    private void IdleUpdate()
    {
        idleCountdown -= Time.deltaTime;
        if (idleCountdown <= 0f)
        {
            animator.SetTrigger("Walk");
        }
    }
    #endregion

    #region Walk state
    [Header("Walk state")]
    [SerializeField] float walkSpeed = 1f;
    float walkCountdown;
    private static void WalkEnter(PlayerCharacter _playerCharacter)
    {
        _playerCharacter.WalkEnter();
    }

    private static void WalkUpdate(PlayerCharacter _playerCharacter)
    {
        _playerCharacter.WalkUpdate();
    }

    private void WalkEnter()
    {
        int random = Random.Range(0, destinations.Count);
        currentDestination = destinations[random];
        agent.speed = walkSpeed;
        agent.SetDestination(currentDestination.position);
    }   

    private void WalkUpdate()
    {
        if (!agent.hasPath)
        {
            animator.SetTrigger("Idle");
        }
    }
    #endregion
}
